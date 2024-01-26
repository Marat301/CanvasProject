namespace Library.LearningManagement.Models {
    public class Person {
        public int ID { get; set; }
        public string Name { get; set; }
        public Dictionary<int, double> Grades { get; set; }
        public PersonClassification Classification { get; set; }

        public Person() {
            Name = string.Empty;
            Grades = new Dictionary<int, double>();
        }

        public override string ToString() {
            return $"[{ID}]{Name} - {Classification}";
        }
    }

    public enum PersonClassification {
        Freshman, Sophomore, Junior, Senior
    }
}

