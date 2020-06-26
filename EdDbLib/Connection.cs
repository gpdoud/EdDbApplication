using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SqlClient;

namespace EdDbLib {
    
    public class Connection {

        public  SqlConnection sqlConnection { get; private set; } = null;
        private string connStr = null;

        public Connection(string server, string instance,
            string database) {
            connStr = $"server={server}\\{instance};" +
                            $"database={database};" +
                            "trusted_connection=true;";
            sqlConnection = new SqlConnection(connStr);
            sqlConnection.Open();
            if(sqlConnection.State != System.Data.ConnectionState.Open) {
                throw new Exception("Connection did not open -- Check the parameters.");
            }

        }
        public void Close() {
            if(sqlConnection != null) {
                sqlConnection.Close();
                sqlConnection = null;
            }
        }
    }
}
