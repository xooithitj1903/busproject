using busproject.Models;
using FireSharp.Interfaces;
using FireSharp.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FireSharp.Config;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.IO;
using System.Data.Entity;
using Firebase.Auth;
using System.Net;
using System.Net.Mail;

namespace busproject.Controllers
{
    public class TaiKhoanController : Controller
    {
        // GET: TaiKhoan
        IFirebaseConfig config = new FireSharp.Config.FirebaseConfig
        {
            AuthSecret = "HIIV929CfeQ8GGTRMig0ZMaL65iep8B8ftmoAt9R",
            BasePath = "https://busproject-5ba81-default-rtdb.firebaseio.com/"
        };
        IFirebaseClient client;
        // GET: DangnhapDangky
        [HttpGet]
        public ActionResult Dangky()
        {
            return View();
        }
        public ActionResult TiemKiem()
        {
            return View();
        }
        public ActionResult Tracuu(FormCollection form)
        {
            string id = null;
            client = new FireSharp.FirebaseClient(config);
            FirebaseResponse response = client.Get("TaiKhoan/");
            dynamic data = JsonConvert.DeserializeObject<dynamic>(response.Body);
            var list = new List<TaiKhoan>();
            foreach (var item in data)
            {
                list.Add(JsonConvert.DeserializeObject<TaiKhoan>(((JProperty)item).Value.ToString()));
            }
            foreach(var item in list)
            {
                if(item.phone == form["phone"])
                {
                    id = item.Id;
                }
            }
            if(id == null)
            {
                return View("TiemKiem");
            }
            client = new FireSharp.FirebaseClient(config);
            FirebaseResponse response2 = client.Get("lichsu/");
            dynamic data2 = JsonConvert.DeserializeObject<dynamic>(response2.Body);
            var list2 = new List<LichSu>();
            var list3 = new List<LichSu>();
            foreach (var item in data2)
            {
                list2.Add(JsonConvert.DeserializeObject<LichSu>(((JProperty)item).Value.ToString()));
            }
            foreach(var item in list2)
            {
                if(item.idkh == id)
                {
                    list3.Add(item);
                }
            }
            return View(list3);
        }
        [HttpPost]
        public ActionResult Dangky(TaiKhoan taikhoan)
        {
            try
            {
                AddStudentToFirebase(taikhoan);
                return RedirectToAction("Dangnhap");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
            }

            return View();
        }
        public ActionResult Dangxuat()
        {
            Session.Clear();
            return RedirectToAction("DangNhap");
        }
        public ActionResult DangNhap()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Dangnhap(TaiKhoan taiKhoan)
        {
            client = new FireSharp.FirebaseClient(config);
            FirebaseResponse response = client.Get("TaiKhoan/");
            dynamic data = JsonConvert.DeserializeObject<dynamic>(response.Body);
            var list = new List<TaiKhoan>();
            foreach (var item in data)
            {
                list.Add(JsonConvert.DeserializeObject<TaiKhoan>(((JProperty)item).Value.ToString()));
            }
            foreach (var items in list)
            {
                if ("admin" == taiKhoan.Name && "12345678" == taiKhoan.pass)
                {
                    Session["PasswordUser"] = taiKhoan.pass;
                    Session["Email"] = "admin";
                    return RedirectToAction("Index", "Chuyenxe");
                }
                if (items.Name == taiKhoan.Name && items.pass == taiKhoan.pass)
                {
                    Session["ID"] = items.Id;
                    return RedirectToAction("Index","Chuyenxe");
                }
            }
            return View();
        }
        private void AddStudentToFirebase(TaiKhoan taikhoan)
        {
            client = new FireSharp.FirebaseClient(config);
            var data = taikhoan;
            PushResponse response = client.Push("TaiKhoan/", data);
            data.Id = response.Result.name;
            SetResponse setResponse = client.Set("TaiKhoan/" + data.Id, data);
        }
    }
}