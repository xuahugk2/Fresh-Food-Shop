using LAPTRINHWEB.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using PagedList;
using PagedList.Mvc;
using LAPTRINHWEB.ThanhToan_Visa;

namespace LAPTRINHWEB.Controllers
{
    public class ProductsController : Controller
    {
        // GET: Products
        private FreshFoodEntities db = new FreshFoodEntities();
        //POST: FIND PRODUCTS
        [HttpPost]
        public ActionResult Find(FormCollection f)
        {
            var name = f["name"];
            List<Products> list = db.Products.Where(p => p.Name.ToLower().Contains(name.ToLower())).ToList();         
            return View(list);

        }
        // GET: Products
        public ActionResult Index(int? id, int? page)
        {          
            ViewBag.Title = "DauCatMoi";
            if (id == null || id == 0)
            {
                int pageNumber = (page ?? 1);
                int pageSize = 9;
                return View(db.Products.ToList().OrderBy(n => n.Id).ToPagedList(pageNumber, pageSize));
            }
            if (id == 4)
            {
                return View(db.Products.Where(c => (c.CatId == 9 || c.CatId == 10)).ToList());
            }
            return View(db.Products.Where(c => c.CatId == id).ToList());
        }
        
        private List<Products> getList(int id)
        {
            List<Products> listPro = new List<Products>();
            List<Categorys> list = db.Categorys.Where(l => l.ParentId == id).ToList();
            if (list.Count > 0)
            {
                foreach (var item in db.Products.ToList())
                {
                    if (item.Categorys.ParentId == id)
                        listPro.Add(item);

                }
                return listPro;
            }
            listPro = db.Products.Where(p => p.CatId == id).ToList();
            return listPro;
        }
        public ActionResult Group(int id)
        {
            return View(getList(id));
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
        public ActionResult Chitiet(int? id)
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