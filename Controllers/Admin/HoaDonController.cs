using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using QLKS.Models;

namespace QLKS.Controllers.Admin
{
    public class HoaDonController : Controller
    {
        private dataQLKSEntities db = new dataQLKSEntities();

        // GET: HoaDon
        public ActionResult Index()
        {
            var tblHoaDons = db.tblHoaDons.Include(t => t.tblNhanVien).Include(t => t.tblPhieuDatPhong).Include(t => t.tblTinhTrangHoaDon);
            double tong = 0;
            foreach (var item in tblHoaDons.ToList())
            {
                if (item.ma_tinh_trang == 2)
                {
                    tong += (double)item.tong_tien;
                }
            }
            ViewBag.tong_tien = tong.ToString("C");
            return View(tblHoaDons.ToList());
        }

        [HttpPost]
        public ActionResult Index(String beginDate, String endDate)
        {
            System.Diagnostics.Debug.WriteLine("your message here " + beginDate);
            List<tblHoaDon> dshd = new List<tblHoaDon>();
            String query = "select * from tblHoaDon where 1 = 1";
            if (!beginDate.Equals(""))
                query += " and ngay_tra_phong >= '" + beginDate + "'";
            if (!endDate.Equals(""))
                query += " and ngay_tra_phong <= '" + endDate + "'";
            System.Diagnostics.Debug.WriteLine(query);
            dshd = db.tblHoaDons.SqlQuery(query).ToList();
            double tong = 0;
            foreach (var item in dshd)
            {
                if (item.ma_tinh_trang == 2)
                {
                    tong += (double)item.tong_tien;
                }
            }
            ViewBag.tong_tien = tong.ToString("C");
            return View(dshd);
        }

        // GET: HoaDon/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tblHoaDon tblHoaDon = db.tblHoaDons.Find(id);
            if (tblHoaDon == null)
            {
                return HttpNotFound();
            }
            return View(tblHoaDon);
        }

        // GET: HoaDon/Create
        public ActionResult Create()
        {
            ViewBag.ma_nv = new SelectList(db.tblNhanViens, "ma_nv", "ho_ten");
            ViewBag.ma_tgd = new SelectList(db.tblPhieuDatPhongs, "ma_tgd", "ma_kh");
            ViewBag.ma_tinh_trang = new SelectList(db.tblTinhTrangHoaDons, "ma_tinh_trang", "mo_ta");
            return View();
        }

        // POST: HoaDon/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ma_hd,ma_tgd,ngay_tra_phong,ma_tinh_trang")] tblHoaDon tblHoaDon)
        {
            if (ModelState.IsValid)
            {
                db.tblHoaDons.Add(tblHoaDon);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.ma_nv = new SelectList(db.tblNhanViens, "ma_nv", "ho_ten", tblHoaDon.ma_nv);
            ViewBag.ma_tgd = new SelectList(db.tblPhieuDatPhongs, "ma_tgd", "ma_kh", tblHoaDon.ma_tgd);
            ViewBag.ma_tinh_trang = new SelectList(db.tblTinhTrangHoaDons, "ma_tinh_trang", "mo_ta", tblHoaDon.ma_tinh_trang);
            return View(tblHoaDon);
        }
        public ActionResult Add(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tblPhieuDatPhong tblPhieuDatPhong = db.tblPhieuDatPhongs.Find(id);
            if (tblPhieuDatPhong == null)
            {
                return HttpNotFound();
            }
            return View(tblPhieuDatPhong);
        }
        // GET: HoaDon/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tblHoaDon tblHoaDon = db.tblHoaDons.Find(id);
            if (tblHoaDon == null)
            {
                return HttpNotFound();
            }
            ViewBag.ma_nv = new SelectList(db.tblNhanViens, "ma_nv", "ho_ten", tblHoaDon.ma_nv);
            ViewBag.ma_tgd = new SelectList(db.tblPhieuDatPhongs, "ma_tgd", "ma_kh", tblHoaDon.ma_tgd);
            ViewBag.ma_tinh_trang = new SelectList(db.tblTinhTrangHoaDons, "ma_tinh_trang", "mo_ta", tblHoaDon.ma_tinh_trang);
            return View(tblHoaDon);
        }

