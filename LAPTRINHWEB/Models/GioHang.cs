using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using LAPTRINHWEB.Models;

namespace LAPTRINHWEB.Models
{
    
    public class GioHang
    {
        public int Masp { get; set; }
        public string Tensp { get; set; }
        public string Hinhanh { get; set; }
        public Double Dongia { get; set; }
        public int Soluong { get; set; }
        public Double Thanhtien
        {
            get { return Soluong * Dongia; }
        }
        private FreshFoodEntities db = new FreshFoodEntities();
        public GioHang(int masp)
        {
            Masp = masp;
            Products product = db.Products.Single(n => n.Id == Masp);
            Tensp = product.Name;
            Hinhanh = product.Img;
            Dongia = double.Parse(product.PriceBuy.ToString());
            Soluong = 1;
        }
    }
}