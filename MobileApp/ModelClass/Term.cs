using System;
using System.Collections.Generic;
using System.Text;
using SQLite;

namespace MobileApp.ModelClass
{
    public class Term
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public string Title { get; set; }
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
    }
}
