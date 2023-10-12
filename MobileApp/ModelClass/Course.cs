using System;
using System.Collections.Generic;
using System.Text;
using SQLite;

namespace MobileApp.ModelClass
{
    public class Course
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public int TermId { get; set; } //Foreign Key from Term class/table
        public string Name { get; set; }
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
        public string Status { get; set; }
        public string InstructorName { get; set; }
        public string InstructorEmail { get; set; }
        public string InstructorPhone { get; set; }        
        public bool Notification { get; set; }
        public string Note { get; set; }
    } 
}
