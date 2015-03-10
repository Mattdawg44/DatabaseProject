using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using MenuManager.Models;

namespace MenuManager.Controllers
{
    public class MenuController : Controller
    {
        private MenuManagerDBEntities db = new MenuManagerDBEntities();

        // GET: Menu
        public ActionResult Index(int? s, int? mi, int? i)
        {
            var menus = db.Menus.Include(m => m.Store);
            if (s != null)
                menus = menus.Where(m => (m.Store_ID == s));
            if (mi != null)
                menus = menus.Where(m => m.MenuItems.Any(menuItem => (menuItem.ID == mi)));
            if (i != null)
                menus = menus.Where(m => m.MenuItems.Any(menuItem => menuItem.Ingredients.Any(ing => (ing.ID == i))));
            return View(menus.OrderBy(m => m.Store.Address).ToList());
        }

        // GET: Menu/Create
        public ActionResult Create()
        {
            ViewBag.Store_ID = new SelectList(db.Stores, "ID", "Name");
            ViewBag.CancelUrl = Helpers.GetCancelUrl(Request, Url);
            return View();
        }

        // POST: Menu/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,Name,Store_ID")] Menu menu)
        {
            if (ModelState.IsValid)
            {
                db.Menus.Add(menu);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Store_ID = new SelectList(db.Stores, "ID", "Name", menu.Store_ID);
            ViewBag.CancelUrl = Helpers.GetCancelUrl(Request, Url);
            return View(menu);
        }

        // GET: Menu/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("Index");
            }
            Menu menu = db.Menus.Find(id);
            if (menu == null)
            {
                return HttpNotFound();
            }
            ViewBag.Store_ID = new SelectList(db.Stores, "ID", "Name", menu.Store_ID);
            ViewBag.CancelUrl = Helpers.GetCancelUrl(Request, Url);
            return View(menu);
        }

        // POST: Menu/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,Name,Store_ID")] Menu menu)
        {
            if (ModelState.IsValid)
            {
                db.Entry(menu).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Store_ID = new SelectList(db.Stores, "ID", "Name", menu.Store_ID);
            ViewBag.CancelUrl = Helpers.GetCancelUrl(Request, Url);
            return View(menu);
        }

        // GET: Menu/Details/5
        public ActionResult Details(int? id, bool isDeleting = true)
        {
            if (id == null)
            {
                return RedirectToAction("Index");
            }
            Menu menu = db.Menus.Find(id);
            if (menu == null)
            {
                return HttpNotFound();
            }
            if (isDeleting)
                ViewBag.CancelUrl = Helpers.GetCancelUrl(Request, Url);
            return View(menu);
        }

        // GET: Menu/Delete/5
        public ActionResult Delete(int? id)
        {
            return RedirectToAction("Details", new { id = id, isDeleting = true });
        }

        // POST: Menu/Details/5
        [HttpPost, ActionName("Details")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Menu menu = db.Menus.Find(id);
            db.Menus.Remove(menu);
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
