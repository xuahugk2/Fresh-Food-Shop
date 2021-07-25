using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;
using System.Text;
using System.Net;
using Newtonsoft.Json.Linq;

namespace LAPTRINHWEB.ThanhToan
{
    public class Payment
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public static bool ThanhToan(double tongtien)
        {
            try
            {
                string endpoint = "https://test-payment.momo.vn/gw_payment/transactionProcessor";
                string partnerCode = "MOMO5RGX20191128";
                string accessKey = "M8brj9K6E22vXoDB";
                string serectkey = "nqQiVSgDMy809JoPF6OzP5OdBUB550Y4";
                string orderInfo = "thanh toan hoa don";
                string returnUrl = "https://momo.vn/return";
                string notifyurl = "https://momo.vn/notify";

                string amount = tongtien.ToString();
                string orderid = Guid.NewGuid().ToString();
                string requestId = Guid.NewGuid().ToString();
                string extraData = "";

                //Before sign HMAC SHA256 signature
                string rawHash = "partnerCode=" +
                    partnerCode + "&accessKey=" +
                    accessKey + "&requestId=" +
                    requestId + "&amount=" +
                    amount + "&orderId=" +
                    orderid + "&orderInfo=" +
                    orderInfo + "&returnUrl=" +
                    returnUrl + "&notifyUrl=" +
                    notifyurl + "&extraData=" +
                    extraData;

                log.Debug("rawHash = " + rawHash);

                MoMoSecurity crypto = new MoMoSecurity();
                //sign signature SHA256
                string signature = crypto.signSHA256(rawHash, serectkey);
                log.Debug("Signature = " + signature);

                //build body json request
                JObject message = new JObject{

                    { "partnerCode", partnerCode },
                    { "accessKey", accessKey },
                    { "requestId", requestId },
                    { "amount", amount },
                    { "orderId", orderid },
                    { "orderInfo", orderInfo },
                    { "returnUrl", returnUrl },
                    { "notifyUrl", notifyurl },
                    { "extraData", extraData },
                    { "requestType", "captureMoMoWallet" },
                    { "signature", signature }

                };

                log.Debug("Json request to MoMo: " + message.ToString());
                string responseFromMomo = PaymentRequest.sendPaymentRequest(endpoint, message.ToString());

                JObject jmessage = JObject.Parse(responseFromMomo);
                log.Debug("Return from MoMo: " + jmessage.ToString());
                System.Diagnostics.Process.Start(jmessage.GetValue("payUrl").ToString());
                return true;
            }
            catch
            {
                return false;
            }
            
        }
    }
}