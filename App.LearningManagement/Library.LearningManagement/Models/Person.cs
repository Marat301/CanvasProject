namespace Library.LearningManagement.Models {
    public class Person {
        private static int lastID = 0;

        public int ID {
            get; private set;
        }

        public string Name { get; set; }

        public Person() {
            Name = string.Empty;
            ID = ++lastID;
        }

        public override string ToString() {
            return $"[{ID}] {Name}";
        }
    }
}