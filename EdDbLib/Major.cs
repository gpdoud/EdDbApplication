using System;
using System.Collections.Generic;
using System.Reflection.Metadata.Ecma335;
using System.Text;

namespace EdDbLib {
    public class Major {

        public const string SelectAll = "SELECT * From Major;";
        public const string SelectByPk = "SELECT * from Major Where Id = {Id};";
        public const string Delete = "DELETE from Major Where Id = @Id;";

        public int Id { get; private set; } = 0;

        private string _code = string.Empty;
        public string Code { 
            get {
                return _code;
            }
            set {
                if(value.Length > 4) {
                    throw new Exception("Code length must be <= 4");
                }
                _code = value;
            } 
        }

        private string _description = string.Empty;
        public string Description {
            get { return _description; } 
            set {
                if(value.Length > 50) {
                    throw new Exception("Description length must be <= 50");
                }
                _description = value;
            }
        }

        private int _minSat = 400;
        public int MinSat {
            get => _minSat; 
            set {
                if(value < 400 || value > 1600) {
                    throw new Exception("MinSat must be between 400 and 1600");
                }
                _minSat = value;
            }
        }

        public Major(int id) {
            this.Id = id;
        }
        public Major() {}
    }
}
