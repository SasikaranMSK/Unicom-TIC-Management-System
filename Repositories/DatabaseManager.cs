using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Threading.Tasks;
using UnicornTICManagementSystem.Models;

namespace UnicornTICManagementSystem.Repositories
{
    public class DatabaseManager
    {
        private readonly string _connectionString;

        public DatabaseManager()
        {
            _connectionString = "Data Source=UnicornTIC.db;Version=3;";
            InitializeDatabase();
        }

        public void InitializeDatabase()
        {
            using (var connection = new SQLiteConnection(_connectionString))
            {
                connection.Open();

                // Create Users table
                var createUsersTable = @"
                CREATE TABLE IF NOT EXISTS Users (
                    Id INTEGER PRIMARY KEY AUTOINCREMENT,
                    Username TEXT NOT NULL UNIQUE,
                    Password TEXT NOT NULL,
                    Email TEXT NOT NULL,
                    FirstName TEXT NOT NULL,
                    LastName TEXT NOT NULL,
                    Role INTEGER NOT NULL,
                    CreatedDate DATETIME NOT NULL,
                    IsActive BOOLEAN NOT NULL DEFAULT 1
                )";

                // Create Students table
                var createStudentsTable = @"
                CREATE TABLE IF NOT EXISTS Students (
                    Id INTEGER PRIMARY KEY AUTOINCREMENT,
                    StudentNumber TEXT NOT NULL UNIQUE,
                    FirstName TEXT NOT NULL,
                    LastName TEXT NOT NULL,
                    Email TEXT NOT NULL,
                    DateOfBirth DATETIME NOT NULL,
                    Address TEXT,
                    PhoneNumber TEXT,
                    EnrollmentDate DATETIME NOT NULL,
                    IsActive BOOLEAN NOT NULL DEFAULT 1
                )";

                // Create Courses table
                var createCoursesTable = @"
                CREATE TABLE IF NOT EXISTS Courses (
                    Id INTEGER PRIMARY KEY AUTOINCREMENT,
                    CourseCode TEXT NOT NULL UNIQUE,
                    CourseName TEXT NOT NULL,
                    Description TEXT,
                    Credits INTEGER NOT NULL,
                    LectureId INTEGER NOT NULL,
                    LectureName TEXT NOT NULL,
                    StartDate DATETIME NOT NULL,
                    EndDate DATETIME NOT NULL,
                    MaxStudents INTEGER NOT NULL,
                    IsActive BOOLEAN NOT NULL DEFAULT 1
                )";

                // Create Exams table
                var createExamsTable = @"
                CREATE TABLE IF NOT EXISTS Exams (
                    Id INTEGER PRIMARY KEY AUTOINCREMENT,
                    ExamName TEXT NOT NULL,
                    CourseId INTEGER NOT NULL,
                    CourseName TEXT NOT NULL,
                    ExamDate DATETIME NOT NULL,
                    Duration TEXT NOT NULL,
                    Location TEXT,
                    MaxMarks INTEGER NOT NULL,
                    Instructions TEXT,
                    Type INTEGER NOT NULL,
                    IsActive BOOLEAN NOT NULL DEFAULT 1,
                    FOREIGN KEY (CourseId) REFERENCES Courses(Id)
                )";

                // Create Marks table
                var createMarksTable = @"
                CREATE TABLE IF NOT EXISTS Marks (
                    Id INTEGER PRIMARY KEY AUTOINCREMENT,
                    StudentId INTEGER NOT NULL,
                    StudentName TEXT NOT NULL,
                    ExamId INTEGER NOT NULL,
                    ExamName TEXT NOT NULL,
                    CourseId INTEGER NOT NULL,
                    CourseName TEXT NOT NULL,
                    MarksObtained DECIMAL NOT NULL,
                    MaxMarks DECIMAL NOT NULL,
                    Grade TEXT NOT NULL,
                    DateRecorded DATETIME NOT NULL,
                    Comments TEXT,
                    FOREIGN KEY (StudentId) REFERENCES Students(Id),
                    FOREIGN KEY (ExamId) REFERENCES Exams(Id),
                    FOREIGN KEY (CourseId) REFERENCES Courses(Id)
                )";

                // Create Timetables table
                var createTimetablesTable = @"
                CREATE TABLE IF NOT EXISTS Timetables (
                    Id INTEGER PRIMARY KEY AUTOINCREMENT,
                    CourseId INTEGER NOT NULL,
                    CourseName TEXT NOT NULL,
                    DayOfWeek INTEGER NOT NULL,
                    StartTime TEXT NOT NULL,
                    EndTime TEXT NOT NULL,
                    Classroom TEXT NOT NULL,
                    LectureName TEXT NOT NULL,
                    EffectiveDate DATETIME NOT NULL,
                    EndDate DATETIME,
                    IsActive BOOLEAN NOT NULL DEFAULT 1,
                    FOREIGN KEY (CourseId) REFERENCES Courses(Id)
                )";

                // Create Student-Course enrollment table
                var createEnrollmentsTable = @"
                CREATE TABLE IF NOT EXISTS StudentCourseEnrollments (
                    Id INTEGER PRIMARY KEY AUTOINCREMENT,
                    StudentId INTEGER NOT NULL,
                    CourseId INTEGER NOT NULL,
                    EnrollmentDate DATETIME NOT NULL,
                    IsActive BOOLEAN NOT NULL DEFAULT 1,
                    FOREIGN KEY (StudentId) REFERENCES Students(Id),
                    FOREIGN KEY (CourseId) REFERENCES Courses(Id),
                    UNIQUE(StudentId, CourseId)
                )";

                using (var command = new SQLiteCommand(connection))
                {
                    // Execute table creation commands
                    command.CommandText = createUsersTable;
                    command.ExecuteNonQuery();

                    command.CommandText = createStudentsTable;
                    command.ExecuteNonQuery();

                    command.CommandText = createCoursesTable;
                    command.ExecuteNonQuery();

                    command.CommandText = createExamsTable;
                    command.ExecuteNonQuery();

                    command.CommandText = createMarksTable;
                    command.ExecuteNonQuery();

                    command.CommandText = createTimetablesTable;
                    command.ExecuteNonQuery();

                    command.CommandText = createEnrollmentsTable;
                    command.ExecuteNonQuery();
                }

                // Insert sample data if tables are empty
                InsertSampleData(connection);
            }
        }

