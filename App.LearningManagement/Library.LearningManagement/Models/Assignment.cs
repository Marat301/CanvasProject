namespace Library.LearningManagement.Models {
    public class Assignment {
        public string? Name { get; set; }
        public string? Description { get; set; }
        public decimal TotalAvailablePoints { get; set; }
        public DateTime DueDate { get; set; }
    }
}
