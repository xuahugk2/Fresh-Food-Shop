using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LAPTRINHWEB.Models
{
    public class PayClass
    {
        public string id { get; set; }
        public string value { get; set; }
        public string src { get; set; }

        public static List<PayClass> getList()
        {
            PayClass p1 = new PayClass
            {
                id = "momo",
                value = "Momo",
                src = "momo.png"
            };
            PayClass p2 = new PayClass
            {
                id = "visa",
                value = "Visa",
                src = "visa.png"
            };

            List<PayClass> list = new List<PayClass>();
            list.Add(p1);
            list.Add(p2);
            return list;
        }
    }
}