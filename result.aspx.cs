using LAPTRINHWEB.ThanhToan_Visa;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace LAPTRINHWEB
{
    public partial class result : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request.QueryString["data"] != null)
            {
                var request_data = Request.QueryString["data"];

                var data_base64 = Alepay.Base64Decode(request_data);

                var decData = Crypter.Decrypt<ResponseCallback>(data_base64);

                var result_trans = Alepay.getTransactionInfo(decData.data);
                showResult.InnerText = result_trans;
            }
        }
    }
}