using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.LearningManagement.Models {
    public class AssignmentGroup {
        public List<Assignment> Assignments { get; set; }
        public int ID { get; private set; }
        private static int lastID;

        public string Name { get; set; }

        private decimal weight;
        public decimal Weight {
            get { return weight; }
            set {
                if (value > 1) {
                    value /= 100;
                }
                weight = value;
            }
        }

        public AssignmentGroup() {
            Assignments = new List<Assignment>();
            Name = string.Empty;
            Weight = 1;

            ID = ++lastID;
        }

        public override string ToString() {
            return $"[{ID}] {Name} ({Weight}%)\n{string.Join("\n", Assignments.Select(s => s.ToString()).ToArray())}";
        }
    }
}