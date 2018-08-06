using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Applications.Models
{
    public class Application
    {

        public int ApplicationID { get; set; }
        public int ApplicationTypeID { get; set; }
        public int ItemID { get; set; }
        public int Count { get; set; }
        public string Comment { get; set; }
        public DateTime Date { get; set; }

    }
}