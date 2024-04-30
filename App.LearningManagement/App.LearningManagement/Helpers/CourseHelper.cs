using Library.LearningManagement.Models;
using Library.LearningManagement.Services;

namespace App.LearningManagement.Helpers {
    public class CourseHelper {
        private CourseService courseService;
        private StudentService studentService;

        // use the current services (singleton implementation)
        public CourseHelper() {
            studentService = StudentService.Current;
            courseService = CourseService.Current;
        }

        // creates a course
        public void CreateCourseRecord(Course? selectedCourse = null) {
            // check if we are updating or creating a course
            bool isNewCourse = false;
            if (selectedCourse == null) {
                isNewCourse = true;
                selectedCourse = new Course();
            }

            var choice = "Y";

            // Get course code
            if (!isNewCourse) {
                Console.WriteLine("Do you want to update the course code? (Y/N)");
                choice = Console.ReadLine() ?? "N";
            }

            if (choice.Equals("Y", StringComparison.InvariantCultureIgnoreCase)) {
                Console.WriteLine("What is the code for the course?");
                selectedCourse.Prefix = Console.ReadLine() ?? string.Empty;
            }

            // Get course name
            if (!isNewCourse) {
                Console.WriteLine("Do you want to update the course name? (Y/N)");
                choice = Console.ReadLine() ?? "N";
            } else {
                choice = "Y";
            }

            if (choice.Equals("Y", StringComparison.InvariantCultureIgnoreCase)) {
                Console.WriteLine("What is the name for the course?");
                selectedCourse.Name = Console.ReadLine() ?? string.Empty;
            }

            // Get course description
            if (!isNewCourse) {
                Console.WriteLine("Do you want to update the course description? (Y/N)");
                choice = Console.ReadLine() ?? "N";
            } else {
                choice = "Y";
            }

            if (choice.Equals("Y", StringComparison.InvariantCultureIgnoreCase)) {
                Console.WriteLine("What is the description of the course?");
                selectedCourse.Description = Console.ReadLine() ?? string.Empty;
            }

            if (isNewCourse) {
                SetupRoster(selectedCourse);
                SetupAssignments(selectedCourse);
                SetupModules(selectedCourse);
            }

            if (isNewCourse) {
                courseService.Add(selectedCourse);
            }
        }

        // updates a course
        public void UpdateCourseRecord() {
            Console.WriteLine("Enter the course code to update");
            courseService.Courses.ForEach(Console.WriteLine);

            var selection = Console.ReadLine();

            // get course whose ID matches given ID
            var selectedCourse = courseService.Courses.FirstOrDefault
                (s => s.Code.Equals(selection, StringComparison.InvariantCultureIgnoreCase));

            // reset all the course's data
            if (selectedCourse != null) {
                CreateCourseRecord(selectedCourse);
            }
        }

        // search for course and print all their data
        public void SearchCourses(string? query = null) {
            if (string.IsNullOrEmpty(query)) {
                courseService.Courses.ForEach(Console.WriteLine);
            } else {
                // find and print the course
                courseService.Search(query).ToList().ForEach(Console.WriteLine);
            }

            Console.WriteLine("Select a course:");
            var code = Console.ReadLine() ?? string.Empty;

            var selectedCourse = courseService.Courses.FirstOrDefault
                (c => c.Code.Equals(code, StringComparison.InvariantCultureIgnoreCase));

            if (selectedCourse != null) {
                Console.WriteLine(selectedCourse.DetailDisplay);
            }

        }

        // adds a student to the selected course
        public void AddStudent() {
            Console.WriteLine("Enter the course code to add the student:");
            courseService.Courses.ForEach(Console.WriteLine);
            var selection = Console.ReadLine();

            var selectedCourse = courseService.Courses.FirstOrDefault
                (s => s.Code.Equals(selection, StringComparison.InvariantCultureIgnoreCase));

            if (selectedCourse != null) {
                studentService.Students.Where(s => !selectedCourse.Roster.Any
                (s2 => s2.ID == s.ID)).ToList().ForEach(Console.WriteLine);

                if (studentService.Students.Any(s => !selectedCourse.Roster.Any
                (s2 => s2.ID == s.ID))) {
                    selection = Console.ReadLine() ?? string.Empty;
                }

                if (selection != null) {
                    var selectedId = int.Parse(selection);
                    var selectedStudent = studentService.Students.FirstOrDefault(s => s.ID == selectedId);
                    if (selectedStudent != null) {
                        selectedCourse.Roster.Add(selectedStudent);
                    }
                }

            }
        }

