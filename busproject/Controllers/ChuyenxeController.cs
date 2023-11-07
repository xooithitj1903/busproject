using busproject.Models;
using FireSharp.Interfaces;
using FireSharp.Response;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Globalization;

namespace busproject.Controllers
{
    public class ChuyenxeController : Controller
    {
        // GET: Chuyenxe
        IFirebaseConfig config = new FireSharp.Config.FirebaseConfig
        {
            AuthSecret = "HIIV929CfeQ8GGTRMig0ZMaL65iep8B8ftmoAt9R",
            BasePath = "https://busproject-5ba81-default-rtdb.firebaseio.com/"
        };
        
        IFirebaseClient client;
        // GET: DangnhapDangky
         public ActionResult Index()
        {
            client = new FireSharp.FirebaseClient(config);
            FirebaseResponse response = client.Get("chuyenxe/");
            dynamic data = JsonConvert.DeserializeObject<dynamic>(response.Body);
            var list = new List<Chuyenxe>();
            foreach (var item in data)
            {
                list.Add(JsonConvert.DeserializeObject<Chuyenxe>(((JProperty)item).Value.ToString()));
            }
            return View(list);
        }
        public ActionResult Indexadmin()
        {
            client = new FireSharp.FirebaseClient(config);
            FirebaseResponse response = client.Get("chuyenxe/");
            dynamic data = JsonConvert.DeserializeObject<dynamic>(response.Body);
            var list = new List<Chuyenxe>();
            foreach (var item in data)
            {
                list.Add(JsonConvert.DeserializeObject<Chuyenxe>(((JProperty)item).Value.ToString()));
            }
            return View(list);
        }
        [HttpGet]
        public ActionResult Edit(string id)
        {
            client = new FireSharp.FirebaseClient(config);
            FirebaseResponse response = client.Get("chuyenxe/" + id);
            Chuyenxe data = JsonConvert.DeserializeObject<Chuyenxe>(response.Body);
            return View(data);
        }

        [HttpPost]
        public ActionResult Edit(Chuyenxe sanpham)
        {
            client = new FireSharp.FirebaseClient(config);
            FirebaseResponse response = client.Set("chuyenxe/" + sanpham.id, sanpham);
            Chuyenxe data = JsonConvert.DeserializeObject<Chuyenxe>(response.Body);
            return RedirectToAction("Indexadmin");
        }
        [HttpGet]
        public ActionResult Delete(string id)
        {
            client = new FireSharp.FirebaseClient(config);
            FirebaseResponse response = client.Delete("chuyenxe/" + id);
            Chuyenxe data = JsonConvert.DeserializeObject<Chuyenxe>(response.Body);
            return RedirectToAction("Indexadmin");
        }
        public ActionResult indexls()
        {
            client = new FireSharp.FirebaseClient(config);
            FirebaseResponse response = client.Get("lichsu/");
            dynamic data = JsonConvert.DeserializeObject<dynamic>(response.Body);
            var list = new List<LichSu>();
            foreach (var item in data)
            {
                list.Add(JsonConvert.DeserializeObject<LichSu>(((JProperty)item).Value.ToString()));
            }
            return View(list);
        }
        [HttpGet]
        public ActionResult Editls(string id)
        {
            client = new FireSharp.FirebaseClient(config);
            FirebaseResponse response = client.Get("lichsu/" + id);
            LichSu data = JsonConvert.DeserializeObject<LichSu>(response.Body);
            return View(data);
        }

