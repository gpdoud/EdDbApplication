using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;
using System.Linq;

using EdDbLib.Models;

namespace EdDbLib.Controllers {
    
    public class ClassesController : BaseController {

        public class ClassWithInstructor {
            public Class Class { get; set; }
            public Instructor Instructor { get; set; }
        }

        public IEnumerable<ClassWithInstructor> GetClassesWithInstructor() {
            var instCtrl = new InstructorsController(Connection);
            var classes = from c in GetAll()
                          join i in instCtrl.GetAll()
                          on c.InstructorId equals i.Id
                          select new ClassWithInstructor {
                              Class = c, Instructor = i
                          };
            return classes;
        }

        public bool Delete(int Id) {
            var cmd = new SqlCommand(Class.SqlDelete, Connection.sqlConnection);
            cmd.Parameters.AddWithValue("@Id", Id);
            var rowsAffected = cmd.ExecuteNonQuery();
            return CheckRowsAffected(rowsAffected);
        }

        public bool Update(Class cls) {
            var cmd = new SqlCommand(Class.SqlUpdate, Connection.sqlConnection);
            LoadSqlParameters(cmd, cls);
            cmd.Parameters.AddWithValue("@Id", cls.Id);
            var rowsAffected = cmd.ExecuteNonQuery();
            return CheckRowsAffected(rowsAffected);
        }

        public bool Insert(Class cls, string InstructorLastname) {
            var instCtrl = new InstructorsController(Connection);
            var inst = instCtrl.GetByLastname(InstructorLastname);
            if(inst == null) return false;
            cls.InstructorId = inst.Id;
            return Insert(cls);
        }
        public bool Insert(Class cls) {
            var cmd = new SqlCommand(Class.SqlInsert, Connection.sqlConnection);
            LoadSqlParameters(cmd, cls);
            var rowsAffected = cmd.ExecuteNonQuery();
            return CheckRowsAffected(rowsAffected);
        }

        //private bool CheckRowsAffected(int rowsAffected) {
        //    return rowsAffected switch
        //    {
        //        0 => false,
        //        1 => true,
        //        _ => throw new Exception("ERROR: Multiple rows affected")
        //    };
        //}

        private void LoadSqlParameters(SqlCommand cmd, Class cls) {
            cmd.Parameters.AddWithValue("@Code", cls.Code);
            cmd.Parameters.AddWithValue("@Subject", cls.Subject);
            cmd.Parameters.AddWithValue("@Section", cls.Section);
            cmd.Parameters.AddWithValue("@InstructorId", (object)cls.InstructorId ?? DBNull.Value);
        }

        public Class GetByCode(string Code) {
            var cmd = new SqlCommand(Class.SqlGetByCode, Connection.sqlConnection);
            cmd.Parameters.AddWithValue("@Code", Code);
            var reader = cmd.ExecuteReader();
            if(!reader.HasRows) {
                reader.Close();
                return null;
            }
            reader.Read();
            var cls = LoadClass(reader);
            reader.Close();
            return cls;
        }

        public Class GetByPK(int Id) {
            var cmd = new SqlCommand(Class.SqlGetByPK, Connection.sqlConnection);
            cmd.Parameters.AddWithValue("@Id", Id);
            var reader = cmd.ExecuteReader();
            if(!reader.HasRows) {
                reader.Close();
                return null;
            }
            reader.Read();
            var cls = LoadClass(reader);
            reader.Close();
            return cls;
        }

        public IEnumerable<Class> GetAll() {
            var cmd = new SqlCommand(Class.SqlGetAll, Connection.sqlConnection);
            var reader = cmd.ExecuteReader();
            var classes = new List<Class>();
            while(reader.Read()) {
                var cls = LoadClass(reader);
                classes.Add(cls);
            }
            reader.Close();
            return classes;
        }

        private Class LoadClass(SqlDataReader reader) {
            var id = Convert.ToInt32(reader["Id"]);
            var cls = new Class(id);
            cls.Code = reader["Code"].ToString();
            cls.Subject = reader["Subject"].ToString();
            cls.Section = (int)reader["Section"];
            cls.InstructorId = DBNull.Value.Equals(reader["InstructorId"]) 
                ? null : (int?)reader["InstructorId"];
            return cls;
        }

        public ClassesController(Connection connection) : base(connection) {
        }
    }
}
