using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;
using System.Net;
using System.IO;
using System.Text;
using System.Security.Cryptography;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace LAPTRINHWEB.ThanhToan_Visa
{
    public class Alepay
    {
        public static string sendOrderToAlepay(object obj)
        {
            var url = Config.apiURL + "/checkout/v1/request-order";
            var result_json = sendRequestToAlepay(obj, url);

            // Convert json về object
            JavaScriptSerializer jss = new JavaScriptSerializer();
            ResponseData response_data = jss.Deserialize<ResponseData>(result_json);

            if (response_data.errorCode == "000")
            {
                //giải mã data trong khối (errorCode,data,checksum,errorDescription) về Object ResponseCheckout (token,checkoutUrl)
                var response_checkout = Crypter.Decrypt<ResponseCheckout>(response_data.data);

                // convert dữ liệu về json
                // {"token":"ALE1501841496836","checkoutUrl":"http://test.alepay.vn/checkout/v1/ALE1501841496836/vi"}
                var settings = new JsonSerializerSettings()
                {
                    ContractResolver = new CamelCasePropertyNamesContractResolver(),
                    Converters = new List<JsonConverter> { new TickDateTimeConverter() }
                };
                return JsonConvert.SerializeObject(response_checkout, settings);


            }
            else
                return result_json;
        }

        public static string sendRequestToAlepay(object obj, string url)
        {
            // mã hóa dữ liệu bằng RSA
            var encData = Crypter.Encrypt(obj);

            // tạo checksum
            var checksumData = MD5PHP(encData + Config.checksumKey);

            RequestData requestData = new RequestData();
            requestData.token = Config.apiKey;
            requestData.data = encData;
            requestData.checksum = checksumData;

            // convert dữ liệu về json
            // {"token":"...","data":"Zt...qB4=","checksum":"..."}
            var settings = new JsonSerializerSettings()
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver(),
                Converters = new List<JsonConverter> { new TickDateTimeConverter() }
            };
            var dataJson = JsonConvert.SerializeObject(requestData, settings);

            // post dữ liệu sang api
            var result_json = Post_Alepay(dataJson, url);

            // dữ liệu trả về dạng json
            // {"errorCode":"000","data":"Zt...qB4=","checksum":"d27bff9efbfc201fcabb1d49102e4896","errorDescription":"Thành công"}
            return result_json;
        }

        public static string MD5PHP(string input)
        {
            byte[] asciiBytes = ASCIIEncoding.ASCII.GetBytes(input);
            byte[] hashedBytes = MD5CryptoServiceProvider.Create().ComputeHash(asciiBytes);
            string hashedString = BitConverter.ToString(hashedBytes).Replace("-", "").ToLower();
            return hashedString;
        }

        public static string Post_Alepay(string myParameters, string URI)
        {
            using (WebClient wc = new WebClient())
            {
                wc.Headers[HttpRequestHeader.ContentType] = "application/json;charset=UTF-8";
                try
                {
                    string HtmlResult = wc.UploadString(URI, "Post", myParameters);
                    return HtmlResult;
                }
                catch (WebException ex)
                {
                    var statusCode = (int)((HttpWebResponse)ex.Response).StatusCode;
                    var des = ((HttpWebResponse)ex.Response).StatusDescription;
                    var resp = new StreamReader(ex.Response.GetResponseStream()).ReadToEnd();
                    return resp;
                }
            }
        }

        public static string Base64Decode(string base64EncodedData)
        {
            var base64EncodedBytes = System.Convert.FromBase64String(base64EncodedData);
            return System.Text.Encoding.UTF8.GetString(base64EncodedBytes);
        }

        public static string getTransactionInfo(string transaction_code)
        {
            var url = Config.apiURL + "/checkout/v1/get-transaction-info";

            var objRequest = new
            {
                transactionCode = transaction_code,
            };

            ResponseCheckCheckout response_check_checkout = new ResponseCheckCheckout();

            var result_json = sendRequestToAlepay(objRequest, url);

            JavaScriptSerializer jss = new JavaScriptSerializer();
            ResponseData response_data = jss.Deserialize<ResponseData>(result_json);

            if (response_data.errorCode == "000")
            {
                var response_transaction = Crypter.Decrypt<ResponseTransaction>(response_data.data);

                var settings = new JsonSerializerSettings()
                {
                    ContractResolver = new CamelCasePropertyNamesContractResolver(),
                    Converters = new List<JsonConverter> { new TickDateTimeConverter() }
                };
                return JsonConvert.SerializeObject(response_transaction, settings);
            }
            else
                return result_json;
        }
    }

    public class RequestData
    {
        public string token { get; set; }
        public string data { get; set; }
        public string checksum { get; set; }
    }
    public class ResponseData
    {
        public string errorCode { get; set; }
        public string data { get; set; }
        public string checksum { get; set; }
        public string errorDescription { get; set; }
    }
    public class ResponseCheckout
    {
        public string token { get; set; }
        public string checkoutUrl { get; set; }
    }
    public class ResponseCheckCheckout
    {
        public string status { get; set; }
        public string tokenreturn { get; set; }
        public string datareturn { get; set; }
    }
    public class ResponseCallback
    {
        public string errorCode { get; set; }
        public string data { get; set; }
        public string cancel { get; set; }
    }

    public class ResponseTransaction
    {
        public string transactionCode { get; set; }
        public string orderCode { get; set; }
        public string amount { get; set; }
        public string currency { get; set; }
        public string buyerEmail { get; set; }
        public string buyerPhone { get; set; }
        public string cardNumber { get; set; }
        public string buyerName { get; set; }
        public string status { get; set; }
        public string message { get; set; }
        public string installment { get; set; }
        public string is3D { get; set; }
        public string month { get; set; }
        public string bankCode { get; set; }
        public string bankName { get; set; }
        public string method { get; set; }
        public string transactionTime { get; set; }
        public string successTime { get; set; }
        public string bankHotline { get; set; }
    }
}