        [HttpPost]
        public ActionResult Editls(LichSu sanpham)
        {
            client = new FireSharp.FirebaseClient(config);
            FirebaseResponse response = client.Set("lichsu/" + sanpham.id, sanpham);
            LichSu data = JsonConvert.DeserializeObject<LichSu>(response.Body);
            return RedirectToAction("indexls");
        }
        [HttpGet]
        public ActionResult Deletels(string id)
        {
            client = new FireSharp.FirebaseClient(config);
            FirebaseResponse response = client.Delete("lichsu/" + id);
            LichSu data = JsonConvert.DeserializeObject<LichSu>(response.Body);
            return RedirectToAction("indexls");
        }
        [HttpGet]
        public ActionResult ThemChuyenxe()
        {
            return View();
        }
        [HttpPost]
        public ActionResult ThemChuyenxe(Chuyenxe chuyenxe)
        {
            try
            {
                AddStudentToFirebase(chuyenxe);
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
            }

            return View();
        }
        private void AddStudentToFirebase(Chuyenxe chuyenxe)
        {
            client = new FireSharp.FirebaseClient(config);
            var data = chuyenxe;
            PushResponse response = client.Push("chuyenxe/", data);
            data.id = response.Result.name;
            SetResponse setResponse = client.Set("chuyenxe/" + data.id, data);
        }
        public ActionResult DatVe(FormCollection form, string id)
        {
            if(Session["ID"] == null)
            {
                return RedirectToAction("Dangnhap", "TaiKhoan");
            }
            string id2 = Session["ID"] as string;
            client = new FireSharp.FirebaseClient(config);
            FirebaseResponse response = client.Get("chuyenxe/" + id);
            Chuyenxe data = JsonConvert.DeserializeObject<Chuyenxe>(response.Body);
            client = new FireSharp.FirebaseClient(config);
            FirebaseResponse response2 = client.Get("TaiKhoan/" + id2);
            TaiKhoan data2 = JsonConvert.DeserializeObject<TaiKhoan>(response2.Body);
            LichSu lichsu = new LichSu();
            lichsu.start = data.start;
            lichsu.end = data.end;
            lichsu.idkh = data2.Id;
            lichsu.tenkh = data2.Name;
            lichsu.giave = data.gia;
            lichsu.ngaydatve = DateTime.Now.Date;
            string ngaydiDate = form["ngaydi"];

            // Chuyển đổi ngày từ kiểu dữ liệu date sang datetime
            DateTime ngaydiDatetime = DateTime.Parse(ngaydiDate);

            // Gán giá trị vào trường lichsu.ngaydi
            lichsu.ngaydi = ngaydiDatetime;
            string soluongStr = form["Soluong"];

            // Chuyển đổi giá trị từ kiểu string (form["Soluong"]) sang kiểu int
            if (int.TryParse(soluongStr, out int soluong))
            {
                // Gán giá trị vào trường lichsu.soluongve nếu chuyển đổi thành công
                lichsu.soluongve = soluong;
            }
            AddStudentToFirebase(lichsu);
            return RedirectToAction("indexls");
        }
        private void AddStudentToFirebase(LichSu lichsu)
        {
            client = new FireSharp.FirebaseClient(config);
            var data = lichsu;
            PushResponse response = client.Push("lichsu/", data);
            data.id = response.Result.name;
            SetResponse setResponse = client.Set("lichsu/" + data.id, data);
        }
        public ActionResult Loc(FormCollection form)
        {
            client = new FireSharp.FirebaseClient(config);
            FirebaseResponse response = client.Get("chuyenxe/");
            dynamic data = JsonConvert.DeserializeObject<dynamic>(response.Body);
            var list = new List<Chuyenxe>();
            foreach (var item in data)
            {
                list.Add(JsonConvert.DeserializeObject<Chuyenxe>(((JProperty)item).Value.ToString()));
            }
            DateTime ngaydi;
            string start = form["start"];
            string end = form["end"];
            DateTime targetDate;

            if (DateTime.TryParse(form["Ngaydi"], out ngaydi))
            {
                try
                {
                    targetDate = ngaydi.Date;
                }
                catch (ArgumentOutOfRangeException)
                {
                    // Xử lý khi ngày/tháng/năm không hợp lệ
                    // Ví dụ: trả về một thông báo lỗi hoặc xử lý khác
                    return View(list);
                }
            }
            else
            {
                // Xử lý khi Ngaydi không hợp lệ (không phải kiểu DateTime)
                return View(list);
            }
            list = list.Where(item => item.ngaydi.Date == targetDate && item.start == start && item.end==end).ToList();

            return View(list);
        }
    }
}