        // removes a student from the selected course
        public void RemoveStudent() {
            Console.WriteLine("Enter the course code to remove the student:");
            courseService.Courses.ForEach(Console.WriteLine);
            var selection = Console.ReadLine();

            var selectedCourse = courseService.Courses.FirstOrDefault(s => s.Code.Equals(selection, StringComparison.InvariantCultureIgnoreCase));
            if (selectedCourse != null) {
                selectedCourse.Roster.ForEach(Console.WriteLine);
                if (selectedCourse.Roster.Any()) {
                    selection = Console.ReadLine() ?? string.Empty;
                } else {
                    selection = null;
                }

                if (selection != null) {
                    var selectedId = int.Parse(selection);
                    var selectedStudent = studentService.Students.FirstOrDefault(s => s.ID == selectedId);
                    if (selectedStudent != null) {
                        selectedCourse.Roster.Remove(selectedStudent);
                    }
                }

            }
        }

        // adds an assignment to the selected course.
        public void AddAssignment() {
            Console.WriteLine("Enter the course code that you would like to add the assignment to:");
            courseService.Courses.ForEach(Console.WriteLine);
            var selection = Console.ReadLine();

            var selectedCourse = courseService.Courses.FirstOrDefault(s => s.Code.Equals(selection, StringComparison.InvariantCultureIgnoreCase));
            if (selectedCourse != null) {
                CreateAssignmentWithGroup(selectedCourse);
            }
        }

        // adds a submission to the selected course
        public void AddSubmission() {
            Console.WriteLine("Enter the course code that you would like to add the assignment to:");
            courseService.Courses.ForEach(Console.WriteLine);
            var selection = Console.ReadLine();

            var selectedCourse = courseService.Courses.FirstOrDefault(s => s.Code.Equals(selection, StringComparison.InvariantCultureIgnoreCase));
            if (selectedCourse != null) {
                Console.WriteLine("Enter the ID of the student:");
                selectedCourse.Roster.Where(r => r is Student).ToList().ForEach(Console.WriteLine);
                var selectedStudentId = int.Parse(Console.ReadLine() ?? "0");
                var selectedStudent = selectedCourse.Roster.FirstOrDefault(s => s.ID == selectedStudentId);

                Console.WriteLine("Enter the ID of the assignment:");
                selectedCourse.Assignments.ToList().ForEach(Console.WriteLine);
                var selectedAssignmentId = int.Parse(Console.ReadLine() ?? "0");
                var selectedAssignment = selectedCourse.Assignments.FirstOrDefault(a => a.ID == selectedAssignmentId);

                CreateSubmission(selectedCourse, selectedStudent as Student, selectedAssignment);
            }
        }

        // adds an assignment in a group
        private void CreateAssignmentWithGroup(Course selectedCourse) {
            if (selectedCourse.AssignmentGroups.Any()) {
                Console.WriteLine("[0] Add a new group");
                selectedCourse.AssignmentGroups.ForEach(Console.WriteLine);

                var selectionStr = Console.ReadLine() ?? string.Empty;
                var selectionInt = int.Parse(selectionStr);

                if (selectionInt == 0) {
                    var newGroup = new AssignmentGroup();
                    Console.WriteLine("Group Name:");
                    newGroup.Name = Console.ReadLine() ?? string.Empty;
                    Console.WriteLine("Group Weight:");
                    newGroup.Weight = decimal.Parse(Console.ReadLine() ?? "1");

                    newGroup.Assignments.Add(CreateAssignment());
                    selectedCourse.AssignmentGroups.Add(newGroup);
                } else if (selectionInt != 0) {
                    var selectedGroup = selectedCourse.AssignmentGroups.FirstOrDefault(g => g.ID == selectionInt);
                    if (selectedGroup != null) {
                        selectedGroup.Assignments.Add(CreateAssignment());
                    }
                }

            } else {
                var newGroup = new AssignmentGroup();
                Console.WriteLine("Group Name:");
                newGroup.Name = Console.ReadLine() ?? string.Empty;
                Console.WriteLine("Group Weight:");
                newGroup.Weight = decimal.Parse(Console.ReadLine() ?? "1");

                newGroup.Assignments.Add(CreateAssignment());
                selectedCourse.AssignmentGroups.Add(newGroup);
            }
        }

