using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;

namespace EdDbLib {
    
    public class StudentsController {

        public Connection Connection { get; private set; } = null;

        public bool Delete(int Id) {
            var sql = $"DELETE From Student Where Id = {Id}";
            var sqlCmd = new SqlCommand(sql, Connection.sqlConnection);
            var rowsAffected = sqlCmd.ExecuteNonQuery();
            if(rowsAffected != 1) {
                throw new Exception("Delete return result not 1");
            }
            // if we get here, it worked!
            return true;
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
            if(rowsAffected != 1) {
                throw new Exception("Update return result not 1");
            }
            // if we get here, it worked!
            return true;
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
            if(rowsAffected != 1) {
                throw new Exception("Insert return result not 1");
            }
            // if we get here, it worked!
            return true;
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

        public StudentsController(Connection connection) {
            this.Connection = connection;
        }
    }
}
