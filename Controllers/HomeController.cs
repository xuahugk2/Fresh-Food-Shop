using LAPTRINHWEB.Models;
using LAPTRINHWEB.ThanhToan;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LAPTRINHWEB.Controllers
{
    public class HomeController : Controller
    {
        private FreshFoodEntities db = new FreshFoodEntities();
        public ActionResult Index()
        {
            List<ProductSales> list = db.ProductSales.ToList();
            return View(list);           
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
        
        public ActionResult Vieclam()
        {
            return View();
        }
        public ActionResult Camnangsudung()
        {
            return View();
        }
        public ActionResult Tintuc()
        {
            return View(db.Posts.ToList());
        }
        public ActionResult Huongdanthanhtoan()
        {
            return View();
        }
        public ActionResult Huongdanmuahang()
        {
            return View();
        }       
    }
}