        // adds a module to the selected course
        public void AddModule() {
            Console.WriteLine("Enter the course code that you would like to add the module to:");
            courseService.Courses.ForEach(Console.WriteLine);
            var selection = Console.ReadLine();

            var selectedCourse = courseService.Courses.FirstOrDefault(s => s.Code.Equals(selection, StringComparison.InvariantCultureIgnoreCase));
            if (selectedCourse != null) {
                selectedCourse.Modules.Add(CreateModule(selectedCourse));
            }
        }

        // adds an announcement to the selected course
        public void AddAnnouncement() {
            Console.WriteLine("Enter the course code that you would like to add the announcement to:");
            courseService.Courses.ForEach(Console.WriteLine);
            var selection = Console.ReadLine();

            var selectedCourse = courseService.Courses.FirstOrDefault(s => s.Code.Equals(selection, StringComparison.InvariantCultureIgnoreCase));
            if (selectedCourse != null) {
                selectedCourse.Announcements.Add(CreateAnnouncement(selectedCourse));
            }
        }

        // removes a module from the selected course.
        public void RemoveModule() {
            Console.WriteLine("Enter the course code:");
            courseService.Courses.ForEach(Console.WriteLine);
            var selection = Console.ReadLine();

            var selectedCourse = courseService.Courses.FirstOrDefault(s => s.Code.Equals(selection, StringComparison.InvariantCultureIgnoreCase));

            if (selectedCourse != null) {
                Console.WriteLine("Choose a module to delete:");
                selectedCourse.Modules.ForEach(Console.WriteLine);
                var selectionStr = Console.ReadLine() ?? string.Empty;
                var selectionInt = int.Parse(selectionStr);
                var selectedModule = selectedCourse.Modules.FirstOrDefault(m => m.ID == selectionInt);
                if (selectedModule != null) {
                    selectedCourse.Modules.Remove(selectedModule);
                }
            }
        }

        public void UpdateModule() {
            Console.WriteLine("Enter the course code:");
            courseService.Courses.ForEach(Console.WriteLine);
            var selection = Console.ReadLine();

            var selectedCourse = courseService.Courses.FirstOrDefault(s => s.Code.Equals(selection, StringComparison.InvariantCultureIgnoreCase));

            if (selectedCourse != null && selectedCourse.Modules.Any()) {
                Console.WriteLine("Enter the ID of the module to update:");
                selectedCourse.Modules.ForEach(Console.WriteLine);

                selection = Console.ReadLine();
                var selectedModule = selectedCourse
                    .Modules
                    .FirstOrDefault(m => m.ID.ToString().Equals(selection, StringComparison.InvariantCultureIgnoreCase));

                if (selectedModule != null) {
                    // modify the name of the module
                    Console.WriteLine("Would you like to modify the name of the module?");
                    selection = Console.ReadLine();
                    if (selection?.Equals("Y", StringComparison.InvariantCultureIgnoreCase) ?? false) {
                        Console.WriteLine("Name:");
                        selectedModule.Name = Console.ReadLine();
                    }

                    // modify the description of the module
                    Console.WriteLine("Would you like to modify the description of the module?");
                    selection = Console.ReadLine();
                    if (selection?.Equals("Y", StringComparison.InvariantCultureIgnoreCase) ?? false) {
                        Console.WriteLine("Description:");
                        selectedModule.Description = Console.ReadLine();
                    }

                    // delete content from module
                    Console.WriteLine("Would you like to delete content from this module?");
                    selection = Console.ReadLine();
                    if (selection?.Equals("Y", StringComparison.InvariantCultureIgnoreCase) ?? false) {
                        var keepRemoving = true;
                        while (keepRemoving) {
                            selectedModule.Content.ForEach(Console.WriteLine);
                            selection = Console.ReadLine();

                            var contentToRemove = selectedModule
                                .Content
                                .FirstOrDefault(c => c.ID.ToString().Equals(selection, StringComparison.InvariantCultureIgnoreCase));
                            if (contentToRemove != null) {
                                selectedModule.Content.Remove(contentToRemove);
                            }

                            Console.WriteLine("Would you like to remove more content?");
                            selection = Console.ReadLine();
                            if (selection?.Equals("N", StringComparison.InvariantCultureIgnoreCase) ?? false) {
                                keepRemoving = false;
                            }
                        }

                    }

                    // adds content to the module
                    Console.WriteLine("Would you like to add content?");
                    var choice = Console.ReadLine() ?? "N";
                    while (choice.Equals("Y", StringComparison.InvariantCultureIgnoreCase)) {
                        Console.WriteLine("What type of content would you like to add?");
                        Console.WriteLine("1. Assignment");
                        Console.WriteLine("2. File");
                        Console.WriteLine("3. Page");
                        var contentChoice = int.Parse(Console.ReadLine() ?? "0");

                        switch (contentChoice) {
                            case 1:
                                var newAssignmentContent = CreateAssignmentItem(selectedCourse);
                                if (newAssignmentContent != null) {
                                    selectedModule.Content.Add(newAssignmentContent);
                                }
                                break;
                            case 2:
                                var newFileContent = CreateFileItem(selectedCourse);
                                if (newFileContent != null) {
                                    selectedModule.Content.Add(newFileContent);
                                }
                                break;
                            case 3:
                                var newPageContent = CreatePageItem(selectedCourse);
                                if (newPageContent != null) {
                                    selectedModule.Content.Add(newPageContent);
                                }
                                break;
                            default:
                                break;
                        }

                        Console.WriteLine("Would you like to add more content?");
                        choice = Console.ReadLine() ?? "N";
                    }

                }

            }

        }

