using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace busproject.Models
{
    public class Chuyenxe
    {
        public string id { get; set; }
        public string start { get; set; }
        public string end { get; set; }
        public DateTime ngaydi { get; set; }
        public decimal gia { get; set; }
        public int sokm { get; set; }
        public string loaive { get; set; }
    }
}