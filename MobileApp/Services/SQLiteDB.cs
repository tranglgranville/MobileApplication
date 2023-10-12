using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using SQLite;
using Xamarin.Forms;
using System.IO;
using System.Threading.Tasks;
using MobileApp.ModelClass;
using Xamarin.Essentials;
using System.Collections.ObjectModel;

namespace MobileApp.Services
{
    public static class SQLiteDB
    {
        private static SQLiteAsyncConnection _db;
        private static SQLiteConnection _dbConnection;
        static async Task Init()
        {
            if(_db != null)
            {
                return;
            }

            var databasePath = Path.Combine(FileSystem.AppDataDirectory, "mobile_app.db");

            _db = new SQLiteAsyncConnection(databasePath);
            _dbConnection = new SQLiteConnection(databasePath);

            await _db.CreateTableAsync<Term>();
            await _db.CreateTableAsync<Course>();
            await _db.CreateTableAsync<Assessment>();
        }
        #region DemoData
        public static async void LoadSampleData()
        {
            //each term has 6 courses
            await Init();
            Term term1 = new Term
            {
                Title = "Spring 2023",
                Start = new DateTime(2023,01,01).Date,
                End = new DateTime(2023,06,30).Date,
            };
            await _db.InsertAsync(term1);

            Course course1 = new Course
            {
                //Id = 1,
                TermId = term1.Id,
                Name = "Calculus I",
                Start = new DateTime(2023,01,01),
                End = new DateTime(2023,01,31),
                Status = "Completed",
                InstructorName = "Trang Granville",
                InstructorEmail = "tgranvi@wgu.edu",
                InstructorPhone = "2067128910",
                Notification = true,
                Note = "",
            };
            await _db.InsertAsync(course1);

            Assessment ass1 = new Assessment
            {
                CourseId = course1.Id,
                Name = "OA Calculus I",
                Start = new DateTime(2023, 01, 15),
                End = new DateTime(2023, 01, 16),          
                Notification = true,
            };
            await _db.InsertAsync(ass1);

            Assessment ass11 = new Assessment
            {
                CourseId = course1.Id,
                Name = "PA Calculus I",
                Start = new DateTime(2023, 01, 24),
                End = new DateTime(2023, 01, 26),
                Notification = true,
            };
            await _db.InsertAsync(ass11);

            Course course2 = new Course
            {
                TermId = term1.Id,
                Name = "Introduction to Chemistry",
                Start = new DateTime(2023, 02, 01),
                End = new DateTime(2023, 02, 28),
                Status = "Completed",
                InstructorName = "William Brown",
                InstructorEmail = "Bwill@wgu.edu",
                InstructorPhone = "4256748769",
                Notification = true,
                Note = "",
            };
            await _db.InsertAsync(course2);

            Assessment ass2 = new Assessment
            {
                CourseId = course2.Id,
                Name = "PA Introduction to Chemistry",
                Start = new DateTime(2023, 02, 25),
                End = new DateTime(2023, 02, 27),
                Notification = true,
            };
            await _db.InsertAsync(ass2);

            Term term2 = new Term
            {
                Title = "Fall 2023",
                Start = new DateTime(2023, 07, 01),
                End = new DateTime(2023, 12, 30),
            };
            await _db.InsertAsync(term2);

            Course course3 = new Course
            {
                TermId = term2.Id,
                Name = "Physic I",
                Start = new DateTime(2023, 07, 01),
                End = new DateTime(2023, 07, 31),
                Status = "In-Progress",
                InstructorName = "William Brown",
                InstructorEmail = "Bwill@wgu.edu",
                InstructorPhone = "2537852526",
                Notification = true,
                Note = "",
            };
            await _db.InsertAsync(course3);

            Assessment ass3 = new Assessment
            {
                CourseId = course3.Id,
                Name = "PA Physic I",
                Start = new DateTime(2023, 07, 20),
                End = new DateTime(2023, 07, 27),
                
                Notification = true,
            };
            await _db.InsertAsync(ass3);
        }
        public static async Task ClearSampleData()
        {
            await Init();
            await _db.DropTableAsync<Term>();
            await _db.DropTableAsync<Course>();
            await _db.DropTableAsync<Assessment>();
            _db = null;
            _dbConnection = null;
        }
        /*public static void LoadSyncSampleData()
        {
            var databasePath = Path.Combine(FileSystem.AppDataDirectory, "mobile_app.dp");
            _dbConnection = new SQLiteConnection(databasePath);
            _dbConnection.CreateTable<Term>();
            _dbConnection.CreateTable<Course>();
            _dbConnection.CreateTable<Assessment>();

            Term sync1Term = new Term()
            {
                Title = "Term 1",
                Start = new DateTime(2023, 07, 01).Date,
                End = new DateTime(2023, 12, 30).Date,
            };
            _dbConnection.Insert(sync1Term);

            Course sync1Course = new Course
            {
                TermId = sync1Term.Id,
                Name = "Introduction to Communication",
                Start = new DateTime(2023, 01, 01),
                End = new DateTime(2023, 06, 30),
                Status = "Completed",
                InstructorName = "William Brown",
                InstructorEmail = "Bwill@wgu.edu",
                InstructorPhone = "2222222222",
                Notification = true,
                Note = "",
            };
            _dbConnection.Insert(sync1Course);

            Assessment sync1Assessment = new Assessment 
            {
                CourseId = sync1Course.Id,
                Name = "Assessment 3",
                Start = new DateTime(2023, 08, 01),
                End = new DateTime(2023, 08, 02),
                Notification = true,
            };
        }*/
        /*public static async void LoadSampleDataSql()
        {
            await Init();

            int lastRowId;
            await _db.ExecuteAsync("INSERT INTO Term(TITTLE, STARTDATE, ENDDATE) VALUES (?, ?, ?)");
        }*/
        #endregion