        private void InsertSampleData(SQLiteConnection connection)
        {
            // Check if users exist
            var checkUsersQuery = "SELECT COUNT(*) FROM Users";
            using (var checkCommand = new SQLiteCommand(checkUsersQuery, connection))
            {
                var userCount = Convert.ToInt32(checkCommand.ExecuteScalar());

                if (userCount == 0)
                {
                    // Insert sample users
                    var insertUsers = @"
                    INSERT INTO Users (Username, Password, Email, FirstName, LastName, Role, CreatedDate, IsActive)
                    VALUES 
                    ('admin', 'admin123', 'admin@unicorn.edu', 'System', 'Administrator', 2, datetime('now'), 1),
                    ('lecture1', 'lecture123', 'lecture1@unicorn.edu', 'John', 'Smith', 1, datetime('now'), 1),
                    ('student1', 'student123', 'student1@unicorn.edu', 'Alice', 'Johnson', 0, datetime('now'), 1)";

                    using (var command = new SQLiteCommand(insertUsers, connection))
                    {
                        command.ExecuteNonQuery();
                    }

                    // Insert sample students
                    var insertStudents = @"
                    INSERT INTO Students (StudentNumber, FirstName, LastName, Email, DateOfBirth, Address, PhoneNumber, EnrollmentDate, IsActive)
                    VALUES 
                    ('STU001', 'Alice', 'Johnson', 'alice.johnson@student.unicorn.edu', '2000-05-15', '123 Main St, City', '555-0101', datetime('now', '-6 months'), 1),
                    ('STU002', 'Bob', 'Wilson', 'bob.wilson@student.unicorn.edu', '1999-08-22', '456 Oak Ave, City', '555-0102', datetime('now', '-4 months'), 1),
                    ('STU003', 'Carol', 'Davis', 'carol.davis@student.unicorn.edu', '2001-03-10', '789 Pine St, City', '555-0103', datetime('now', '-2 months'), 1)";

                    using (var command = new SQLiteCommand(insertStudents, connection))
                    {
                        command.ExecuteNonQuery();
                    }

                    // Insert sample courses
                    var insertCourses = @"
                    INSERT INTO Courses (CourseCode, CourseName, Description, Credits, LectureId, LectureName, StartDate, EndDate, MaxStudents, IsActive)
                    VALUES 
                    ('CS101', 'Introduction to Computer Science', 'Basic concepts of computer science and programming', 3, 2, 'John Smith', datetime('now', '-1 month'), datetime('now', '+2 months'), 30, 1),
                    ('MATH201', 'Calculus I', 'Differential and integral calculus', 4, 2, 'John Smith', datetime('now', '-1 month'), datetime('now', '+2 months'), 25, 1),
                    ('ENG101', 'English Composition', 'Academic writing and communication skills', 3, 2, 'John Smith', datetime('now', '-1 month'), datetime('now', '+2 months'), 20, 1)";

                    using (var command = new SQLiteCommand(insertCourses, connection))
                    {
                        command.ExecuteNonQuery();
                    }

                    // Insert sample exams
                    var insertExams = @"
                    INSERT INTO Exams (ExamName, CourseId, CourseName, ExamDate, Duration, Location, MaxMarks, Instructions, Type, IsActive)
                    VALUES 
                    ('Midterm Exam', 1, 'Introduction to Computer Science', datetime('now', '+1 week'), '02:00:00', 'Room 101', 100, 'Bring calculator and ID', 0, 1),
                    ('Final Exam', 1, 'Introduction to Computer Science', datetime('now', '+1 month'), '03:00:00', 'Room 101', 150, 'Comprehensive exam covering all topics', 1, 1),
                    ('Quiz 1', 2, 'Calculus I', datetime('now', '+3 days'), '01:00:00', 'Room 202', 50, 'Covers chapters 1-3', 2, 1)";

                    using (var command = new SQLiteCommand(insertExams, connection))
                    {
                        command.ExecuteNonQuery();
                    }

                    // Insert sample marks
                    var insertMarks = @"
                    INSERT INTO Marks (StudentId, StudentName, ExamId, ExamName, CourseId, CourseName, MarksObtained, MaxMarks, Grade, DateRecorded, Comments)
                    VALUES 
                    (1, 'Alice Johnson', 1, 'Midterm Exam', 1, 'Introduction to Computer Science', 85, 100, 'A', datetime('now', '-1 week'), 'Excellent work'),
                    (2, 'Bob Wilson', 1, 'Midterm Exam', 1, 'Introduction to Computer Science', 78, 100, 'B+', datetime('now', '-1 week'), 'Good understanding'),
                    (1, 'Alice Johnson', 3, 'Quiz 1', 2, 'Calculus I', 45, 50, 'A-', datetime('now', '-2 days'), 'Well done')";

                    using (var command = new SQLiteCommand(insertMarks, connection))
                    {
                        command.ExecuteNonQuery();
                    }

                    // Insert sample timetables
                    var insertTimetables = @"
                    INSERT INTO Timetables (CourseId, CourseName, DayOfWeek, StartTime, EndTime, Classroom, LectureName, EffectiveDate, IsActive)
                    VALUES 
                    (1, 'Introduction to Computer Science', 1, '09:00', '10:30', 'Lab 101', 'John Smith', datetime('now', '-1 month'), 1),
                    (1, 'Introduction to Computer Science', 3, '09:00', '10:30', 'Lab 101', 'John Smith', datetime('now', '-1 month'), 1),
                    (2, 'Calculus I', 2, '11:00', '12:30', 'Room 201', 'John Smith', datetime('now', '-1 month'), 1),
                    (2, 'Calculus I', 4, '11:00', '12:30', 'Room 201', 'John Smith', datetime('now', '-1 month'), 1)";

                    using (var command = new SQLiteCommand(insertTimetables, connection))
                    {
                        command.ExecuteNonQuery();
                    }

                    // Insert sample enrollments
                    var insertEnrollments = @"
                    INSERT INTO StudentCourseEnrollments (StudentId, CourseId, EnrollmentDate, IsActive)
                    VALUES 
                    (1, 1, datetime('now', '-1 month'), 1),
                    (1, 2, datetime('now', '-1 month'), 1),
                    (2, 1, datetime('now', '-3 weeks'), 1),
                    (3, 1, datetime('now', '-2 weeks'), 1),
                    (3, 3, datetime('now', '-2 weeks'), 1)";

                    using (var command = new SQLiteCommand(insertEnrollments, connection))
                    {
                        command.ExecuteNonQuery();
                    }
                }
            }
        }

