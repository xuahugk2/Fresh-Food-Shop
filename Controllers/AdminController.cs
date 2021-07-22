using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using LAPTRINHWEB.Models;
using PagedList;
using PagedList.Mvc;

namespace LAPTRINHWEB.Controllers
{
    public class AdminController : Controller
    {
        private FreshFoodEntities db = new FreshFoodEntities();
        // GET: Admin
        
        public ActionResult Index()
        {
            
            return View(Login());
        }

        public ActionResult Sanpham(int ?page)
        {
            int pageNumber = (page ?? 1);
            int pageSize = 10;
            return View(db.Products.ToList().OrderBy(n=>n.Id).ToPagedList(pageNumber,pageSize));
        }

        public ActionResult Loaisanpham(int ?page1)
        {
            int pageNumber1 = (page1 ?? 1);
            int pageSize1 = 10;
            return View(db.Categorys.ToList().OrderBy(m=>m.Id).ToPagedList(pageNumber1,pageSize1));
        }

        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(FormCollection collection)
        {
            var user = collection["username"];
            var pass = collection["password"];
            if (String.IsNullOrEmpty(user))
            {
                ViewData["loi1"] = "Phải nhập tên đăng nhập";
            }
            else if (String.IsNullOrEmpty(pass))
            {
                ViewData["loi2"] = "Phải nhập mật khẩu";
            }
            else
            {
                Users ad = db.Users.SingleOrDefault(n => n.UserName == user && n.Password == pass);
                if (ad != null)
                {
                    ViewBag.Thongbao = "Đăng nhập thành công!";
                    Session["Taikhoanadmin"] = ad;
                    return RedirectToAction("Index", "Admin");
                }
                else
                {
                    ViewBag.Thongbao = "Tên đăng nhập hoặc mật khẩu không đúng";
                }
            }
            return View();
        }
        // GET: Products/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Products product = db.Products.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }

        // GET: Products/Create
        public ActionResult Create()
        {
            ViewBag.CatId = new SelectList(db.Categorys, "Id", "Name");
            return View();
        }

        // POST: Products/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,CatId,Name,Slug,Detail,PriceBuy,Img,MetaKey,MetaDesc,Created_By,Created_At,Updated_By,Updated_At,Status")] Products product)
        {
            if (ModelState.IsValid)
            {
                db.Products.Add(product);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.CatId = new SelectList(db.Categorys, "Id", "Name", product.CatId);
            return View(product);
        }

        // GET: Products/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Products product = db.Products.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            ViewBag.CatId = new SelectList(db.Categorys, "Id", "Name", product.CatId);
            return View(product);
        }

        // POST: Products/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,CatId,Name,Slug,Detail,PriceBuy,Img,MetaKey,MetaDesc,Created_By,Created_At,Updated_By,Updated_At,Status")] Products product)
        {
            if (ModelState.IsValid)
            {
                db.Entry(product).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.CatId = new SelectList(db.Categorys, "Id", "Name", product.CatId);
            return View(product);
        }

        // GET: Products/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Products product = db.Products.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }

        // POST: Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Products product = db.Products.Find(id);
            db.Products.Remove(product);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        ///loai san pham/////////////////////////////////////////////////////////////
        // GET: Categorys/Details/5
        public ActionResult chitiet(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Categorys product = db.Categorys.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }

        // GET: Products/Create
        public ActionResult taomoi()
        {
            ViewBag.CatId = new SelectList(db.Categorys, "Id", "Name");
            return View();
        }

        // POST: Products/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult taomoi([Bind(Include = "Id,Name,Slug,ParentId,Orders,Img,MetaKey,MetaDesc,Created_By,Created_At,Updated_By,Updated_At,Status")] Categorys product)
        {
            if (ModelState.IsValid)
            {
                db.Categorys.Add(product);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.CatId = new SelectList(db.Categorys, "Id", "Name", product.ParentId);
            return View(product);
        }

        // GET: Products/Edit/5
        public ActionResult chinhsua(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Categorys product = db.Categorys.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            ViewBag.CatId = new SelectList(db.Categorys, "Id", "Name", product.ParentId);
            return View(product);
        }

        // POST: Products/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult chinhsua([Bind(Include = "Id,Name,Slug,ParentId,Orders,Img,MetaKey,MetaDesc,Created_By,Created_At,Updated_By,Updated_At,Status")] Categorys product)
        {
            if (ModelState.IsValid)
            {
                db.Entry(product).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.CatId = new SelectList(db.Categorys, "Id", "Name", product.ParentId);
            return View(product);
        }

        // GET: Products/Delete/5
        public ActionResult xoa(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Categorys product = db.Categorys.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }

        // POST: Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult xoaConfirmed(int id)
        {
            Categorys product = db.Categorys.Find(id);
            db.Categorys.Remove(product);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}