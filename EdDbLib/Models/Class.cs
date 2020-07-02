using System;
using System.Collections.Generic;
using System.Text;

namespace EdDbLib.Models {
    
    public class Class {

        public const string SqlGetAll = "SELECT * from Class;";
        public const string SqlGetByPK = "SELECT * from Class Where Id = @Id;";
        public const string SqlGetByCode = "SELECT * from Class Where Code = @Code;";
        public const string SqlInsert = "INSERT Class " +
                                            "(Code, Subject, Section, InstructorId) VALUES " +
                                            "(@Code, @Subject, @Section, @InstructorId);";
        public const string SqlUpdate = "UPDATE Class Set " +
                                            "Code = @Code, " +
                                            "Subject = @Subject, " +
                                            "Section = @Section, " +
                                            "InstructorId = @InstructorId " +
                                            "Where Id = @Id;";
        public const string SqlDelete = "DELETE Class Where Id = @Id;";

        public int Id { get; private set; } = 0;
        public string Code { get; set; } = string.Empty;
        public string Subject { get; set; } = string.Empty;
        public int Section { get; set; } = 0;
        public int? InstructorId { get; set; } = null;

        public Class(int id) {
            this.Id = id;
        }
        public Class() : this(0) { }
    }
}