        // updates an assignment for the selected course
        public void UpdateAssignment() {
            Console.WriteLine("Enter the course code:");
            courseService.Courses.ForEach(Console.WriteLine);
            var selection = Console.ReadLine();

            var selectedCourse = courseService.Courses.FirstOrDefault(s => s.Code.Equals(selection, StringComparison.InvariantCultureIgnoreCase));
            if (selectedCourse != null) {
                Console.WriteLine("Choose an assignment to update:");
                selectedCourse.Assignments.ToList().ForEach(Console.WriteLine);
                var selectionStr = Console.ReadLine() ?? string.Empty;
                var selectionInt = int.Parse(selectionStr);
                var selectedGroup = selectedCourse.AssignmentGroups.FirstOrDefault
                    (ag => ag.Assignments.Any(a => a.ID == selectionInt));
                
                if (selectedGroup != null) {
                    var selectedAssignment = selectedGroup.Assignments.FirstOrDefault(a => a.ID == selectionInt);
                    if (selectedAssignment != null) {
                        var index = selectedGroup.Assignments.IndexOf(selectedAssignment);
                        selectedGroup.Assignments.RemoveAt(index);
                        selectedGroup.Assignments.Insert(index, CreateAssignment());
                    }
                }

            }
        }

        // removes an assignment from the selected course
        public void RemoveAssignment() {
            Console.WriteLine("Enter the course code:");
            courseService.Courses.ForEach(Console.WriteLine);
            var selection = Console.ReadLine();

            var selectedCourse = courseService.Courses.FirstOrDefault(s => s.Code.Equals(selection, StringComparison.InvariantCultureIgnoreCase));
            if (selectedCourse != null) {
                Console.WriteLine("Choose an assignment to delete:");
                selectedCourse.Assignments.ToList().ForEach(Console.WriteLine);
                var selectionStr = Console.ReadLine() ?? string.Empty;
                var selectionInt = int.Parse(selectionStr);

                var selectedGroup = selectedCourse.AssignmentGroups.FirstOrDefault
                    (ag => ag.Assignments.Any(a => a.ID == selectionInt));

                if (selectedGroup != null) {
                    var selectedAssignment = selectedGroup.Assignments.FirstOrDefault(a => a.ID == selectionInt);
                    if (selectedAssignment != null) {
                        var index = selectedGroup.Assignments.Remove(selectedAssignment);
                    }
                }
            }
        }

