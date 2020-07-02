using System;
using System.Collections.Generic;
using System.Text;

namespace EdDbLib.Models {
    
    public class Instructor {
#region Refactoring
        public const string ID = "Id";
        public const string FIRSTNAME = "Firstname";
        public const string LASTNAME = "Lastname";
        public const string YEARSEXPERIENCE = "YearsExperience";
        public const string ISTENURED = "IsTenured";

        public const string SqlGetAll = "SELECT * from Instructor;";
        public const string SqlGetByPK = "SELECT * from Instructor Where Id = @Id;";
        public const string SqlGetByLastname = "SELECT * from Instructor Where Lastname = @Lastname;";
        public const string SqlInsert = "INSERT Instructor " +
                                            "(Firstname, Lastname, YearsExperience, IsTenured) VALUES " +
                                            "(@Firstname, @Lastname, @YearsExperience, @IsTenured);";
        public const string SqlUpdate = "UPDATE Instructor Set " +
                                            "Firstname = @Firstname, " +
                                            "Lastname = @Lastname, " +
                                            "YearsExperience = @YearsExperience, " +
                                            "IsTenured = @IsTenured " +
                                            "Where Id = @Id;";
        public const string SqlDelete = "DELETE Instructor Where Id = @Id;";
#endregion

        public int Id { get; private set; } = 0;
        public string Firstname { get; set; } = string.Empty;
        public string Lastname { get; set; } = string.Empty;
        public int YearsExperience { get; set; } = 0;
        public bool IsTenured { get; set; } = false;

        public string Fullname { get => @"{Firstname} {Lastname}"; }

        public Instructor(int id) {
            this.Id = id;
        }
        public Instructor() : this(0) { }
    }
}
