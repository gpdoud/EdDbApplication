using System;

namespace EdDbLib {

    public class Student {

        public int Id { get; private set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public string StateCode { get; set; }
        public int SAT { get; set; }
        public decimal GPA { get; set; }
        public int? MajorId { get; set; }

        public Student(int id) {
            this.Id = id;
        }
        public Student() : this(0) { }

    }
}
