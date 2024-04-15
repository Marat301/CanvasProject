using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.LearningManagement.Models {
    public class Submission {
        private static int lastID = 0;
        public int ID {
            get; private set;
        }

        public Student Student { get; set; }
        public Assignment Assignment { get; set; }
        public string Content { get; set; }

        public decimal Grade { get; set; }

        public Submission() {
            ID = ++lastID;
            Content = string.Empty;
        }

        public override string ToString() {
            return $"[{ID}] ({Grade}) {Student.Name}: {Assignment.Name}";
        }
    }
}