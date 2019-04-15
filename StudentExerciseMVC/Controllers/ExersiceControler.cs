using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using StudentExerciseMVC.Models;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace StudentExerciseMVC.Controllers
{
    public class ExersizeController : Controller
    {
        private readonly IConfiguration _configuration;

        public ExersizeController(IConfiguration configuration)
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
                    cmd.CommandText = @"SELECT e.Id AS ExersiizeId,
                                               e.Exercise_Name,
                                                e.Exercise_Language
                                          FROM Exercise e";
                    SqlDataReader reader = cmd.ExecuteReader();

                    List<Exercise> exercises = new List<Exercise>();

                    while (reader.Read())
                    {

                        Exercise exercise = new Exercise()
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("ExersiizeId")),
                            Name = reader.GetString(reader.GetOrdinal("Exercise_Name")),
                            Language = reader.GetString(reader.GetOrdinal("Exercise_Language")),
                        };

                        exercises.Add(exercise);

                    }

                    reader.Close();
                    return View(exercises);
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
                    cmd.CommandText = @"SELECT Id ,
                                               Exercise_Name,
                                                Exercise_Language
                                          FROM Exercise
                                        WHERE Id = @Id;";
                    cmd.Parameters.Add(new SqlParameter("@Id", Id));
                    SqlDataReader reader = cmd.ExecuteReader();
                    Exercise exercise = null;

                    if (reader.Read())
                    {
                        exercise = new Exercise
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("Id")),
                            Name = reader.GetString(reader.GetOrdinal("Exercise_Name")),
                            Language = reader.GetString(reader.GetOrdinal("Exercise_Language")),
                        };

                    }
                    reader.Close();
                    return View(exercise);
                }

            }
        }


        private Exercise GetStudentById(int Id)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"SELECT e.Id ,
                                               e.Exercise_Name,
                                                e.Exercise_Language
                                          FROM Exercise e
                                        WHERE e.Id = @Id;";
                    cmd.Parameters.Add(new SqlParameter("@Id", Id));
                    SqlDataReader reader = cmd.ExecuteReader();
                    Exercise exercise = null;

                    if (reader.Read())
                    {
                        exercise = new Exercise
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("e.Id")),
                            Name = reader.GetString(reader.GetOrdinal("Exercise_Name")),
                            Language = reader.GetString(reader.GetOrdinal("Exercise_Language")),
                        };

                    }

                    reader.Close();

                    return exercise;
                }

            }
        }

        public ActionResult Delete(int Id)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"SELECT Id as ExersiizeId,
                                               Exercise_Name,
                                                Exercise_Language
                                          FROM Exercise
                                        WHERE Id = @Id;";


                    cmd.Parameters.Add(new SqlParameter("@Id", Id));
                    SqlDataReader reader = cmd.ExecuteReader();
                    Exercise exercise = null;

                    if (reader.Read())
                    {
                        exercise = new Exercise
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("ExersiizeId")),
                            Name = reader.GetString(reader.GetOrdinal("Exercise_Name")),
                            Language = reader.GetString(reader.GetOrdinal("Exercise_Language")),
                        };

                    }

                    reader.Close();

                    return View(exercise);
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
                    cmd.CommandText = "DELETE FROM StudentExercise WHERE ExerciseId = @Id;";
                    cmd.CommandText += "DELETE FROM Exercise WHERE Id = @Id;";

                    cmd.Parameters.Add(new SqlParameter("@Id", Id));

                    cmd.ExecuteNonQuery();
                }
            }

            return RedirectToAction(nameof(Index));
        }



        //        public ActionResult Create()
        //        {
        //            StudentCreateViewModel viewModel =
        //                new StudentCreateViewModel(_configuration.GetConnectionString("DefaultConnection"));
        //            return View(viewModel);
        //        }

        //        // POST: Students/Create
        //        [HttpPost]
        //        [ValidateAntiForgeryToken]
        //        public ActionResult Create(StudentCreateViewModel viewModel)
        //        {
        //            try
        //            {
        //                using (SqlConnection conn = Connection)
        //                {
        //                    conn.Open();
        //                    using (SqlCommand cmd = conn.CreateCommand())
        //                    {
        //                        cmd.CommandText = @"INSERT INTO students (First_Name, Last_Name, Slack_Handle, Cohort_id)
        //                                             VALUES (@First_Name, @Last_Name, @Slack_Handle, @Cohort_id)";

        //                        cmd.Parameters.Add(new SqlParameter("@First_Name", viewModel.Student.FirstName));
        //                        cmd.Parameters.Add(new SqlParameter("@Last_Name", viewModel.Student.LastName));
        //                        cmd.Parameters.Add(new SqlParameter("@Slack_Handle", viewModel.Student.SlackHandle));
        //                        cmd.Parameters.Add(new SqlParameter("@Cohort_id", viewModel.Student.CohortId));

        //                        cmd.ExecuteNonQuery();

        //                        return RedirectToAction(nameof(Index));
        //                    }
        //                }
        //            }
        //            catch
        //            {

        //                return View();
        //            }
        //        }



        //        private List<Cohort> GetAllCohorts()
        //        {
        //            using (SqlConnection conn = Connection)
        //            {
        //                conn.Open();
        //                using (SqlCommand cmd = conn.CreateCommand())
        //                {
        //                    cmd.CommandText = @"SELECT Id, Cohort_Name from Cohort;";
        //                    SqlDataReader reader = cmd.ExecuteReader();

        //                    List<Cohort> cohorts = new List<Cohort>();

        //                    while (reader.Read())
        //                    {
        //                        cohorts.Add(new Cohort
        //                        {
        //                            Id = reader.GetInt32(reader.GetOrdinal("Id")),
        //                            Name = reader.GetString(reader.GetOrdinal("Cohort_Name"))
        //                        });
        //                    }
        //                    reader.Close();

        //                    return cohorts;
        //                }
        //            }

        //        }


        //        // GET: Instructors/Edit/5
        //        public ActionResult Edit(int id)
        //        {
        //            Student Student = GetStudentById(id);
        //            if (Student == null)
        //            {
        //                return NotFound();
        //            }
        //            //This piece of code keeps the type correct to pass into the edit
        //            StudentEditViewModel viewModel = new StudentEditViewModel
        //            {
        //                Cohorts = GetAllCohorts(),
        //                Student = Student
        //            };

        //            return View(viewModel);
        //        }

        //        // POST: Instructors/Edit/5
        //        [HttpPost]
        //        [ValidateAntiForgeryToken]
        //        public ActionResult Edit(int Id, StudentEditViewModel viewModel)
        //        {
        //            try
        //            {
        //                using (SqlConnection conn = Connection)
        //                {
        //                    conn.Open();
        //                    using (SqlCommand cmd = conn.CreateCommand())
        //                    {
        //                        cmd.CommandText = @"UPDATE students
        //                                           SET First_Name = @First_Name,
        //                                               Last_Name = @Last_Name,
        //                                               Slack_Handle = @Slack_Handle,
        //                                               Cohort_id = @Cohort_id
        //                                         WHERE Id = @Id;";
        //                        cmd.Parameters.Add(new SqlParameter("@First_Name", viewModel.Student.FirstName));
        //                        cmd.Parameters.Add(new SqlParameter("@Last_Name", viewModel.Student.LastName));
        //                        cmd.Parameters.Add(new SqlParameter("@Slack_Handle", viewModel.Student.SlackHandle));
        //                        cmd.Parameters.Add(new SqlParameter("@Cohort_id", viewModel.Student.CohortId));
        //                        cmd.Parameters.Add(new SqlParameter("@Id", Id));

        //                        cmd.ExecuteNonQuery();

        //                        return RedirectToAction(nameof(Index));
        //                    }
        //                }
        //            }
        //            catch
        //            {
        //                viewModel.Cohorts = GetAllCohorts();
        //                return View(viewModel);
        //            }

    }

        }


