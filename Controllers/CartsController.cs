using LAPTRINHWEB.Models;
using LAPTRINHWEB.ThanhToan;
using LAPTRINHWEB.ThanhToan_Visa;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;
using System.Web.UI.WebControls;



namespace LAPTRINHWEB.Controllers
{
    public class CartsController : Controller
    {
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
            if (sanpham != null)
            {
                listGiohang.RemoveAll(n => n.Masp == Masp);
                return RedirectToAction("GioHang");
            }
            if (listGiohang.Count == 0)
            {
                return RedirectToAction("Index", "Home");
            }
            return RedirectToAction("GioHang");
        }
        public ActionResult Capnhatgiohang(int Masp, FormCollection f)
        {
            List<GioHang> listGiohang = Laygiohang();
            GioHang sanpham = listGiohang.SingleOrDefault(n => n.Masp == Masp);
            if (sanpham != null)
            {
                sanpham.Soluong = int.Parse(f["txtSoluong"].ToString());
            }
            return RedirectToAction("GioHang");
        }
        public ActionResult KetQua(FormCollection f)
        {
            try
            {
                Payment.ThanhToan(Tongtien());
                ViewBag.Message = "Thành công!";
                return View();
            }
            catch
            {
                ViewBag.Message = "Thất bại!";
                return View();
            }
        }

        public ActionResult HinhThuc()
        {
            return View();     
        }

        public ActionResult Visa(FormCollection f)
        {
            return View();
        }

        public string SendOrdertToAlepay()
        {
            RequestData rq = new RequestData();
            rq.amount = Request.Form["amount"];
            rq.currency = Request.Form["currency"];
            rq.orderDescription = Request.Form["orderDescription"];
            rq.totalItem = Request.Form["totalItem"];
            rq.buyerName = Request.Form["buyerName"];
            rq.buyerEmail = Request.Form["buyerEmail"];
            rq.buyerPhone = Request.Form["buyerPhone"];
            rq.buyerAddress = Request.Form["buyerAddress"];
            rq.buyerCity = Request.Form["buyerCity"];
            rq.buyerCountry = Request.Form["buyerCountry"];

            rq.orderCode = DateTime.Now.ToString("Mdyyyy");
            rq.checkoutType = "1"; // Thanh toán
            rq.allowDomestic = false;// Thanh toán bằng thẻ nội địa
            rq.paymentHours = "48";
            rq.installment = false;
            string baseUrl = Request.Url.GetLeftPart(UriPartial.Authority);
            rq.returnUrl = baseUrl + "/Carts/KetQua";
            rq.cancelUrl = baseUrl + "/Carts/Visa";
            var result = Alepay.sendOrderToAlepay(rq);
            return result;

        }
        public class RequestData
        {
            public string amount { get; set; }
            public string currency { get; set; }
            public string orderDescription { get; set; }
            public string totalItem { get; set; }
            public string buyerName { get; set; }
            public string buyerEmail { get; set; }
            public string buyerPhone { get; set; }
            public string buyerAddress { get; set; }
            public string buyerCity { get; set; }
            public string buyerCountry { get; set; }
            public string orderCode { get; internal set; }
            public string checkoutType { get; internal set; }
            public bool installment { get; internal set; }
            public string paymentHours { get; internal set; }
            public bool allowDomestic { get; internal set; }
            public string returnUrl { get; internal set; }
            public string cancelUrl { get; internal set; }
        }
    }
}