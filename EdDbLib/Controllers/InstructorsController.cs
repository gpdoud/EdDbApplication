using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;

using EdDbLib.Models;

namespace EdDbLib.Controllers {
    
    public class InstructorsController : BaseController {

        public bool Delete(int Id) {
            var cmd = new SqlCommand(Instructor.SqlDelete, Connection.sqlConnection);
            LoadSqlParameterInstructorId(cmd, Id);
            //cmd.Parameters.AddWithValue($"@{Instructor.ID}", Id);
            var rowsAffected = cmd.ExecuteNonQuery();
            return CheckRowsAffected(rowsAffected);
        }

        public bool Update(Instructor instructor) {
            var cmd = new SqlCommand(Instructor.SqlUpdate, Connection.sqlConnection);
            LoadSqlParameters(cmd, instructor);
            LoadSqlParameterInstructorId(cmd, instructor.Id);
            //cmd.Parameters.AddWithValue($"@{Instructor.ID}", instructor.Id);
            var rowsAffected = cmd.ExecuteNonQuery();
            return CheckRowsAffected(rowsAffected);
        }

        private void LoadSqlParameterInstructorId(SqlCommand cmd, int id) {
            cmd.Parameters.AddWithValue($"@{Instructor.ID}", id);
        }

        public bool Insert(Instructor instructor) {
            var cmd = new SqlCommand(Instructor.SqlInsert, Connection.sqlConnection);
            LoadSqlParameters(cmd, instructor);
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

        private void LoadSqlParameters(SqlCommand cmd, Instructor instructor) {
            cmd.Parameters.AddWithValue($"@{Instructor.FIRSTNAME}", instructor.Firstname);
            cmd.Parameters.AddWithValue($"@{Instructor.LASTNAME}", instructor.Lastname);
            cmd.Parameters.AddWithValue($"@{Instructor.YEARSEXPERIENCE}", instructor.YearsExperience);
            cmd.Parameters.AddWithValue($"@{Instructor.ISTENURED}", instructor.IsTenured ? 1 : 0);
        }

        public Instructor GetByLastname(string Lastname) {
            var cmd = new SqlCommand(Instructor.SqlGetByLastname, Connection.sqlConnection);
            cmd.Parameters.AddWithValue($"@{Instructor.LASTNAME}", Lastname);
            var reader = cmd.ExecuteReader();
            if(!reader.HasRows) {
                reader.Close();
                return null;
            }
            reader.Read();
            var instructor = LoadInstructor(reader);
            reader.Close();
            return instructor;
        }

        public Instructor GetByPK(int Id) {
            var cmd = new SqlCommand(Instructor.SqlGetByPK, Connection.sqlConnection);
            cmd.Parameters.AddWithValue($"@{Instructor.ID}", Id);
            var reader = cmd.ExecuteReader();
            if(!reader.HasRows) {
                reader.Close();
                return null;
            }
            reader.Read();
            var instructor = LoadInstructor(reader);
            reader.Close();
            return instructor;
        }

        public IEnumerable<Instructor> GetAll() {
            var cmd = new SqlCommand(Instructor.SqlGetAll, Connection.sqlConnection);
            var reader = cmd.ExecuteReader();
            var instructors = new List<Instructor>();
            while(reader.Read()) {
                var instructor = LoadInstructor(reader);
                instructors.Add(instructor);
            }
            reader.Close();
            return instructors;
        }

        private Instructor LoadInstructor(SqlDataReader reader) {
            var id = Convert.ToInt32(reader[Instructor.ID]);
            var instr = new Instructor(id);
            instr.Firstname = reader[Instructor.FIRSTNAME].ToString();
            instr.Lastname = reader[Instructor.LASTNAME].ToString();
            instr.YearsExperience = (int)reader[Instructor.YEARSEXPERIENCE];
            instr.IsTenured = (bool)reader[Instructor.ISTENURED];
            return instr;
        }

        public InstructorsController(Connection connection) : base(connection) {
        }
    }
}
