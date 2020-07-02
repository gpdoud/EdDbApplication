using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;

namespace EdDbLib.Controllers {
    
    public class BaseController {

        public Connection Connection { get; protected set; } = null;

        public void BeginTransaction() {
            var cmd = new SqlCommand("BEGIN TRANSACTION", Connection.sqlConnection);
            var result = cmd.ExecuteNonQuery();
        }

        public void CommitTransaction() {
            var cmd = new SqlCommand("COMMIT TRANSACTION", Connection.sqlConnection);
            var result = cmd.ExecuteNonQuery();
        }

        public void RollbackTransaction() {
            var cmd = new SqlCommand("ROLLBACK TRANSACTION", Connection.sqlConnection);
            var result = cmd.ExecuteNonQuery();
        }

        protected bool CheckRowsAffected(int rowsAffected) {
            return rowsAffected switch
            {
                0 => false,
                1 => true,
                _ => throw new Exception("ERROR: Multiple rows affected")
            };
        }

        public BaseController(Connection connection) {
            Connection = connection;
        }

    }
}