        // POST: HoaDon/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ma_hd,ma_nv,ma_tgd,ngay_tra_phong,ma_tinh_trang,tien_phong,tien_dich_vu,phu_thu,tong_tien")] tblHoaDon tblHoaDon)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tblHoaDon).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.ma_nv = new SelectList(db.tblNhanViens, "ma_nv", "ho_ten", tblHoaDon.ma_nv);
            ViewBag.ma_tgd = new SelectList(db.tblPhieuDatPhongs, "ma_tgd", "ma_kh", tblHoaDon.ma_tgd);
            ViewBag.ma_tinh_trang = new SelectList(db.tblTinhTrangHoaDons, "ma_tinh_trang", "mo_ta", tblHoaDon.ma_tinh_trang);
            return View(tblHoaDon);
        }

        // GET: HoaDon/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tblHoaDon tblHoaDon = db.tblHoaDons.Find(id);
            if (tblHoaDon == null)
            {
                return HttpNotFound();
            }
            return View(tblHoaDon);
        }

        // POST: HoaDon/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            tblHoaDon tblHoaDon = db.tblHoaDons.Find(id);
            db.tblHoaDons.Remove(tblHoaDon);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
        public ActionResult Result(String ma_tgd)
        {
            if (ma_tgd == null)
            {
                return RedirectToAction("Index", "Admin");
            }
            else
            {
                tblHoaDon hd = new tblHoaDon();
                hd.ma_tgd = Int32.Parse(ma_tgd);
                hd.ma_tinh_trang = 1;
                try
                {
                    db.tblHoaDons.Add(hd);
                    tblPhieuDatPhong tgd = db.tblPhieuDatPhongs.Find(Int32.Parse(ma_tgd));
                    if (tgd == null)
                    {
                        return HttpNotFound();
                    }
                    tblPhong p = db.tblPhongs.Find(tgd.ma_phong);
                    if (p == null)
                    {
                        return HttpNotFound();
                    }
                    tgd.ma_tinh_trang = 2;
                    db.Entry(tgd).State = EntityState.Modified;
                    p.ma_tinh_trang = 2;
                    db.Entry(p).State = EntityState.Modified;
                    ViewBag.ngay_ra = tgd.ngay_ra;
                    db.SaveChanges();
                    ViewBag.Result = "success";
                }
                catch
                {
                    ViewBag.Result = "error";
                }
            }
            return View();
        }
        public ActionResult ThanhToan(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tblHoaDon tblHoaDon = db.tblHoaDons.Find(id);
            if (tblHoaDon == null)
            {
                return HttpNotFound();
            }
            var so_ngay = (tblHoaDon.tblPhieuDatPhong.ngay_ra - tblHoaDon.tblPhieuDatPhong.ngay_vao).Value.Days;
            //if (so_ngay < 1)
            //    so_ngay = 1;
            var tien_phong = so_ngay * tblHoaDon.tblPhieuDatPhong.tblPhong.tblLoaiPhong.gia;
            ViewBag.tien_phong = tien_phong;
            ViewBag.so_ngay = so_ngay;
            ViewBag.time_now = DateTime.Now.ToString();
            tblNhanVien nv = (tblNhanVien)Session["NV"];
            if(nv!= null)
            {
                ViewBag.ho_ten = nv.ho_ten;
            }
            List<tblDichVuDaDat> dsdv = db.tblDichVuDaDats.Where(u => u.ma_hd == id).ToList();
            ViewBag.list_dv = dsdv;
            double tongtiendv = 0;
            List<double> tt = new List<double>();
            foreach (var item in dsdv)
            {
                double t = (double)(item.so_luong * item.tblDichVu.gia);
                tongtiendv += t;
                tt.Add(t);
            }
            ViewBag.list_tt = tt;
            ViewBag.tien_dich_vu = tongtiendv;
            ViewBag.tong_tien = tien_phong + tongtiendv;
            return View(tblHoaDon);
        }
        public ActionResult GoiDichVu(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            GoiDichVuModel mod = new GoiDichVuModel();
            mod.dsDichVu = db.tblDichVus.ToList();
            mod.dsDvDaDat = db.tblDichVuDaDats.Where(u => u.ma_hd == id).ToList();
            ViewBag.ma_hd = id;
            ViewBag.so_phong = db.tblHoaDons.Find(id).tblPhieuDatPhong.tblPhong.so_phong;
            return View(mod);
        }
        public ActionResult XacNhanGoiDichVu(String ma_hd, String ma_dv, String so_luong)
        {
            if (ma_hd == null || ma_dv == null || so_luong == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tblDichVuDaDat dsdv = new tblDichVuDaDat();
            
            try
            {
                dsdv.ma_hd = Int32.Parse(ma_hd);
                dsdv.ma_dv = Int32.Parse(ma_dv);
                dsdv.so_luong = Int32.Parse(so_luong);
                db.tblDichVuDaDats.Add(dsdv);
                db.SaveChanges();
            }
            catch
            {

            }
            return RedirectToAction("GoiDichVu", "HoaDon", new { id = ma_hd });
        }
        public ActionResult SuaDichVu(String ma_hd, String edit_id, String edit_so_luong)
        {
            if (ma_hd == null || edit_id == null || edit_so_luong == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tblDichVuDaDat dsdv = db.tblDichVuDaDats.Find(Int32.Parse(edit_id));
            dsdv.so_luong = Int32.Parse(edit_so_luong);
            db.Entry(dsdv).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("GoiDichVu", "HoaDon", new { id = ma_hd });
        }
        public ActionResult XoaDichVu(String ma_hd, String del_id)
        {
            tblDichVuDaDat d = db.tblDichVuDaDats.Find(Int32.Parse(del_id));
            db.tblDichVuDaDats.Remove(d);
            db.SaveChanges();
            return RedirectToAction("GoiDichVu", "HoaDon", new { id = ma_hd });
        }
        public ActionResult XacNhanThanhToan(String ma_hd, String tien_phong, String tien_dich_vu, String tong_tien)
        {
            if (ma_hd == null || tien_phong == null || tien_dich_vu == null || tong_tien == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            try
            {
                tblHoaDon hd = db.tblHoaDons.Find(Int32.Parse(ma_hd));
                tblNhanVien nv = (tblNhanVien)Session["NV"];
                if(nv!=null)
                    hd.ma_nv = nv.ma_nv;
                hd.tien_phong = Double.Parse(tien_phong);
                hd.tien_dich_vu = Double.Parse(tien_dich_vu);
                hd.phu_thu = 0;
                hd.tong_tien = Double.Parse(tong_tien);
                hd.ma_tinh_trang = 2;
                hd.ngay_tra_phong = DateTime.Now;
                db.Entry(hd).State = EntityState.Modified;

                tblPhong p = db.tblPhongs.Find(hd.tblPhieuDatPhong.ma_phong);
                p.ma_tinh_trang = 3;
                db.Entry(p).State = EntityState.Modified;

                db.SaveChanges();

                ViewBag.result = "success";
            }
            catch (Exception e)
            {
                ViewBag.result = "error";
            }
            return View();
        }
        public ActionResult ChiTietHoaDon(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tblHoaDon tblHoaDon = db.tblHoaDons.Find(id);
            if (tblHoaDon == null)
            {
                return HttpNotFound();
            }

            var tien_phong = (tblHoaDon.tblPhieuDatPhong.ngay_ra - tblHoaDon.tblPhieuDatPhong.ngay_vao).Value.TotalDays * tblHoaDon.tblPhieuDatPhong.tblPhong.tblLoaiPhong.gia;
            ViewBag.tien_phong = tien_phong;

            ViewBag.time_now = DateTime.Now.ToString();

            List<tblDichVuDaDat> dsdv = db.tblDichVuDaDats.Where(u => u.ma_hd == id).ToList();
            ViewBag.list_dv = dsdv;
            double tongtiendv = 0;
            List<double> tt = new List<double>();
            foreach (var item in dsdv)
            {
                double t = (double)(item.so_luong * item.tblDichVu.gia);
                tongtiendv += t;
                tt.Add(t);
            }
            ViewBag.list_tt = tt;
            ViewBag.tien_dich_vu = tongtiendv;
            ViewBag.tong_tien = tien_phong + tongtiendv;
            return View(tblHoaDon);
        }
    }
}