        // sets up the roster for the selected course
        private void SetupRoster(Course c) {
            Console.WriteLine("What students would you like to enroll in this course? ('Q' to quit)");
            bool continueAdding = true;
            while (continueAdding) {
                studentService.Students.Where(s => !c.Roster.Any
                (s2 => s2.ID == s.ID)).ToList().ForEach(Console.WriteLine);

                var selection = "Q";

                if (studentService.Students.Any(s => !c.Roster.Any(s2 => s2.ID == s.ID))) {
                    selection = Console.ReadLine() ?? string.Empty;
                }

                if (selection.Equals("Q", StringComparison.InvariantCultureIgnoreCase)) {
                    continueAdding = false;
                } else {
                    var selectedId = int.Parse(selection);
                    var selectedStudent = studentService.Students.FirstOrDefault(s => s.ID == selectedId);

                    if (selectedStudent != null) {
                        c.Roster.Add(selectedStudent);
                    }
                }
            }
        }

        // sets up the assignments for the selected course
        private void SetupAssignments(Course c) {
            Console.WriteLine("Would you like to add assignments? (Y/N)");
            var assignResponse = Console.ReadLine() ?? "N";

            bool continueAdding;
            if (assignResponse.Equals("Y", StringComparison.InvariantCultureIgnoreCase)) {
                continueAdding = true;
                while (continueAdding) {
                    CreateAssignmentWithGroup(c);
                    Console.WriteLine("Add more assignments? (Y/N)");
                    assignResponse = Console.ReadLine() ?? "N";

                    if (assignResponse.Equals("N", StringComparison.InvariantCultureIgnoreCase)) {
                        continueAdding = false;
                    }
                }
            }
        }

        // sets up modules for the selected course
        private void SetupModules(Course c) {
            Console.WriteLine("Would you like to add modules? (Y/N)");
            var assignResponse = Console.ReadLine() ?? "N";

            bool continueAdding;
            if (assignResponse.Equals("Y", StringComparison.InvariantCultureIgnoreCase)) {
                continueAdding = true;
                while (continueAdding) {
                    c.Modules.Add(CreateModule(c));
                    Console.WriteLine("Add more modules? (Y/N)");
                    assignResponse = Console.ReadLine() ?? "N";

                    if (assignResponse.Equals("N", StringComparison.InvariantCultureIgnoreCase)) {
                        continueAdding = false;
                    }
                }
            }
        }

        // creates an announcement
        private Announcement CreateAnnouncement(Course c) {
            Console.WriteLine("Enter the name of the announcement:");
            var name = Console.ReadLine();

            Console.WriteLine("Enter the description of the announcement:");
            var description = Console.ReadLine();

            return new Announcement {
                Name = name,
                Description = description
            };
        }

        // updates the selected announcement from the selected course
        public void UpdateAnnouncement() {
            Console.WriteLine("Enter the course code:");
            courseService.Courses.ForEach(Console.WriteLine);
            var selection = Console.ReadLine();

            var selectedCourse = courseService.Courses.FirstOrDefault
                (s => s.Code.Equals(selection, StringComparison.InvariantCultureIgnoreCase));

            if (selectedCourse != null) {
                Console.WriteLine("Which announcement would you like to update?");
                selectedCourse.Announcements.ForEach(Console.WriteLine);

                var selectionStr = Console.ReadLine() ?? string.Empty;
                var selectionInt = int.Parse(selectionStr);
                var selectedAnnouncement = selectedCourse.Announcements.FirstOrDefault
                    (a => a.ID == selectionInt);

                if (selectedAnnouncement != null) {
                    Console.WriteLine("Name:");
                    selectedAnnouncement.Name = Console.ReadLine();

                    Console.WriteLine("Description:");
                    selectedAnnouncement.Description = Console.ReadLine();
                }
            }
        }

        // removes the selected announcement from the selected course
        public void RemoveAnnouncement() {
            Console.WriteLine("Enter the course code:");
            courseService.Courses.ForEach(Console.WriteLine);
            var selection = Console.ReadLine();

            var selectedCourse = courseService.Courses.FirstOrDefault
                (s => s.Code.Equals(selection, StringComparison.InvariantCultureIgnoreCase));

            if (selectedCourse != null) {
                Console.WriteLine("Choose an announcement to delete:");
                selectedCourse.Announcements.ForEach(Console.WriteLine);

                var selectionStr = Console.ReadLine() ?? string.Empty;
                var selectionInt = int.Parse(selectionStr);
                var selectedAnnouncement = selectedCourse.Announcements.FirstOrDefault
                    (a => a.ID == selectionInt);

                if (selectedAnnouncement != null) {
                    selectedCourse.Announcements.Remove(selectedAnnouncement);
                }
            }
        }

