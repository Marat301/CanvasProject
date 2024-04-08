namespace Library.LearningManagement.Models {
    public class Person {
        public int ID { get; set; }
        public string Name { get; set; }

        public Person() {
            Name = string.Empty;
        }

        public override string ToString() {
            return $"[{ID}]{Name}";
        }
    }
}

