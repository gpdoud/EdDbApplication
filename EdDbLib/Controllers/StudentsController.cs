using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

using EdDbLib.Controllers;

namespace EdDbLib {
    
    public class StudentsController : BaseController {
        
        public struct StudentsPerState {
            public string StateCode { get; set; }
            public int Count { get; set; }
        }
        public struct StudentAndMajor {
            public int Id { get; set; }
            public string Fullname { get; set; }
            public string Major { get; set; }
        }

        public IEnumerable<StudentAndMajor> GetStudentWithMajor() {
            var majCtrl = new MajorsController(Connection);
            var students = from s in GetAll()
                           join m in majCtrl.GetAll()
                           on s.MajorId equals m.Id into sm
                           from s2 in sm.DefaultIfEmpty()
                           select new StudentAndMajor {
                               Id = s.Id, 
                               Fullname = $"{s.Firstname} {s.Lastname}", 
                               Major = s2?.Description ?? "Undeclared"
                           };
            return students;            
        }

        public IEnumerable<StudentsPerState> GetStudentsPerState() {

            var studentsPerState = from s in GetAll()
                                   group s by s.StateCode into sc
                                   select new StudentsPerState {
                                       StateCode = sc.Key, Count = sc.Count()
                                   };
            return studentsPerState;
        }

        public IEnumerable<Student> GetByLastname(string Lastname) {
            var students = GetAll();
            var someStudents = from s in students
                               where s.Lastname.StartsWith(Lastname)
                               orderby s.Lastname
                               select s;
            return someStudents;
        }

        public bool Delete(int Id) {
            var sql = $"DELETE From Student Where Id = {Id}";
            var sqlCmd = new SqlCommand(sql, Connection.sqlConnection);
            var rowsAffected = sqlCmd.ExecuteNonQuery();
            return CheckRowsAffected(rowsAffected);
        }

        public bool Update(Student student) {
            var majorid = (student.MajorId == null) 
                ? " NULL " 
                : $" {student.MajorId} ";

            var sql = $"UPDATE Student Set " +
                        $" Firstname = '{student.Firstname}', " +
                        $" Lastname = '{student.Lastname}', " +
                        $" StateCode = '{student.StateCode}', " +
                        $" SAT = {student.SAT}, " +
                        $" GPA = {student.GPA}, " +
                        $" MajorId = {majorid} " +
                        $" Where Id = {student.Id}; ";

            var sqlCmd = new SqlCommand(sql, Connection.sqlConnection);
            var rowsAffected = sqlCmd.ExecuteNonQuery();
            return CheckRowsAffected(rowsAffected);
        }

        public bool Insert(Student student, string MajorCode) {
            var majorCtrl = new MajorsController(this.Connection);
            var major = majorCtrl.GetByCode(MajorCode);
            student.MajorId = major?.Id;
            return Insert(student);
        }

        public bool Insert(Student student) {
            var majorid = (student.MajorId == null)
                ? " NULL "
                : $" {student.MajorId} ";

            var sql = $"INSERT Student " +
                        " (Firstname, Lastname, StateCode, SAT, GPA, MajorId) " +
                        " VALUES " +
                        $" ('{student.Firstname}', '{student.Lastname}', " +
                        $" '{student.StateCode}', {student.SAT}, {student.GPA}, " +
                        $" {majorid});";
            var sqlCmd = new SqlCommand(sql, Connection.sqlConnection);
            var rowsAffected = sqlCmd.ExecuteNonQuery();
            return CheckRowsAffected(rowsAffected);
        }

        public Student GetByPK(int Id) {
            var sql = $"SELECT * From Student Where Id = {Id}";
            var sqlCmd = new SqlCommand(sql, Connection.sqlConnection);
            var reader = sqlCmd.ExecuteReader();
            if(!reader.HasRows) {
                reader.Close();
                return null;
            }
            // if we get to here, we found a student
            reader.Read();
            var id = Convert.ToInt32(reader["Id"]);
            var student = new Student(id);
            student.Firstname = reader["Firstname"].ToString();
            student.Lastname = reader["Lastname"].ToString();
            student.StateCode = reader["StateCode"].ToString();
            student.SAT = Convert.ToInt32(reader["SAT"]);
            student.GPA = Convert.ToDecimal(reader["GPA"]);
            student.MajorId = null;
            if(!reader["MajorId"].Equals(DBNull.Value)) {
                student.MajorId = Convert.ToInt32(reader["MajorId"]);
            }
            reader.Close();
            return student;
        }

        public List<Student> GetAll() {
            var sql = "SELECT * From Student;";
            var sqlCmd = new SqlCommand(sql, Connection.sqlConnection);
            var reader = sqlCmd.ExecuteReader();
            var students = new List<Student>(60);
            while(reader.Read()) {
                var id = Convert.ToInt32(reader["Id"]);
                var student = new Student(id);
                student.Firstname = reader["Firstname"].ToString();
                student.Lastname = reader["Lastname"].ToString();
                student.StateCode = reader["StateCode"].ToString();
                student.SAT = Convert.ToInt32(reader["SAT"]);
                student.GPA = Convert.ToDecimal(reader["GPA"]);
                student.MajorId = null;
                if(!reader["MajorId"].Equals(DBNull.Value)) {
                    student.MajorId = Convert.ToInt32(reader["MajorId"]);
                }
                students.Add(student);
            }
            reader.Close();
            return students;
        }

        public StudentsController(Connection connection) : base(connection) {
        }
    }
}
