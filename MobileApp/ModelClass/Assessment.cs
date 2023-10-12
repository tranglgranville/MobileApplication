using System;
using System.Collections.Generic;
using System.Text;
using SQLite;

namespace MobileApp.ModelClass
{
    public class Assessment
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public int CourseId { get; set; } //Foreign Key
        public string Name { get; set; }
        public DateTime Start{ get; set; }
        public DateTime End{ get; set; }
        public bool Notification { get; set; }
    }
}