        // User methods
        public async Task<User> GetUserByCredentialsAsync(string username, string password)
        {
            return await Task.Run(() =>
            {
                using (var connection = new SQLiteConnection(_connectionString))
                {
                    connection.Open();

                    var query = "SELECT * FROM Users WHERE Username = @username AND Password = @password AND IsActive = 1";
                    using (var command = new SQLiteCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@username", username);
                        command.Parameters.AddWithValue("@password", password);

                        using (var reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                return new User
                                {
                                    Id = Convert.ToInt32(reader["Id"]),
                                    Username = reader["Username"].ToString(),
                                    Password = reader["Password"].ToString(),
                                    Email = reader["Email"].ToString(),
                                    FirstName = reader["FirstName"].ToString(),
                                    LastName = reader["LastName"].ToString(),
                                    Role = (UserRole)Convert.ToInt32(reader["Role"]),
                                    CreatedDate = Convert.ToDateTime(reader["CreatedDate"]),
                                    IsActive = Convert.ToBoolean(reader["IsActive"])
                                };
                            }
                        }
                    }
                }
                return null;
            });
        }

        public async Task<string> GetNextUsernameAsync(UserRole role)
        {
            return await Task.Run(() =>
            {
                using (var connection = new SQLiteConnection(_connectionString))
                {
                    connection.Open();
                    string prefix = role == UserRole.Student ? "STU" :
                                    role == UserRole.Lecture ? "LEC" : "ADM";
                    var query = $"SELECT Username FROM Users WHERE Username LIKE '{prefix}%' ORDER BY Id DESC LIMIT 1";
                    using (var command = new SQLiteCommand(query, connection))
                    {
                        var result = command.ExecuteScalar() as string;
                        if (!string.IsNullOrEmpty(result) && result.StartsWith(prefix))
                        {
                            if (int.TryParse(result.Substring(3), out int lastNum))
                                return $"{prefix}{(lastNum + 1):D3}";
                        }
                        return $"{prefix}001";
                    }
                }
            });
        }

        public async Task<bool> AddUserAsync(User user)
        {
            return await Task.Run(() =>
            {
                using (var connection = new SQLiteConnection(_connectionString))
                {
                    connection.Open();

                    var query = @"
                    INSERT INTO Users (Username, Password, Email, FirstName, LastName, Role, CreatedDate, IsActive)
                    VALUES (@username, @password, @email, @firstName, @lastName, @role, @createdDate, @isActive)";

                    using (var command = new SQLiteCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@username", user.Username);
                        command.Parameters.AddWithValue("@password", user.Password);
                        command.Parameters.AddWithValue("@email", user.Email);
                        command.Parameters.AddWithValue("@firstName", user.FirstName);
                        command.Parameters.AddWithValue("@lastName", user.LastName);
                        command.Parameters.AddWithValue("@role", (int)user.Role);
                        command.Parameters.AddWithValue("@createdDate", user.CreatedDate);
                        command.Parameters.AddWithValue("@isActive", user.IsActive);

                        return command.ExecuteNonQuery() > 0;
                    }
                }
            });
        }

