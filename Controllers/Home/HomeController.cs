using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using QLKS.Models;
using System.Diagnostics;
using System.Globalization;
using System.IO;

namespace QLKS.Controllers
{
    public class HomeController : Controller
    {
        private dataQLKSEntities db = new dataQLKSEntities();
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
        [HttpGet]
        public ActionResult FindRoom()
        {
            return RedirectToAction("Index", "Home");
        }
        [HttpPost]
        public ActionResult FindRoom(String datestart, String dateend)
        {
            List<tblPhong> li = new List<tblPhong>();
            if (datestart.Equals("") || dateend.Equals(""))
            {
                li = db.tblPhongs.ToList();
            }
            else
            {
                Session["ngay_vao"] = datestart;
                Session["ngay_ra"] = dateend;

                datestart = DateTime.ParseExact(datestart, "dd/MM/yyyy", CultureInfo.InvariantCulture).ToString("yyyy/MM/dd");
                dateend = DateTime.ParseExact(dateend, "dd/MM/yyyy", CultureInfo.InvariantCulture).ToString("yyyy/MM/dd");

                ViewBag.dateS = datestart.ToString();
                ViewBag.dateE = dateend.ToString();

                String query = "select * from tblPhong a where a.ma_phong not in(select a.ma_phong from tblPhieuDatPhong as a right join tblPhong as b on a.ma_phong = b.ma_phong where a.ngay_ra > '" + datestart + "' and a.ngay_vao < '" + dateend + "')";
                li = db.tblPhongs.SqlQuery(query).ToList();
            }
            return View(li);
        }
        public ActionResult ChonPhong(string id)
        {
            Session["ma_phong"] = id;
            return RedirectToAction("BookRoom", "Home");
        }
        public ActionResult BookRoom()
        {
            if (Session["KH"] == null)
            {
                return RedirectToAction("Login", "KhachHang");
            }
            tblKhachHang kh = (tblKhachHang)Session["KH"];
            ViewBag.ma_kh = kh.ma_kh;
            ViewBag.ten_kh = kh.ho_ten;
            ViewBag.ngay_dat = DateTime.Now;
            ViewBag.ngay_vao = (String)Session["ngay_vao"];
            ViewBag.ngay_ra = (String)Session["ngay_ra"];

            if (Session["ma_phong"] != null)
            {
                ViewBag.ma_phong = (String)Session["ma_phong"];
                int map = Int32.Parse((String)Session["ma_phong"]);
                tblPhong p = (tblPhong)db.tblPhongs.Find(map);
                ViewBag.so_phong = p.so_phong;
            }
            var liP = db.tblPhieuDatPhongs.Where(u => u.ma_kh == kh.ma_kh && u.ma_tinh_trang == 1).ToList();
            return View(liP);
        }
        public ActionResult Result(String ma_kh, String ngay_vao, String ngay_ra, String ma_phong)
        {
            if (ma_kh == null || ngay_vao == null || ngay_ra == null || ma_phong == null)
            {
                return RedirectToAction("Index", "Home");
            }
            else
            {
                tblPhieuDatPhong tgd = new tblPhieuDatPhong();
                tgd.ma_kh = ma_kh;
                tgd.ma_phong = Int32.Parse(ma_phong);
                tgd.ma_tinh_trang = 1;
                tgd.ngay_dat = DateTime.Now;
                tgd.ngay_vao = DateTime.ParseExact(ngay_vao, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                tgd.ngay_ra = DateTime.ParseExact(ngay_ra, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                try
                {
                    db.tblPhieuDatPhongs.Add(tgd);
                    db.SaveChanges();
                    ViewBag.Result = "success";
                    setNull();
                }
                catch
                {
                    ViewBag.Result = "error";
                }
            }
            return View();
        }

        public ActionResult HuyPhieuDatPhong()
        {
            setNull();
            return RedirectToAction("BookRoom", "Home");
        }
        private void setNull()
        {
            Session["ngay_vao"] = null;
            Session["ngay_ra"] = null;
            Session["ma_phong"] = null;
        }
        public ActionResult Chat()
        {
            return View();
        }
        public ActionResult Upload()
        {
            return View();
        }
        public ActionResult Slider(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tblPhong p = db.tblPhongs.Include(a => a.tblLoaiPhong).Where(a=>a.ma_phong==id).First();
            return View(p);
        }
    }
}