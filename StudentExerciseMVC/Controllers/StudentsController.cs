using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using StudentExerciseMVC.Models;
using StudentExerciseMVC.Models.ViewModels;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace StudentExerciseMVC.Controllers
{
    public class StudentsController : Controller
    {
        private readonly IConfiguration _configuration;

        public StudentsController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public SqlConnection Connection => new SqlConnection(_configuration.GetConnectionString("DefaultConnection"));

        // GET: Students
        public ActionResult Index()
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"SELECT s.Id AS StudentId,
                                               s.First_Name,
                                               s.Last_Name,
                                               s.Slack_Handle,
                                               s.Cohort_id,
                                               c.Cohort_Name AS CohortName
                                          FROM students s
                                               LEFT JOIN Cohort c ON s.Cohort_id = c.Id;";

                    SqlDataReader reader = cmd.ExecuteReader();

                    List<Student> students = new List<Student>();

                    while (reader.Read())
                    {

                        Student student = new Student()
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("StudentId")),
                            FirstName = reader.GetString(reader.GetOrdinal("First_Name")),
                            LastName = reader.GetString(reader.GetOrdinal("Last_Name")),
                            SlackHandle = reader.GetString(reader.GetOrdinal("Slack_Handle")),
                            CohortId = reader.GetInt32(reader.GetOrdinal("Cohort_id")),
                            Cohort = new Cohort
                            {
                                Name = reader.GetString(reader.GetOrdinal("CohortName")),
                            }
                        };

                        students.Add(student);

                    }

                    reader.Close();
                    return View(students);
                }
            }

        }




        public ActionResult Details(int Id)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {

                   cmd.CommandText = @"SELECT s.Id,
                                               s.First_Name,
                                               s.Last_Name,
                                               s.Slack_Handle,
                                               s.Cohort_id,
                                               c.Cohort_Name AS CohortName,
                                               e.Exercise_Name,
                                               e.Exercise_Language
                                                 FROM students s
                                               LEFT JOIN Cohort c ON s.Cohort_id = c.Id
                                               LEFT JOIN Exercise e ON s.id = e.Id
                                              WHERE e.Id = @Id
                                               select se.StudentId,
                                               se.ExerciseId
                                               from StudentExercise se
                                               LEFT JOIN students st ON st.id = se.StudentId
                                               WHERE se.StudentId = @Id;";

                    cmd.Parameters.Add(new SqlParameter("@Id", Id));
                    SqlDataReader reader = cmd.ExecuteReader();

                    Student student = null;
                    Exercise Exercise = null;
                    if (reader.Read())
                    {
                        student = new Student
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("Id")),
                            FirstName = reader.GetString(reader.GetOrdinal("First_Name")),
                            LastName = reader.GetString(reader.GetOrdinal("Last_Name")),
                            SlackHandle = reader.GetString(reader.GetOrdinal("Slack_Handle")),
                            CohortId = reader.GetInt32(reader.GetOrdinal("Cohort_id")),

                            Exercise = new Exercise
                            {
                                Name = reader.GetString(reader.GetOrdinal("Exercise_Name")),
                                Language = reader.GetString(reader.GetOrdinal("Exercise_Language"))
                            },
                            Cohort = new Cohort
                            {
                                Name = reader.GetString(reader.GetOrdinal("CohortName")),
                            }
                        };
                    }
                    reader.Close();
                    return View(student);
                }
            }
        }


        private Student GetStudentById(int id)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"SELECT s.Id,
                                               s.First_Name,
                                               s.Last_Name,
                                               s.Slack_Handle,
                                               s.Cohort_id,
                                               c.Cohort_Name AS CohortName
                                          FROM students s
                                               LEFT JOIN Cohort c ON s.Cohort_id = c.Id
                                              WHERE s.Id = @Id;";
                    cmd.Parameters.Add(new SqlParameter("@id", id));
                    SqlDataReader reader = cmd.ExecuteReader();

                    Student student = null;

                    if (reader.Read())
                    {
                        student = new Student
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("Id")),
                            FirstName = reader.GetString(reader.GetOrdinal("First_Name")),
                            LastName = reader.GetString(reader.GetOrdinal("Last_Name")),
                            SlackHandle = reader.GetString(reader.GetOrdinal("Slack_Handle")),
                            CohortId = reader.GetInt32(reader.GetOrdinal("Cohort_id")),
                            Cohort = new Cohort
                            {
                                Name = reader.GetString(reader.GetOrdinal("CohortName")),
                            }
                        };
                    }

                    reader.Close();

                    return student;

                }
            }
        }

        public ActionResult Delete(int id)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"SELECT s.Id,
                                               s.First_Name,
                                               s.Last_Name,
                                               s.Slack_Handle,
                                               s.Cohort_id,
                                               c.Cohort_Name AS CohortName
                                          FROM students s
                                               LEFT JOIN Cohort c ON s.Cohort_id = c.Id
                                              WHERE s.Id = @Id;";
                    cmd.Parameters.Add(new SqlParameter("@id", id));
                    SqlDataReader reader = cmd.ExecuteReader();

                    Student studenta = null;

                    if (reader.Read())
                    {
                        studenta = new Student
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("Id")),
                            FirstName = reader.GetString(reader.GetOrdinal("First_Name")),
                            LastName = reader.GetString(reader.GetOrdinal("Last_Name")),
                            SlackHandle = reader.GetString(reader.GetOrdinal("Slack_Handle")),
                            CohortId = reader.GetInt32(reader.GetOrdinal("Cohort_id")),
                            Cohort = new Cohort
                            {
                                Name = reader.GetString(reader.GetOrdinal("CohortName")),
                            }
                        };
                    }

                    reader.Close();

                    return View(studenta);

                }
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int Id, IFormCollection collection)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = "DELETE FROM StudentExercise WHERE StudentId = @Id;";
                    cmd.CommandText += "DELETE FROM students WHERE Id = @Id;";

                    cmd.Parameters.Add(new SqlParameter("@Id", Id));

                    cmd.ExecuteNonQuery();
                }
            }

            return RedirectToAction(nameof(Index));
        }



        public ActionResult Create()
        {
            StudentCreateViewModel viewModel =
                new StudentCreateViewModel(_configuration.GetConnectionString("DefaultConnection"));
            return View(viewModel);
        }

        // POST: Students/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(StudentCreateViewModel viewModel)
        {
            try
            {
                using (SqlConnection conn = Connection)
                {
                    conn.Open();
                    using (SqlCommand cmd = conn.CreateCommand())
                    {
                        cmd.CommandText = @"INSERT INTO students (First_Name, Last_Name, Slack_Handle, Cohort_id)
                                             VALUES (@First_Name, @Last_Name, @Slack_Handle, @Cohort_id)";

                        cmd.Parameters.Add(new SqlParameter("@First_Name", viewModel.Student.FirstName));
                        cmd.Parameters.Add(new SqlParameter("@Last_Name", viewModel.Student.LastName));
                        cmd.Parameters.Add(new SqlParameter("@Slack_Handle", viewModel.Student.SlackHandle));
                        cmd.Parameters.Add(new SqlParameter("@Cohort_id", viewModel.Student.CohortId));

                        cmd.ExecuteNonQuery();

                        return RedirectToAction(nameof(Index));
                    }
                }
            }
            catch
            {

                return View();
            }
        }



        private List<Cohort> GetAllCohorts()
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"SELECT Id, Cohort_Name from Cohort;";
                    SqlDataReader reader = cmd.ExecuteReader();

                    List<Cohort> cohorts = new List<Cohort>();

                    while (reader.Read())
                    {
                        cohorts.Add(new Cohort
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("Id")),
                            Name = reader.GetString(reader.GetOrdinal("Cohort_Name"))
                        });
                    }
                    reader.Close();

                    return cohorts;
                }
            }

        }


        // GET: Instructors/Edit/5
        public ActionResult Edit(int id)
        {
            Student Student = GetStudentById(id);
            if (Student == null)
            {
                return NotFound();
            }
            //This piece of code keeps the type correct to pass into the edit
            StudentEditViewModel viewModel = new StudentEditViewModel
            {
                Cohorts = GetAllCohorts(),
                Student = Student
            };

            return View(viewModel);
        }

        // POST: Instructors/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int Id, StudentEditViewModel viewModel)
        {
            try
            {
                using (SqlConnection conn = Connection)
                {
                    conn.Open();
                    using (SqlCommand cmd = conn.CreateCommand())
                    {
                        cmd.CommandText = @"UPDATE students
                                           SET First_Name = @First_Name,
                                               Last_Name = @Last_Name,
                                               Slack_Handle = @Slack_Handle,
                                               Cohort_id = @Cohort_id
                                         WHERE Id = @Id;";
                        cmd.Parameters.Add(new SqlParameter("@First_Name", viewModel.Student.FirstName));
                        cmd.Parameters.Add(new SqlParameter("@Last_Name", viewModel.Student.LastName));
                        cmd.Parameters.Add(new SqlParameter("@Slack_Handle", viewModel.Student.SlackHandle));
                        cmd.Parameters.Add(new SqlParameter("@Cohort_id", viewModel.Student.CohortId));
                        cmd.Parameters.Add(new SqlParameter("@Id", Id));

                        cmd.ExecuteNonQuery();

                        return RedirectToAction(nameof(Index));
                    }
                }
            }
            catch
            {
                viewModel.Cohorts = GetAllCohorts();
                return View(viewModel);
            }

        }

    }
}