        public async Task<List<User>> GetAllUsersAsync()
        {
            return await Task.Run(() =>
            {
                var users = new List<User>();
                using (var connection = new SQLiteConnection(_connectionString))
                {
                    connection.Open();
                    var query = "SELECT * FROM Users WHERE IsActive = 1";
                    using (var command = new SQLiteCommand(query, connection))
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            users.Add(new User
                            {
                                Id = Convert.ToInt32(reader["Id"]),
                                Username = reader["Username"].ToString(),
                                Password = reader["Password"].ToString(),
                                Email = reader["Email"].ToString(),
                                FirstName = reader["FirstName"].ToString(),
                                LastName = reader["LastName"].ToString(),
                                Role = (UserRole)Convert.ToInt32(reader["Role"]),
                                CreatedDate = Convert.ToDateTime(reader["CreatedDate"]),
                                IsActive = Convert.ToBoolean(reader["IsActive"])
                            });
                        }
                    }
                }
                return users;
            });
        }

        public async Task<bool> UpdateUserAsync(User user)
        {
            return await Task.Run(() =>
            {
                using (var connection = new SQLiteConnection(_connectionString))
                {
                    connection.Open();

                    var query = @"
                    UPDATE Users SET Password=@password, Email=@email, FirstName=@firstName, LastName=@lastName, Role=@role, IsActive=@isActive
                    WHERE Id=@id";

                    using (var command = new SQLiteCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@id", user.Id);
                        command.Parameters.AddWithValue("@password", user.Password);
                        command.Parameters.AddWithValue("@email", user.Email);
                        command.Parameters.AddWithValue("@firstName", user.FirstName);
                        command.Parameters.AddWithValue("@lastName", user.LastName);
                        command.Parameters.AddWithValue("@role", (int)user.Role);
                        command.Parameters.AddWithValue("@isActive", user.IsActive);

                        return command.ExecuteNonQuery() > 0;
                    }
                }
            });
        }

        public async Task<bool> DeleteUserAsync(int id)
        {
            return await Task.Run(() =>
            {
                using (var connection = new SQLiteConnection(_connectionString))
                {
                    connection.Open();

                    var query = "UPDATE Users SET IsActive = 0 WHERE Id = @id";
                    using (var command = new SQLiteCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@id", id);
                        return command.ExecuteNonQuery() > 0;
                    }
                }
            });
        }

        // Student methods
        public async Task<List<Student>> GetAllStudentsAsync()
        {
            return await Task.Run(() =>
            {
                var students = new List<Student>();
                using (var connection = new SQLiteConnection(_connectionString))
                {
                    connection.Open();

                    var query = "SELECT * FROM Students WHERE IsActive = 1 ORDER BY LastName, FirstName";
                    using (var command = new SQLiteCommand(query, connection))
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var student = new Student
                            {
                                Id = Convert.ToInt32(reader["Id"]),
                                StudentNumber = reader["StudentNumber"].ToString(),
                                FirstName = reader["FirstName"].ToString(),
                                LastName = reader["LastName"].ToString(),
                                Email = reader["Email"].ToString(),
                                DateOfBirth = Convert.ToDateTime(reader["DateOfBirth"]),
                                Address = reader["Address"]?.ToString() ?? "",
                                PhoneNumber = reader["PhoneNumber"]?.ToString() ?? "",
                                EnrollmentDate = Convert.ToDateTime(reader["EnrollmentDate"]),
                                IsActive = Convert.ToBoolean(reader["IsActive"])
                            };

                            // Load enrolled courses
                            student.EnrolledCourses = GetStudentCourses(student.Id);
                            students.Add(student);
                        }
                    }
                }
                return students;
            });
        }

        public async Task<Student> GetStudentByIdAsync(int id)
        {
            return await Task.Run(() =>
            {
                using (var connection = new SQLiteConnection(_connectionString))
                {
                    connection.Open();

                    var query = "SELECT * FROM Students WHERE Id = @id AND IsActive = 1";
                    using (var command = new SQLiteCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@id", id);

                        using (var reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                var student = new Student
                                {
                                    Id = Convert.ToInt32(reader["Id"]),
                                    StudentNumber = reader["StudentNumber"].ToString(),
                                    FirstName = reader["FirstName"].ToString(),
                                    LastName = reader["LastName"].ToString(),
                                    Email = reader["Email"].ToString(),
                                    DateOfBirth = Convert.ToDateTime(reader["DateOfBirth"]),
                                    Address = reader["Address"]?.ToString() ?? "",
                                    PhoneNumber = reader["PhoneNumber"]?.ToString() ?? "",
                                    EnrollmentDate = Convert.ToDateTime(reader["EnrollmentDate"]),
                                    IsActive = Convert.ToBoolean(reader["IsActive"])
                                };

                                student.EnrolledCourses = GetStudentCourses(student.Id);
                                return student;
                            }
                        }
                    }
                }
                return null;
            });
        }

        public async Task<bool> AddStudentAsync(Student student)
        {
            return await Task.Run(() =>
            {
                using (var connection = new SQLiteConnection(_connectionString))
                {
                    connection.Open();

                    var query = @"
                    INSERT INTO Students (StudentNumber, FirstName, LastName, Email, DateOfBirth, Address, PhoneNumber, EnrollmentDate, IsActive)
                    VALUES (@studentNumber, @firstName, @lastName, @email, @dateOfBirth, @address, @phoneNumber, @enrollmentDate, @isActive)";

                    using (var command = new SQLiteCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@studentNumber", student.StudentNumber);
                        command.Parameters.AddWithValue("@firstName", student.FirstName);
                        command.Parameters.AddWithValue("@lastName", student.LastName);
                        command.Parameters.AddWithValue("@email", student.Email);
                        command.Parameters.AddWithValue("@dateOfBirth", student.DateOfBirth);
                        command.Parameters.AddWithValue("@address", student.Address ?? "");
                        command.Parameters.AddWithValue("@phoneNumber", student.PhoneNumber ?? "");
                        command.Parameters.AddWithValue("@enrollmentDate", student.EnrollmentDate);
                        command.Parameters.AddWithValue("@isActive", student.IsActive);

                        return command.ExecuteNonQuery() > 0;
                    }
                }
            });
        }

        public async Task<bool> UpdateStudentAsync(Student student)
        {
            return await Task.Run(() =>
            {
                using (var connection = new SQLiteConnection(_connectionString))
                {
                    connection.Open();

                    var query = @"
                    UPDATE Students 
                    SET StudentNumber = @studentNumber, FirstName = @firstName, LastName = @lastName, 
                        Email = @email, DateOfBirth = @dateOfBirth, Address = @address, 
                        PhoneNumber = @phoneNumber, IsActive = @isActive
                    WHERE Id = @id";

                    using (var command = new SQLiteCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@id", student.Id);
                        command.Parameters.AddWithValue("@studentNumber", student.StudentNumber);
                        command.Parameters.AddWithValue("@firstName", student.FirstName);
                        command.Parameters.AddWithValue("@lastName", student.LastName);
                        command.Parameters.AddWithValue("@email", student.Email);
                        command.Parameters.AddWithValue("@dateOfBirth", student.DateOfBirth);
                        command.Parameters.AddWithValue("@address", student.Address ?? "");
                        command.Parameters.AddWithValue("@phoneNumber", student.PhoneNumber ?? "");
                        command.Parameters.AddWithValue("@isActive", student.IsActive);

                        return command.ExecuteNonQuery() > 0;
                    }
                }
            });
        }

        public async Task<bool> DeleteStudentAsync(int id)
        {
            return await Task.Run(() =>
            {
                using (var connection = new SQLiteConnection(_connectionString))
                {
                    connection.Open();

                    var query = "UPDATE Students SET IsActive = 0 WHERE Id = @id";
                    using (var command = new SQLiteCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@id", id);
                        return command.ExecuteNonQuery() > 0;
                    }
                }
            });
        }

        public async Task<string> GetNextStudentNumberAsync()
        {
            return await Task.Run(() =>
            {
                using (var connection = new SQLiteConnection(_connectionString))
                {
                    connection.Open();
                    // Assumes StudentNumber format is 'STU###'
                    var query = "SELECT StudentNumber FROM Students ORDER BY Id DESC LIMIT 1";
                    using (var command = new SQLiteCommand(query, connection))
                    {
                        var result = command.ExecuteScalar() as string;
                        if (!string.IsNullOrEmpty(result) && result.StartsWith("STU"))
                        {
                            if (int.TryParse(result.Substring(3), out int lastNum))
                            {
                                return $"STU{(lastNum + 1):D3}";
                            }
                        }
                        // If no students or format is wrong, start from STU001
                        return "STU001";
                    }
                }
            });
        }

        private List<Course> GetStudentCourses(int studentId)
        {
            var courses = new List<Course>();
            using (var connection = new SQLiteConnection(_connectionString))
            {
                connection.Open();

                var query = @"
                SELECT c.* FROM Courses c
                INNER JOIN StudentCourseEnrollments sce ON c.Id = sce.CourseId
                WHERE sce.StudentId = @studentId AND sce.IsActive = 1 AND c.IsActive = 1";

                using (var command = new SQLiteCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@studentId", studentId);
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            courses.Add(new Course
                            {
                                Id = Convert.ToInt32(reader["Id"]),
                                CourseCode = reader["CourseCode"].ToString(),
                                CourseName = reader["CourseName"].ToString(),
                                Description = reader["Description"]?.ToString() ?? "",
                                Credits = Convert.ToInt32(reader["Credits"]),
                                TeacherId = Convert.ToInt32(reader["LectureId"]),
                                TeacherName = reader["LectureName"].ToString(),
                                StartDate = Convert.ToDateTime(reader["StartDate"]),
                                EndDate = Convert.ToDateTime(reader["EndDate"]),
                                MaxStudents = Convert.ToInt32(reader["MaxStudents"]),
                                IsActive = Convert.ToBoolean(reader["IsActive"])
                            });
                        }
                    }
                }
            }
            return courses;
        }

        // Course methods
        public async Task<List<Course>> GetAllCoursesAsync()
        {
            return await Task.Run(() =>
            {
                var courses = new List<Course>();
                using (var connection = new SQLiteConnection(_connectionString))
                {
                    connection.Open();

                    var query = "SELECT * FROM Courses WHERE IsActive = 1 ORDER BY CourseCode";
                    using (var command = new SQLiteCommand(query, connection))
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var course = new Course
                            {
                                Id = Convert.ToInt32(reader["Id"]),
                                CourseCode = reader["CourseCode"].ToString(),
                                CourseName = reader["CourseName"].ToString(),
                                Description = reader["Description"]?.ToString() ?? "",
                                Credits = Convert.ToInt32(reader["Credits"]),
                                TeacherId = Convert.ToInt32(reader["LectureId"]),
                                TeacherName = reader["LectureName"].ToString(),
                                StartDate = Convert.ToDateTime(reader["StartDate"]),
                                EndDate = Convert.ToDateTime(reader["EndDate"]),
                                MaxStudents = Convert.ToInt32(reader["MaxStudents"]),
                                IsActive = Convert.ToBoolean(reader["IsActive"])
                            };

                            // Load enrolled students
                            course.EnrolledStudents = GetCourseStudents(course.Id);
                            courses.Add(course);
                        }
                    }
                }
                return courses;
            });
        }

        public async Task<Course> GetCourseByIdAsync(int id)
        {
            return await Task.Run(() =>
            {
                using (var connection = new SQLiteConnection(_connectionString))
                {
                    connection.Open();

                    var query = "SELECT * FROM Courses WHERE Id = @id AND IsActive = 1";
                    using (var command = new SQLiteCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@id", id);

                        using (var reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                var course = new Course
                                {
                                    Id = Convert.ToInt32(reader["Id"]),
                                    CourseCode = reader["CourseCode"].ToString(),
                                    CourseName = reader["CourseName"].ToString(),
                                    Description = reader["Description"]?.ToString() ?? "",
                                    Credits = Convert.ToInt32(reader["Credits"]),
                                    TeacherId = Convert.ToInt32(reader["LectureId"]),
                                    TeacherName = reader["LectureName"].ToString(),
                                    StartDate = Convert.ToDateTime(reader["StartDate"]),
                                    EndDate = Convert.ToDateTime(reader["EndDate"]),
                                    MaxStudents = Convert.ToInt32(reader["MaxStudents"]),
                                    IsActive = Convert.ToBoolean(reader["IsActive"])
                                };

                                course.EnrolledStudents = GetCourseStudents(course.Id);
                                return course;
                            }
                        }
                    }
                }
                return null;
            });
        }

        public async Task<bool> AddCourseAsync(Course course)
        {
            return await Task.Run(() =>
            {
                using (var connection = new SQLiteConnection(_connectionString))
                {
                    connection.Open();

                    var query = @"
                    INSERT INTO Courses (CourseCode, CourseName, Description, Credits, LectureId, LectureName, StartDate, EndDate, MaxStudents, IsActive)
                    VALUES (@courseCode, @courseName, @description, @credits, @lectureId, @lectureName, @startDate, @endDate, @maxStudents, @isActive)";

                    using (var command = new SQLiteCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@courseCode", course.CourseCode);
                        command.Parameters.AddWithValue("@courseName", course.CourseName);
                        command.Parameters.AddWithValue("@description", course.Description ?? "");
                        command.Parameters.AddWithValue("@credits", course.Credits);
                        command.Parameters.AddWithValue("@lectureId", course.TeacherId);
                        command.Parameters.AddWithValue("@lectureName", course.TeacherName);
                        command.Parameters.AddWithValue("@startDate", course.StartDate);
                        command.Parameters.AddWithValue("@endDate", course.EndDate);
                        command.Parameters.AddWithValue("@maxStudents", course.MaxStudents);
                        command.Parameters.AddWithValue("@isActive", course.IsActive);

                        return command.ExecuteNonQuery() > 0;
                    }
                }
            });
        }

        public async Task<bool> UpdateCourseAsync(Course course)
        {
            return await Task.Run(() =>
            {
                using (var connection = new SQLiteConnection(_connectionString))
                {
                    connection.Open();

                    var query = @"
                    UPDATE Courses 
                    SET CourseCode = @courseCode, CourseName = @courseName, Description = @description,
                        Credits = @credits, LectureId = @lectureId, LectureName = @lectureName,
                        StartDate = @startDate, EndDate = @endDate, MaxStudents = @maxStudents, IsActive = @isActive
                    WHERE Id = @id";

                    using (var command = new SQLiteCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@id", course.Id);
                        command.Parameters.AddWithValue("@courseCode", course.CourseCode);
                        command.Parameters.AddWithValue("@courseName", course.CourseName);
                        command.Parameters.AddWithValue("@description", course.Description ?? "");
                        command.Parameters.AddWithValue("@credits", course.Credits);
                        command.Parameters.AddWithValue("@lectureId", course.TeacherId);
                        command.Parameters.AddWithValue("@lectureName", course.TeacherName);
                        command.Parameters.AddWithValue("@startDate", course.StartDate);
                        command.Parameters.AddWithValue("@endDate", course.EndDate);
                        command.Parameters.AddWithValue("@maxStudents", course.MaxStudents);
                        command.Parameters.AddWithValue("@isActive", course.IsActive);

                        return command.ExecuteNonQuery() > 0;
                    }
                }
            });
        }

        public async Task<bool> DeleteCourseAsync(int id)
        {
            return await Task.Run(() =>
            {
                using (var connection = new SQLiteConnection(_connectionString))
                {
                    connection.Open();

                    var query = "UPDATE Courses SET IsActive = 0 WHERE Id = @id";
                    using (var command = new SQLiteCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@id", id);
                        return command.ExecuteNonQuery() > 0;
                    }
                }
            });
        }

        private List<Student> GetCourseStudents(int courseId)
        {
            var students = new List<Student>();
            using (var connection = new SQLiteConnection(_connectionString))
            {
                connection.Open();

                var query = @"
                SELECT s.* FROM Students s
                INNER JOIN StudentCourseEnrollments sce ON s.Id = sce.StudentId
                WHERE sce.CourseId = @courseId AND sce.IsActive = 1 AND s.IsActive = 1";

                using (var command = new SQLiteCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@courseId", courseId);
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            students.Add(new Student
                            {
                                Id = Convert.ToInt32(reader["Id"]),
                                StudentNumber = reader["StudentNumber"].ToString(),
                                FirstName = reader["FirstName"].ToString(),
                                LastName = reader["LastName"].ToString(),
                                Email = reader["Email"].ToString(),
                                DateOfBirth = Convert.ToDateTime(reader["DateOfBirth"]),
                                Address = reader["Address"]?.ToString() ?? "",
                                PhoneNumber = reader["PhoneNumber"]?.ToString() ?? "",
                                EnrollmentDate = Convert.ToDateTime(reader["EnrollmentDate"]),
                                IsActive = Convert.ToBoolean(reader["IsActive"])
                            });
                        }
                    }
                }
            }
            return students;
        }

        // Exam methods
        public async Task<List<Exam>> GetAllExamsAsync()
        {
            return await Task.Run(() =>
            {
                var exams = new List<Exam>();
                using (var connection = new SQLiteConnection(_connectionString))
                {
                    connection.Open();

                    var query = "SELECT * FROM Exams WHERE IsActive = 1 ORDER BY ExamDate";
                    using (var command = new SQLiteCommand(query, connection))
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            exams.Add(new Exam
                            {
                                Id = Convert.ToInt32(reader["Id"]),
                                ExamName = reader["ExamName"].ToString(),
                                CourseId = Convert.ToInt32(reader["CourseId"]),
                                CourseName = reader["CourseName"].ToString(),
                                ExamDate = Convert.ToDateTime(reader["ExamDate"]),
                                Duration = TimeSpan.Parse(reader["Duration"].ToString()),
                                Location = reader["Location"]?.ToString() ?? "",
                                MaxMarks = Convert.ToInt32(reader["MaxMarks"]),
                                Instructions = reader["Instructions"]?.ToString() ?? "",
                                Type = (ExamType)Convert.ToInt32(reader["Type"]),
                                IsActive = Convert.ToBoolean(reader["IsActive"])
                            });
                        }
                    }
                }
                return exams;
            });
        }

        public async Task<List<Exam>> GetExamsByCourseAsync(int courseId)
        {
            return await Task.Run(() =>
            {
                var exams = new List<Exam>();
                using (var connection = new SQLiteConnection(_connectionString))
                {
                    connection.Open();

                    var query = "SELECT * FROM Exams WHERE CourseId = @courseId AND IsActive = 1 ORDER BY ExamDate";
                    using (var command = new SQLiteCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@courseId", courseId);
                        using (var reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                exams.Add(new Exam
                                {
                                    Id = Convert.ToInt32(reader["Id"]),
                                    ExamName = reader["ExamName"].ToString(),
                                    CourseId = Convert.ToInt32(reader["CourseId"]),
                                    CourseName = reader["CourseName"].ToString(),
                                    ExamDate = Convert.ToDateTime(reader["ExamDate"]),
                                    Duration = TimeSpan.Parse(reader["Duration"].ToString()),
                                    Location = reader["Location"]?.ToString() ?? "",
                                    MaxMarks = Convert.ToInt32(reader["MaxMarks"]),
                                    Instructions = reader["Instructions"]?.ToString() ?? "",
                                    Type = (ExamType)Convert.ToInt32(reader["Type"]),
                                    IsActive = Convert.ToBoolean(reader["IsActive"])
                                });
                            }
                        }
                    }
                }
                return exams;
            });
        }

        public async Task<bool> AddExamAsync(Exam exam)
        {
            return await Task.Run(() =>
            {
                using (var connection = new SQLiteConnection(_connectionString))
                {
                    connection.Open();

                    var query = @"
                    INSERT INTO Exams (ExamName, CourseId, CourseName, ExamDate, Duration, Location, MaxMarks, Instructions, Type, IsActive)
                    VALUES (@examName, @courseId, @courseName, @examDate, @duration, @location, @maxMarks, @instructions, @type, @isActive)";

                    using (var command = new SQLiteCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@examName", exam.ExamName);
                        command.Parameters.AddWithValue("@courseId", exam.CourseId);
                        command.Parameters.AddWithValue("@courseName", exam.CourseName);
                        command.Parameters.AddWithValue("@examDate", exam.ExamDate);
                        command.Parameters.AddWithValue("@duration", exam.Duration.ToString());
                        command.Parameters.AddWithValue("@location", exam.Location ?? "");
                        command.Parameters.AddWithValue("@maxMarks", exam.MaxMarks);
                        command.Parameters.AddWithValue("@instructions", exam.Instructions ?? "");
                        command.Parameters.AddWithValue("@type", (int)exam.Type);
                        command.Parameters.AddWithValue("@isActive", exam.IsActive);

                        return command.ExecuteNonQuery() > 0;
                    }
                }
            });
        }

        public async Task<bool> UpdateExamAsync(Exam exam)
        {
            return await Task.Run(() =>
            {
                using (var connection = new SQLiteConnection(_connectionString))
                {
                    connection.Open();

                    var query = @"
                    UPDATE Exams 
                    SET ExamName = @examName, CourseId = @courseId, CourseName = @courseName,
                        ExamDate = @examDate, Duration = @duration, Location = @location,
                        MaxMarks = @maxMarks, Instructions = @instructions, Type = @type, IsActive = @isActive
                    WHERE Id = @id";

                    using (var command = new SQLiteCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@id", exam.Id);
                        command.Parameters.AddWithValue("@examName", exam.ExamName);
                        command.Parameters.AddWithValue("@courseId", exam.CourseId);
                        command.Parameters.AddWithValue("@courseName", exam.CourseName);
                        command.Parameters.AddWithValue("@examDate", exam.ExamDate);
                        command.Parameters.AddWithValue("@duration", exam.Duration.ToString());
                        command.Parameters.AddWithValue("@location", exam.Location ?? "");
                        command.Parameters.AddWithValue("@maxMarks", exam.MaxMarks);
                        command.Parameters.AddWithValue("@instructions", exam.Instructions ?? "");
                        command.Parameters.AddWithValue("@type", (int)exam.Type);
                        command.Parameters.AddWithValue("@isActive", exam.IsActive);

                        return command.ExecuteNonQuery() > 0;
                    }
                }
            });
        }

        // Mark methods
        public async Task<List<Mark>> GetMarksByStudentAsync(int studentId)
        {
            return await Task.Run(() =>
            {
                var marks = new List<Mark>();
                using (var connection = new SQLiteConnection(_connectionString))
                {
                    connection.Open();

                    var query = "SELECT * FROM Marks WHERE StudentId = @studentId ORDER BY DateRecorded DESC";
                    using (var command = new SQLiteCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@studentId", studentId);
                        using (var reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                marks.Add(new Mark
                                {
                                    Id = Convert.ToInt32(reader["Id"]),
                                    StudentId = Convert.ToInt32(reader["StudentId"]),
                                    StudentName = reader["StudentName"].ToString(),
                                    ExamId = Convert.ToInt32(reader["ExamId"]),
                                    ExamName = reader["ExamName"].ToString(),
                                    CourseId = Convert.ToInt32(reader["CourseId"]),
                                    CourseName = reader["CourseName"].ToString(),
                                    MarksObtained = Convert.ToDecimal(reader["MarksObtained"]),
                                    MaxMarks = Convert.ToDecimal(reader["MaxMarks"]),
                                    Grade = reader["Grade"].ToString(),
                                    DateRecorded = Convert.ToDateTime(reader["DateRecorded"]),
                                    Comments = reader["Comments"]?.ToString() ?? ""
                                });
                            }
                        }
                    }
                }
                return marks;
            });
        }

        public async Task<List<Mark>> GetMarksByCourseAsync(int courseId)
        {
            return await Task.Run(() =>
            {
                var marks = new List<Mark>();
                using (var connection = new SQLiteConnection(_connectionString))
                {
                    connection.Open();

                    var query = "SELECT * FROM Marks WHERE CourseId = @courseId ORDER BY StudentName, DateRecorded DESC";
                    using (var command = new SQLiteCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@courseId", courseId);
                        using (var reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                marks.Add(new Mark
                                {
                                    Id = Convert.ToInt32(reader["Id"]),
                                    StudentId = Convert.ToInt32(reader["StudentId"]),
                                    StudentName = reader["StudentName"].ToString(),
                                    ExamId = Convert.ToInt32(reader["ExamId"]),
                                    ExamName = reader["ExamName"].ToString(),
                                    CourseId = Convert.ToInt32(reader["CourseId"]),
                                    CourseName = reader["CourseName"].ToString(),
                                    MarksObtained = Convert.ToDecimal(reader["MarksObtained"]),
                                    MaxMarks = Convert.ToDecimal(reader["MaxMarks"]),
                                    Grade = reader["Grade"].ToString(),
                                    DateRecorded = Convert.ToDateTime(reader["DateRecorded"]),
                                    Comments = reader["Comments"]?.ToString() ?? ""
                                });
                            }
                        }
                    }
                }
                return marks;
            });
        }

        public async Task<bool> AddMarkAsync(Mark mark)
        {
            return await Task.Run(() =>
            {
                using (var connection = new SQLiteConnection(_connectionString))
                {
                    connection.Open();

                    var query = @"
                    INSERT INTO Marks (StudentId, StudentName, ExamId, ExamName, CourseId, CourseName, MarksObtained, MaxMarks, Grade, DateRecorded, Comments)
                    VALUES (@studentId, @studentName, @examId, @examName, @courseId, @courseName, @marksObtained, @maxMarks, @grade, @dateRecorded, @comments)";

                    using (var command = new SQLiteCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@studentId", mark.StudentId);
                        command.Parameters.AddWithValue("@studentName", mark.StudentName);
                        command.Parameters.AddWithValue("@examId", mark.ExamId);
                        command.Parameters.AddWithValue("@examName", mark.ExamName);
                        command.Parameters.AddWithValue("@courseId", mark.CourseId);
                        command.Parameters.AddWithValue("@courseName", mark.CourseName);
                        command.Parameters.AddWithValue("@marksObtained", mark.MarksObtained);
                        command.Parameters.AddWithValue("@maxMarks", mark.MaxMarks);
                        command.Parameters.AddWithValue("@grade", mark.Grade);
                        command.Parameters.AddWithValue("@dateRecorded", mark.DateRecorded);
                        command.Parameters.AddWithValue("@comments", mark.Comments ?? "");

                        return command.ExecuteNonQuery() > 0;
                    }
                }
            });
        }

        public async Task<bool> UpdateMarkAsync(Mark mark)
        {
            return await Task.Run(() =>
            {
                using (var connection = new SQLiteConnection(_connectionString))
                {
                    connection.Open();

                    var query = @"
                    UPDATE Marks 
                    SET StudentId = @studentId, StudentName = @studentName, ExamId = @examId, ExamName = @examName,
                        CourseId = @courseId, CourseName = @courseName, MarksObtained = @marksObtained, MaxMarks = @maxMarks,
                        Grade = @grade, DateRecorded = @dateRecorded, Comments = @comments
                    WHERE Id = @id";

                    using (var command = new SQLiteCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@id", mark.Id);
                        command.Parameters.AddWithValue("@studentId", mark.StudentId);
                        command.Parameters.AddWithValue("@studentName", mark.StudentName);
                        command.Parameters.AddWithValue("@examId", mark.ExamId);
                        command.Parameters.AddWithValue("@examName", mark.ExamName);
                        command.Parameters.AddWithValue("@courseId", mark.CourseId);
                        command.Parameters.AddWithValue("@courseName", mark.CourseName);
                        command.Parameters.AddWithValue("@marksObtained", mark.MarksObtained);
                        command.Parameters.AddWithValue("@maxMarks", mark.MaxMarks);
                        command.Parameters.AddWithValue("@grade", mark.Grade);
                        command.Parameters.AddWithValue("@dateRecorded", mark.DateRecorded);
                        command.Parameters.AddWithValue("@comments", mark.Comments ?? "");

                        return command.ExecuteNonQuery() > 0;
                    }
                }
            });
        }

        // Timetable methods
        public async Task<List<Timetable>> GetAllTimetablesAsync()
        {
            return await Task.Run(() =>
            {
                var timetables = new List<Timetable>();
                using (var connection = new SQLiteConnection(_connectionString))
                {
                    connection.Open();

                    var query = "SELECT * FROM Timetables WHERE IsActive = 1 ORDER BY DayOfWeek, StartTime";
                    using (var command = new SQLiteCommand(query, connection))
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            timetables.Add(new Timetable
                            {
                                Id = Convert.ToInt32(reader["Id"]),
                                CourseId = Convert.ToInt32(reader["CourseId"]),
                                CourseName = reader["CourseName"].ToString(),
                                DayOfWeek = (DayOfWeek)Convert.ToInt32(reader["DayOfWeek"]),
                                StartTime = TimeSpan.Parse(reader["StartTime"].ToString()),
                                EndTime = TimeSpan.Parse(reader["EndTime"].ToString()),
                                Classroom = reader["Classroom"].ToString(),
                                TeacherName = reader["LectureName"].ToString(),
                                EffectiveDate = Convert.ToDateTime(reader["EffectiveDate"]),
                                EndDate = reader["EndDate"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(reader["EndDate"]),
                                IsActive = Convert.ToBoolean(reader["IsActive"])
                            });
                        }
                    }
                }
                return timetables;
            });
        }

        public async Task<List<Timetable>> GetTimetableByDayAsync(DayOfWeek dayOfWeek)
        {
            return await Task.Run(() =>
            {
                var timetables = new List<Timetable>();
                using (var connection = new SQLiteConnection(_connectionString))
                {
                    connection.Open();

                    var query = "SELECT * FROM Timetables WHERE DayOfWeek = @dayOfWeek AND IsActive = 1 ORDER BY StartTime";
                    using (var command = new SQLiteCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@dayOfWeek", (int)dayOfWeek);
                        using (var reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                timetables.Add(new Timetable
                                {
                                    Id = Convert.ToInt32(reader["Id"]),
                                    CourseId = Convert.ToInt32(reader["CourseId"]),
                                    CourseName = reader["CourseName"].ToString(),
                                    DayOfWeek = (DayOfWeek)Convert.ToInt32(reader["DayOfWeek"]),
                                    StartTime = TimeSpan.Parse(reader["StartTime"].ToString()),
                                    EndTime = TimeSpan.Parse(reader["EndTime"].ToString()),
                                    Classroom = reader["Classroom"].ToString(),
                                    TeacherName = reader["LectureName"].ToString(),
                                    EffectiveDate = Convert.ToDateTime(reader["EffectiveDate"]),
                                    EndDate = reader["EndDate"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(reader["EndDate"]),
                                    IsActive = Convert.ToBoolean(reader["IsActive"])
                                });
                            }
                        }
                    }
                }
                return timetables;
            });
        }

        public async Task<bool> AddTimetableEntryAsync(Timetable timetable)
        {
            return await Task.Run(() =>
            {
                using (var connection = new SQLiteConnection(_connectionString))
                {
                    connection.Open();

                    var query = @"
                    INSERT INTO Timetables (CourseId, CourseName, DayOfWeek, StartTime, EndTime, Classroom, LectureName, EffectiveDate, EndDate, IsActive)
                    VALUES (@courseId, @courseName, @dayOfWeek, @startTime, @endTime, @classroom, @lectureName, @effectiveDate, @endDate, @isActive)";

                    using (var command = new SQLiteCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@courseId", timetable.CourseId);
                        command.Parameters.AddWithValue("@courseName", timetable.CourseName);
                        command.Parameters.AddWithValue("@dayOfWeek", (int)timetable.DayOfWeek);
                        command.Parameters.AddWithValue("@startTime", timetable.StartTime.ToString());
                        command.Parameters.AddWithValue("@endTime", timetable.EndTime.ToString());
                        command.Parameters.AddWithValue("@classroom", timetable.Classroom);
                        command.Parameters.AddWithValue("@lectureName", timetable.TeacherName);
                        command.Parameters.AddWithValue("@effectiveDate", timetable.EffectiveDate);
                        command.Parameters.AddWithValue("@endDate", timetable.EndDate.HasValue ? (object)timetable.EndDate.Value : DBNull.Value);
                        command.Parameters.AddWithValue("@isActive", timetable.IsActive);

                        return command.ExecuteNonQuery() > 0;
                    }
                }
            });
        }

        public async Task<bool> DeleteTimetableEntryAsync(int id)
        {
            return await Task.Run(() =>
            {
                using (var connection = new SQLiteConnection(_connectionString))
                {
                    connection.Open();
                    var query = "UPDATE Timetables SET IsActive = 0 WHERE Id = @id";
                    using (var command = new SQLiteCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@id", id);
                        return command.ExecuteNonQuery() > 0;
                    }
                }
            });
        }

        // Additional utility methods
        public async Task<List<Mark>> GetAllMarksAsync()
        {
            return await Task.Run(() =>
            {
                var marks = new List<Mark>();
                using (var connection = new SQLiteConnection(_connectionString))
                {
                    connection.Open();

                    var query = "SELECT * FROM Marks ORDER BY DateRecorded DESC";
                    using (var command = new SQLiteCommand(query, connection))
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            marks.Add(new Mark
                            {
                                Id = Convert.ToInt32(reader["Id"]),
                                StudentId = Convert.ToInt32(reader["StudentId"]),
                                StudentName = reader["StudentName"].ToString(),
                                ExamId = Convert.ToInt32(reader["ExamId"]),
                                ExamName = reader["ExamName"].ToString(),
                                CourseId = Convert.ToInt32(reader["CourseId"]),
                                CourseName = reader["CourseName"].ToString(),
                                MarksObtained = Convert.ToDecimal(reader["MarksObtained"]),
                                MaxMarks = Convert.ToDecimal(reader["MaxMarks"]),
                                Grade = reader["Grade"].ToString(),
                                DateRecorded = Convert.ToDateTime(reader["DateRecorded"]),
                                Comments = reader["Comments"]?.ToString() ?? ""
                            });
                        }
                    }
                }
                return marks;
            });
        }

        public async Task<bool> EnrollStudentInCourseAsync(int studentId, int courseId)
        {
            return await Task.Run(() =>
            {
                using (var connection = new SQLiteConnection(_connectionString))
                {
                    connection.Open();

                    var query = @"
                    INSERT OR IGNORE INTO StudentCourseEnrollments (StudentId, CourseId, EnrollmentDate, IsActive)
                    VALUES (@studentId, @courseId, datetime('now'), 1)";

                    using (var command = new SQLiteCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@studentId", studentId);
                        command.Parameters.AddWithValue("@courseId", courseId);

                        return command.ExecuteNonQuery() > 0;
                    }
                }
            });
        }

        public async Task<bool> UnenrollStudentFromCourseAsync(int studentId, int courseId)
        {
            return await Task.Run(() =>
            {
                using (var connection = new SQLiteConnection(_connectionString))
                {
                    connection.Open();

                    var query = "UPDATE StudentCourseEnrollments SET IsActive = 0 WHERE StudentId = @studentId AND CourseId = @courseId";
                    using (var command = new SQLiteCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@studentId", studentId);
                        command.Parameters.AddWithValue("@courseId", courseId);

                        return command.ExecuteNonQuery() > 0;
                    }
                }
            });
        }

        public async Task<bool> DeleteLectureAsync(int lectureId)
        {
            using (var connection = new SQLiteConnection(_connectionString))
            {
                await connection.OpenAsync();
                using (var command = new SQLiteCommand("DELETE FROM Lectures WHERE Id = @Id", connection))
                {
                    command.Parameters.AddWithValue("@Id", lectureId);
                    var rowsAffected = await command.ExecuteNonQueryAsync();
                    return rowsAffected > 0;
                }
            }
        }

        // Lecture methods
        public async Task<List<Lecture>> GetAllLecturesAsync()
        {
            return await Task.Run(() =>
            {
                var lectures = new List<Lecture>();
                using (var connection = new SQLiteConnection(_connectionString))
                {
                    connection.Open();
                    var query = "SELECT * FROM Lectures WHERE IsActive = 1";
                    using (var command = new SQLiteCommand(query, connection))
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            lectures.Add(new Lecture
                            {
                                Id = Convert.ToInt32(reader["Id"]),
                                LectureName = reader["LectureName"].ToString(),
                                Email = reader["Email"].ToString(),
                                // ... other fields ...
                                IsActive = Convert.ToBoolean(reader["IsActive"])
                            });
                        }
                    }
                }
                return lectures;
            });
        }
    }
}