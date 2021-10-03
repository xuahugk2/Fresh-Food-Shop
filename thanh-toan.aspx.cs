using LAPTRINHWEB.ThanhToan_Visa;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace LAPTRINHWEB
{
    public partial class thanh_toan : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

            if (Request.QueryString["action"] == "sendOrderToAlepay")
            {
                Response.Clear();
                Response.AddHeader("Content-type", "text/json");
                var rs = this.SendOrdertToAlepay();
                Response.Write(rs);
                Response.End();

            }

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
            rq.returnUrl = baseUrl + "/result.aspx";
            rq.cancelUrl = baseUrl + "/thanh-toan.aspx";
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