        // creates a module
        private Module CreateModule(Course c) {
            // name for the module
            Console.WriteLine("Name:");
            var name = Console.ReadLine() ?? string.Empty;

            // description for the module
            Console.WriteLine("Description:");
            var description = Console.ReadLine() ?? string.Empty;

            var module = new Module {
                Name = name,
                Description = description
            };

            Console.WriteLine("Would you like to add content?");
            var choice = Console.ReadLine() ?? "N";
            while (choice.Equals("Y", StringComparison.InvariantCultureIgnoreCase)) {
                Console.WriteLine("What type of content would you like to add?");
                Console.WriteLine("1. Assignment");
                Console.WriteLine("2. File");
                Console.WriteLine("3. Page");
                var contentChoice = int.Parse(Console.ReadLine() ?? "0");

                switch (contentChoice) {
                    case 1:
                        var newAssignmentContent = CreateAssignmentItem(c);
                        if (newAssignmentContent != null) {
                            module.Content.Add(newAssignmentContent);
                        }
                        break;
                    case 2:
                        var newFileContent = CreateFileItem(c);
                        if (newFileContent != null) {
                            module.Content.Add(newFileContent);
                        }
                        break;
                    case 3:
                        var newPageContent = CreatePageItem(c);
                        if (newPageContent != null) {
                            module.Content.Add(newPageContent);
                        }
                        break;
                    default:
                        break;
                }

                Console.WriteLine("Would you like to add more content?");
                choice = Console.ReadLine() ?? "N";
            }

            return module;
        }

        // creates an assignment item
        private AssignmentItem? CreateAssignmentItem(Course c) {
            // name for the assignment
            Console.WriteLine("Name:");
            var name = Console.ReadLine() ?? string.Empty;

            // description for the assignment
            Console.WriteLine("Description:");
            var description = Console.ReadLine() ?? string.Empty;

            Console.WriteLine("Which assignment should be added?");
            c.Assignments.ToList().ForEach(Console.WriteLine);
            var choice = int.Parse(Console.ReadLine() ?? "-1");

            if (choice >= 0) {
                var assignment = c.Assignments.FirstOrDefault
                    (a => a.ID == choice);

                return new AssignmentItem {
                    Assignment = assignment,
                    Name = name,
                    Description = description
                };
            }
            return null;
        }

        // creates a file item
        private FileItem? CreateFileItem(Course c) {
            // name for the file
            Console.WriteLine("Name:");
            var name = Console.ReadLine() ?? string.Empty;
            // description for the file
            Console.WriteLine("Description:");
            var description = Console.ReadLine() ?? string.Empty;

            Console.WriteLine("Enter a path to the file:");
            var filepath = Console.ReadLine();

            return new FileItem {
                Name = name,
                Description = description,
                Path = filepath
            };
        }

        // creates a page item
        private PageItem? CreatePageItem(Course c) {
            // name for the page item
            Console.WriteLine("Name:");
            var name = Console.ReadLine() ?? string.Empty;

            // description for the page item
            Console.WriteLine("Description:");
            var description = Console.ReadLine() ?? string.Empty;

            Console.WriteLine("Enter page content:");
            var body = Console.ReadLine();

            return new PageItem {
                Name = name,
                Description = description,
                htmlBody = body
            };
        }

        // creates an assignment
        private Assignment CreateAssignment() {
            // name of the assignment
            Console.WriteLine("Name:");
            var assignmentName = Console.ReadLine() ?? string.Empty;

            // description of the assignment
            Console.WriteLine("Description:");
            var assignmentDescription = Console.ReadLine() ?? string.Empty;

            // total points for the assigment
            Console.WriteLine("Total Points:");
            var totalPoints = decimal.Parse(Console.ReadLine() ?? "100");

            // due date for the assignment
            Console.WriteLine("Due Date:");
            var dueDate = DateTime.Parse(Console.ReadLine() ?? "01/01/1900");

            return new Assignment {
                Name = assignmentName,
                Description = assignmentDescription,
                TotalAvailablePoints = totalPoints,
                DueDate = dueDate
            };
        }

