﻿using Library.LearningManagement.Models;
using Library.LearningManagement.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.LearningManagement.Helpers
{
    internal class StudentHelper
    {
        private StudentService studentService = new StudentService();
        public void CreateStudentRecord(Person selectedStudent = null)
        {
            Console.WriteLine("What is the name of the student?");
            var name = Console.ReadLine();

            Console.WriteLine("What is the ID of the student?");
            var id = Console.ReadLine();

            Console.WriteLine("What is the classification of the student? [(F)reshman, S(O)phomore, (J)unior, (S)enior]");
            var classification = Console.ReadLine() ?? string.Empty;

            PersonClassification classEnum;

            if (classification.Equals("O", StringComparison.InvariantCultureIgnoreCase)) {
                classEnum = PersonClassification.Sophomore;
            } else if (classification.Equals("J", StringComparison.InvariantCultureIgnoreCase)) {
                classEnum = PersonClassification.Junior;
            } else if (classification.Equals("S", StringComparison.InvariantCultureIgnoreCase)) {
                classEnum = PersonClassification.Senior;
            } else {
                classEnum = PersonClassification.Freshman;
            }

            bool isCreate = false;
            if (selectedStudent == null)
            {
                isCreate = true;
                selectedStudent = new Person();
            }
            selectedStudent.ID = int.Parse(id ?? "0");
            selectedStudent.Name = name ?? string.Empty;
            selectedStudent.Classification = classEnum;

            if (isCreate)
            {
                studentService.Add(selectedStudent);
            }
        }

        public void UpdateStudentRecord()
        {
            Console.WriteLine("Select the student to update");
            ListStudents();

            var selectionStr = Console.ReadLine();

            if (int.TryParse(selectionStr, out int selectionInt))
            {
                var selectedStudent = studentService.Students.FirstOrDefault(s => s.ID == selectionInt);
                if (selectedStudent != null)
                {
                    CreateStudentRecord(selectedStudent);
                }
            }
        }

        public void ListStudents()
        {
            studentService.Students.ForEach(Console.WriteLine);
        }

        public void SearchStudents()
        {
            Console.WriteLine("Enter the student: ");
            var query = Console.ReadLine() ?? string.Empty;

            studentService.Search(query).ToList().ForEach(Console.WriteLine);
        }
    }
}
