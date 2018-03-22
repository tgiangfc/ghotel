using QLKS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace QLKS.Controllers.Admin
{
    public class ThongKeController : Controller
    {
        dataQLKSEntities db = new dataQLKSEntities();
        // GET: ThongKe
        public ActionResult Index()
        {
            DateTime date = DateTime.Now;
            var firstDayOfMonth = new DateTime(date.Year, date.Month, 1);
            var lastDayOfMonth = firstDayOfMonth.AddMonths(1).AddDays(-1);

            List<tblHoaDon> dshd = new List<tblHoaDon>();
            String query = "select * from tblHoaDon where ma_tinh_trang = 2 and ngay_tra_phong >= '" + firstDayOfMonth + "' and ngay_tra_phong <= '" + lastDayOfMonth + "'";
            System.Diagnostics.Debug.WriteLine(query);
            dshd = db.tblHoaDons.SqlQuery(query).ToList();

            double tong = 0;
            foreach(var item in dshd)
            {
                tong += (double) item.tong_tien;
            }
            ViewBag.tien_ht = tong.ToString("C");

            ViewBag.so_hoa_don = db.tblHoaDons.Count();
            ViewBag.so_phieu_dat_phong = db.tblPhieuDatPhongs.Where(u=>u.ma_tinh_trang == 1).Count();
            ViewBag.so_phong_dang_dat = db.tblPhongs.Where(u => u.ma_tinh_trang == 2).Count();
            ViewBag.so_dich_vu = db.tblDichVus.Count();
            var s = db.tblDichVuDaDats.GroupBy(t => t.ma_dv).Select(t => new { ma_dv = t.Key, total = t.Sum(i=>i.so_luong) });
            ViewBag.chart_dv = ;
            //foreach (var group in s)
            //{
            //    Console.WriteLine("Membername = {0} Totalcost={1}", group.ma_dv, group.total);
            //    System.Diagnostics.Debug.WriteLine("Ma Dv: "+group.madv+" | SL: "+group.total);
            //}
            return View();
        }
    }
}