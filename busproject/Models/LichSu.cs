using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace busproject.Models
{
    public class LichSu
    {
        public string id { get; set; }
        public int soluongve { get; set; }
        public string idkh {  get; set; }
        public string tenkh { get; set; }
        public DateTime ngaydi {  get; set; }
        public string start {  get; set; }
        public string end { get; set; }
        public decimal giave { get; set; }      
        public DateTime ngaydatve { get; set; }
    }
}