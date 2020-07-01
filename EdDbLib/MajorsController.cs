using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;

namespace EdDbLib {
    
    public class MajorsController {

        public Connection Connection { get; private set; } = null;

        public Major GetByCode(string Code) {
            var sql = $"SELECT * from Major Where Code = @Code;";
            var cmd = new SqlCommand(sql, Connection.sqlConnection);
            cmd.Parameters.AddWithValue("@Code", Code);
            var reader = cmd.ExecuteReader();
            if(!reader.HasRows) {
                reader.Close();
                return null;
            }
            // if we get here, we found a major

            // Read() to point to the one row
            reader.Read();
            var major = LoadMajorInstance(reader);
            // Close the SqlDataReader instance
            reader.Close();
            // Return the instance of Major
            return major;

        }

        public bool Insert(Major major) {
            var sql = "INSERT Major " +
                        "(Code, Description, MinSAT) " +
                        "VALUES (@Code, @Description, @MinSAT);";
            var cmd = CreateAndFillParameters(major, sql);
            var rowsAffected = cmd.ExecuteNonQuery();
            switch(rowsAffected) {
                case 0: return false;
                case 1: return true;
                default: throw new Exception($"ERROR: Inserted { rowsAffected } rows!");
            }
        }

        public bool Update(Major major) {
            var sql = "UPDATE Major Set " +
                        " Code = @Code, " +
                        " Description = @Description, " +
                        " MinSAT = @MinSAT " +
                        " Where Id = @Id; ";
            var cmd = CreateAndFillParameters(major, sql);
            cmd.Parameters.AddWithValue("@Id", major.Id);
            var rowsAffected = cmd.ExecuteNonQuery();
            switch(rowsAffected) {
                case 0: return false;
                case 1: return true;
                default: throw new Exception($"ERROR: Updated { rowsAffected } rows!");
            }
        }

        private SqlCommand CreateAndFillParameters(Major major, string sql) {
            var cmd = new SqlCommand(sql, Connection.sqlConnection);
            cmd.Parameters.AddWithValue("@Code", major.Code);
            cmd.Parameters.AddWithValue("@Description", major.Description);
            cmd.Parameters.AddWithValue("@MinSAT", major.MinSat);
            return cmd;
        }

        public bool Delete(int Id) {
            var sql = $"DELETE from Major Where Id = @Id;";
            var cmd = new SqlCommand(sql, Connection.sqlConnection);
            cmd.Parameters.AddWithValue("@Id", Id);

            try {
                // need to catch RefIntgrity exception
                int rowsAffected = cmd.ExecuteNonQuery();

                switch(rowsAffected) {
                    case 0: return false;
                    case 1: return true;
                    default: throw new Exception($"ERROR: Deleted { rowsAffected } rows!");
                }
            } catch(SqlException ex) {
                var refIntEx 
                    = new Exceptions.ReferentialIntegrityException("Cannot delete major used by student", ex);
                throw refIntEx;
            }
        }

        public Major GetByPK(int Id) {
            var sql = $"SELECT * from Major Where Id = {Id};";
            var cmd = new SqlCommand(sql, Connection.sqlConnection);
            var reader = cmd.ExecuteReader();
            if(!reader.HasRows) {
                reader.Close();
                return null;
            }
            // if we get here, we found a major

            // Read() to point to the one row
            reader.Read();
            var major = LoadMajorInstance(reader);
            // Close the SqlDataReader instance
            reader.Close();
            // Return the instance of Major
            return major;
        }

        public List<Major> GetAll() {
            // Connection has the SqlConnection instance and it is open
            // Create the SqlCommand passing the sql statement and 
            // open SqlConnection
            var sqlCmd = new SqlCommand(Major.SelectAll, Connection.sqlConnection);
            // Execute the sql statement and return result set in reader
            var reader = sqlCmd.ExecuteReader();
            // Create the collection class instance
            var majors = new List<Major>();
            // Read() moves to the next row and returns true
            // if no more rows, it returns false
            while(reader.Read()) {
                var major = LoadMajorInstance(reader);
                
                // add instance to the collection
                majors.Add(major);
            }
            // close the SqlDataReader
            reader.Close();
            // return the collection
            return majors;
        }

        private Major LoadMajorInstance(SqlDataReader reader) {
            var id = Convert.ToInt32(reader["Id"]);
            var major = new Major(id);
            major.Code = reader["Code"].ToString();
            major.Description = Convert.ToString(reader["Description"]);
            major.MinSat = Convert.ToInt32(reader["MinSAT"]);
            return major;
        }

        public MajorsController(Connection connection) {
            Connection = connection;
        }
    }
}