        #region Count Determination
        public static async Task<int> GetCourseCountAsync(int selectTermId)
        {
            int courseCount = await _db.ExecuteScalarAsync<int>($"Select Count(*) from Course where TermId = ?", selectTermId);
            return courseCount;
        }
        public static async Task<int> GetAssessmentCountAsync(int selectedCourseId)
        {
            int assessmentCount = await _db.ExecuteScalarAsync<int>($"Select Count(*) from Assessment where CourseId = ?", selectedCourseId);
            return assessmentCount;

        }
        #endregion

        #region Looping through table records
        /*public static async void LoopingAssessmentTable() //p94
        {
            await Init();
            var allAssessRecord = _dbConnection.Query<Assessment>("SELECT * FROM Assessment");

            foreach (var termRecord in allAssessRecord)
            {
                Console.WriteLine("Name " + termRecord.Name);
            }
        }
        public static async Task<IEnumerable<Assessment>> GetNotificationAssessment()
        {
            await Init();
            var allAssessRecord = _dbConnection.Query<Assessment>("SELECT * FROM Assessment");

            return allAssessRecord;
        }*/

        #endregion

        #region Course methods
        public static async Task CourseAdd(int termID, string name, DateTime start, DateTime end, string status, 
            string instructorName, string email, string phone, bool noti, string note)
        {
            await Init();
            var course = new Course
            {
                TermId =termID,
                Name = name,
                Start = start,
                End = end,
                Status = status,
                InstructorName = instructorName,
                InstructorEmail = email,
                InstructorPhone = phone,
                Notification = noti,
                Note = note,
            };

            await _db.InsertAsync(course);

            var id = course.Id; //return the CourseId
        }
        public static async Task CourseRemove(int id)
        {
            await Init();
            await _db.DeleteAsync<Course>(id);
        }
        public static async Task<IEnumerable<Course>> GetCourse(int termId) //get specific course ID
        {
            await Init();
            var courses = await _db.Table<Course>().Where(i => i.TermId == termId).ToListAsync();
            return courses;
        }
        public static async Task<IEnumerable<Course>> GetCourse() // get all course ID
        {
            await Init();
            var courses = await _db.Table<Course>().ToListAsync();
            return courses;
        }
        public static async Task CourseUpdate(int id, string name, DateTime start, DateTime end, string status,
            string instructorName, string email, string phone, bool noti, string note)
        {
            await Init();
            // termid will not change, oaid and paid will be added later
            var courseQuery = await _db.Table<Course>().Where(i => i.Id == id).FirstOrDefaultAsync();

            if(courseQuery != null)
            {
                courseQuery.Name = name;
                courseQuery.Start = start;
                courseQuery.End = end;
                courseQuery.Status = status;
                courseQuery.InstructorName = instructorName;
                courseQuery.InstructorEmail = email;
                courseQuery.InstructorPhone = phone;
                courseQuery.Notification = noti;
                courseQuery.Note = note;

                await _db.UpdateAsync(courseQuery);
            }
        }
        #endregion

        #region Term methods
        public static async Task TermAdd(string title, DateTime start, DateTime end)
        {
            await Init();
            var term = new Term()
            {
                Title = title,
                Start = start,
                End = end,
            };
            await _db.InsertAsync(term);
            var id = term.Id; //term ID
        }
        public static async Task TermRemove(int id)
        {
            await Init();
            await _db.DeleteAsync<Term>(id);
        }
        public static async Task<IEnumerable<Term>> GetTerms() //get term
        {
            await Init();
            var terms = await _db.Table<Term>().ToListAsync();
            return terms;
        }
        
        public static async Task TermUpdate(int id, string title, DateTime start, DateTime end)
        {
            await Init();

            var termQuery = await _db.Table<Term>()
                .Where(i => i.Id == id)
                .FirstOrDefaultAsync();

            if(termQuery != null)
            {
                termQuery.Title = title;
                termQuery.Start = start;
                termQuery.End = end;

                await _db.UpdateAsync(termQuery);
            }
        }

        #endregion       

        #region Assessments
        public static async Task AssessmentAdd(int courseID, string name, DateTime start, DateTime end, bool noti)
        {
            await Init();
            var assessment = new Assessment()
            {
                CourseId = courseID,
                Name = name,
                Start = start,
                End = end,                
                Notification = noti,
            };
            await _db.InsertAsync(assessment);
            var id = assessment.Id;
        }
        public static async Task AssessmentRemove(int id)
        {
            await Init();
            await _db.DeleteAsync<Assessment>(id);
        }
        public static async Task<IEnumerable<Assessment>> GetAssessment(int courseID)
        {
            await Init();
            var assessment = await _db.Table<Assessment>().Where(i => i.CourseId == courseID).ToListAsync();
            return assessment;
        }
        public static async Task<IEnumerable<Assessment>> GetAssessment() //overload used with notifications
        {
            await Init();

            var assessments = await _db.Table<Assessment>().ToListAsync();
            return assessments;
        }
        public static async Task AssessmentUpdate(int id, string name, DateTime start, DateTime end, bool noti)
        {
            await Init();

            var assessQuery = await _db.Table<Assessment>()
               .Where(i => i.Id == id)
               .FirstOrDefaultAsync();

            if (assessQuery != null)
            {
                assessQuery.Name = name;
                assessQuery.Start = start;
                assessQuery.End = end;
                assessQuery.Notification = noti;

                await _db.UpdateAsync(assessQuery);
            }
        }
        #endregion
        
    }
}
