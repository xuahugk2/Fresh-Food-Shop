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
        public ActionResult KetQua()
        {
            if (Request.QueryString["data"] != null)
            {
                var request_data = Request.QueryString["data"];

                var data_base64 = Alepay.Base64Decode(request_data);

                var decData = Crypter.Decrypt<ResponseCallback>(data_base64);

                var result_trans = Alepay.getTransactionInfo(decData.data);
                ViewBag.Message = result_trans;
            }
            return View();
        }

        public ActionResult HinhThuc()
        {
            return View();     
        }

        public ActionResult Visa()
        {
            Response.Clear();
            Response.AddHeader("Content-type", "text/json");
            var rs = this.SendOrdertToAlepay();
            Response.Write(rs);
            Response.End();

            return View("KetQua");
        }

        public string SendOrdertToAlepay()
        {
            RequestData rq = new RequestData();
            rq.amount = Tongtien().ToString();
            rq.currency = "VND";
            rq.orderDescription = "Thanh toán hóa đơn";
            rq.totalItem = Tongsoluong().ToString();
            rq.buyerName = "Nguyễn Xuân Hùng";
            rq.buyerEmail = "hung@gmail.com";
            rq.buyerPhone = "0848506079";
            rq.buyerAddress = "209 Quốc lộ 13";
            rq.buyerCity = "Thành phố Hồ Chí Minh";
            rq.buyerCountry = "Việt Nam";

            rq.orderCode = DateTime.Now.ToString("ddMMyyyy");
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