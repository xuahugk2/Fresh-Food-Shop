using LAPTRINHWEB.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LAPTRINHWEB.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult GioiThieu()
        {          

            return View();
        }

        public ActionResult LienHe()
        {
            

            return View();
        }
        public ActionResult dangky()
        {


            return View();
        }
        public ActionResult dangnhap()
        {


            return View();
        }
        public ActionResult chinhsachchung()
        {


            return View();
        }
        public ActionResult chinhsachvanchuyen()
        {


            return View();
        }
        public ActionResult chinhsachdoitra()
        {


            return View();
        }
        public ActionResult chinhsachbaomat()
        {


            return View();
        }
        private FreshFoodEntities db = new FreshFoodEntities();
        //lay gio hang
        public List<GioHang> Laygiohang()
        {
            List<GioHang> listGiohang = Session["Giohang"] as List<GioHang>;
            if (listGiohang == null)
            {
                listGiohang = new List<GioHang>();
                Session["Giohang"] = listGiohang;
            }
            return listGiohang;
        }
        //them gio hang
        public ActionResult Themgiohang(int Masp, string strURL)
        {
            List<GioHang> listGiohang = Laygiohang();
            GioHang sanpham = listGiohang.Find(n => n.Masp == Masp);
            if (sanpham == null)
            {
                sanpham = new GioHang(Masp);
                listGiohang.Add(sanpham);
                return Redirect(strURL);
            }
            else
            {
                sanpham.Soluong++;
                return Redirect(strURL);
            }
        }
        private int Tongsoluong()
        {
            int tongsl = 0;
            List<GioHang> listGiohang = Session["Giohang"] as List<GioHang>;
            if (listGiohang != null)
            {
                tongsl = listGiohang.Sum(n => n.Soluong);
            }
            return tongsl;
        }
        private double Tongtien()
        {
            double tongtien = 0;
            List<GioHang> listGiohang = Session["Giohang"] as List<GioHang>;
            if (listGiohang != null)
            {
                tongtien = listGiohang.Sum(n => n.Thanhtien);
            }
            return tongtien;
        }
        public ActionResult GioHang()
        {
            List<GioHang> listGiohang = Laygiohang();
            if (listGiohang.Count == 0)
            {
                return RedirectToAction("Index", "Home");
            }
            ViewBag.Tongsoluong = Tongsoluong();
            ViewBag.Tongtien = Tongtien();
            return View(listGiohang);
        }
        public ActionResult GioHangPartial()
        {
            ViewBag.Tongsoluong = Tongsoluong();
            ViewBag.Tongtien = Tongtien();
            return PartialView();
        }
        public ActionResult Xoadonhang(int Masp)
        {
            List<GioHang> listGiohang = Laygiohang();
            GioHang sanpham = listGiohang.SingleOrDefault(n => n.Masp == Masp);
            if(sanpham != null)
            {
                listGiohang.RemoveAll(n => n.Masp == Masp);
                return RedirectToAction("GioHang");
            }
            if(listGiohang.Count == 0)
            {
                return RedirectToAction("Index", "Home");
            }
            return RedirectToAction("GioHang");
        }
    }
}