        // creates a submission
        public void CreateSubmission(Course c, Student? student, Assignment? assignment) {
            if (student == null || assignment == null) {
                return;
            }

            Console.WriteLine("What is the content of the submission?");
            var content = Console.ReadLine();
            c.Submissions.Add(
                new Submission {
                    Student = student,
                    Assignment = assignment,
                    Content = content ?? string.Empty
                }
            );
        }

        // lists the submissions of the selected course
        public void ListSubmissions() {
            Console.WriteLine("Enter the course code that you would like to add the assignment to:");
            courseService.Courses.ForEach(Console.WriteLine);
            var selection = Console.ReadLine();

            var selectedCourse = courseService.Courses.FirstOrDefault
                (s => s.Code.Equals(selection, StringComparison.InvariantCultureIgnoreCase));

            if (selectedCourse != null) {
                selectedCourse.Submissions.ForEach(Console.WriteLine);
            }
        }

        // removes a submission from the selected course
        public void RemoveSubmission() {
            Console.WriteLine("Enter the course code that you would like to add the assignment to:");
            courseService.Courses.ForEach(Console.WriteLine);
            var selection = Console.ReadLine();

            var selectedCourse = courseService.Courses.FirstOrDefault
                (s => s.Code.Equals(selection, StringComparison.InvariantCultureIgnoreCase));

            if (selectedCourse != null) {
                selectedCourse.Submissions.ForEach(Console.WriteLine);
                var selectedId = int.Parse(Console.ReadLine() ?? "0");

                var selectedSubmission = selectedCourse.Submissions.FirstOrDefault(s => s.ID == selectedId);
                if (selectedSubmission != null) {
                    selectedCourse.Submissions.Remove(selectedSubmission);
                }
            }
        }

        // updates a submission from the selected course
        public void UpdateSubmission() {
            Console.WriteLine("Enter the course code that you would like to add the assignment to:");
            courseService.Courses.ForEach(Console.WriteLine);
            var selection = Console.ReadLine();

            var selectedCourse = courseService.Courses.FirstOrDefault
                (s => s.Code.Equals(selection, StringComparison.InvariantCultureIgnoreCase));

            if (selectedCourse != null) {
                selectedCourse.Submissions.ForEach(Console.WriteLine);
                var selectedID = int.Parse(Console.ReadLine() ?? "0");

                Console.WriteLine("Enter new content:");

                selectedCourse.Submissions.FirstOrDefault
                (s => s.ID == selectedID).Content = Console.ReadLine() ?? string.Empty;
            }
        }

        // grades a submission from the selected course
        public void GradeSubmission() {
            Console.WriteLine("Enter the course code that you would like to add the assignment to:");
            courseService.Courses.ForEach(Console.WriteLine);
            var selection = Console.ReadLine();

            var selectedCourse = courseService.Courses.FirstOrDefault
                (s => s.Code.Equals(selection, StringComparison.InvariantCultureIgnoreCase));

            if (selectedCourse != null) {
                selectedCourse.Submissions.ForEach(Console.WriteLine);
                var selectedId = int.Parse(Console.ReadLine() ?? "0");

                Console.WriteLine("Enter grade:");
                selectedCourse.Submissions.FirstOrDefault
                    (s => s.ID == selectedId).Grade = decimal.Parse(Console.ReadLine() ?? "0");
            }
        }

        // gets the student grade from the selected course.
        public void GetStudentGrade() {
            Console.WriteLine("Enter the course code that you would like to add the assignment to:");
            courseService.Courses.ForEach(Console.WriteLine);
            var selection = Console.ReadLine();

            var selectedCourse = courseService.Courses.FirstOrDefault
                (s => s.Code.Equals(selection, StringComparison.InvariantCultureIgnoreCase));

            if (selectedCourse != null) {
                Console.WriteLine("Enter the id for the student:");
                selectedCourse.Roster.Where
                    (r => r is Student).ToList().ForEach(Console.WriteLine);

                var selectedStudentId = int.Parse(Console.ReadLine() ?? "0");
                var weightedAverage = courseService.GetWeightedGrade(selectedCourse.ID, selectedStudentId);

                Console.WriteLine($"Student Grade: ({courseService.GetLetterGrade(weightedAverage)}) {weightedAverage}");

            }

        }
    }
}
