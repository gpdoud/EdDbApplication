using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

using EdDbLib;
using EdDbLib.Controllers;
using EdDbLib.Models;

namespace EdDbApplication {
    class Program {
        private const string Server = "localhost";
        private const string Instance = "sqlexpress";
        private const string Database = "EdDb";

        static void Main() {
            TestTransactions();
        } 
        #region Tests
        static void TestTransactions() {
            var conn = new Connection(Server, Instance, Database);
            var MajorCtrl = new MajorsController(conn);
            var major = new Major() { Code = "ZZZZ", Description = "ALL Zs", MinSat = 800 };
            MajorCtrl.BeginTransaction();
            MajorCtrl.Insert(major);
            MajorCtrl.RollbackTransaction();

        }
        static void TestClassesController() {
            var conn = new Connection(Server, Instance, Database);
            var ClsCtrl = new ClassesController(conn);

            var classes = ClsCtrl.GetAll();
            var class1 = ClsCtrl.GetByPK(1);
            var mat404 = ClsCtrl.GetByCode("MAT404");

            var cls = new Class() {
                Code = "UBW901", Subject = "Under Water Basket Weaving", Section = 901, InstructorId = null
            };
            var result = ClsCtrl.Insert(cls, "Tensi");
            cls = ClsCtrl.GetByCode("UBW901");
            cls.InstructorId = 1;
            result = ClsCtrl.Update(cls);
            result = ClsCtrl.Delete(cls.Id);

            conn.Close();
        }
        static void TestInstructorsController() {
            var conn = new Connection(Server, Instance, Database);
            var InstCtrl = new InstructorsController(conn);

            var albert = new Instructor() {
                Firstname = "Albert", Lastname = "Einstein", YearsExperience = 50, IsTenured = false
            };
            var result = InstCtrl.Insert(albert);
            albert = InstCtrl.GetByLastname("Einstein");
            albert.IsTenured = true;
            result = InstCtrl.Update(albert);
            result = InstCtrl.Delete(albert.Id);

            var instructor1 = InstCtrl.GetByPK(1);
            var instructors = InstCtrl.GetAll();
            
            conn.Close();
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
