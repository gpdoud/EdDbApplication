using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

using EdDbLib;

namespace EdDbApplication {
    class Program {
        static void Main() {
            TestStudentController();
        } 
        static void TestMajorsController() {
            var connection = new Connection("localhost", "sqlexpress", "EdDb");
            var majorsCtrl = new MajorsController(connection);

            var MajorUWBW = majorsCtrl.GetByCode("UWBW");

            //var newMajor = new Major {
            //    Code = "UWBW",
            //    Description = "Underwater Basket Weaving",
            //    MinSat = 1600
            //};
            //var success1 = majorsCtrl.Insert(newMajor);

            //var major = majorsCtrl.GetByPK(10);
            //major.Description = "A change by Greg";
            //var success = majorsCtrl.Update(major);

            //var success = majorsCtrl.Delete(9);

            //var majors = majorsCtrl.GetAll();
            //var major1 = majorsCtrl.GetByPK(1);
            //var major0 = majorsCtrl.GetByPK(0);
        }
        #region Tests
        static void TestStudentController() {
            var connection = new Connection("localhost", "sqlexpress", "EdDb");
            var studentsCtrl = new StudentsController(connection);

            var newStudent = new Student() {
                Firstname = "Fred",
                Lastname = "Flintstone",
                StateCode = "SA",
                SAT = 1000,
                GPA = 2.5m,
                MajorId = null
            };
            var itWorked = studentsCtrl.Insert(newStudent, "UWBW");

            //var fred = new Student(61) {
            //    Firstname = "Fredrick",
            //    Lastname = "Flintstone",
            //    StateCode = "SA",
            //    SAT = 1000,
            //    GPA = 2.5m,
            //    MajorId = null
            //};
            //var itWorked = studentsCtrl.Update(fred);

            //var itWorked = studentsCtrl.Delete(100);
            
            //var student = studentsCtrl.GetByPK(10);
            //var noStudent = studentsCtrl.GetByPK(-1);
            //var students = studentsCtrl.GetAll();
            
            connection.Close();
        }
        static void Test1() {

            var connStr = @"server=localhost\sqlexpress;database=EdDb;trusted_connection=true;";
            var sqlConn = new SqlConnection(connStr);
            sqlConn.Open();
            if(sqlConn.State != ConnectionState.Open) {
                throw new Exception("Connection did not open!");
            }

            var sql = "SELECT * From Student;";
            var sqlCmd = new SqlCommand(sql, sqlConn);
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

            sqlConn.Close();
        }
        #endregion
    }
}
