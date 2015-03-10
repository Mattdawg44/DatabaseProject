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
    public class IngredientController : Controller
    {
        private MenuManagerDBEntities db = new MenuManagerDBEntities();

        // GET: Ingredients
        public ActionResult Index(int? s, int? m, int? mi)
        {
            var ingredients = db.Ingredients.Include(i => i.Store);
            if (s != null)
                ingredients = ingredients.Where(i => (i.Store_ID == s));
            if (m != null)
            {
                ingredients = ingredients.Where(i => i.MenuItems.Any(menuItem => (menuItem.Menu_ID == m)));
            }
            if (mi != null)
            {
                ingredients = ingredients.Where(i => i.MenuItems.Any(menuItem => menuItem.ID == mi));
            }

            ingredients = ingredients.OrderBy(i => i.Store.Name).ThenBy(i => i.Name);
            return View(ingredients.ToList());
        }

        // GET: Ingredients/Create
        public ActionResult Create()
        {
            ViewBag.Store_ID = new SelectList(db.Stores, "ID", "Name");
            ViewBag.CancelUrl = Helpers.GetCancelUrl(Request, Url);
            return View();
        }

        // POST: Ingredients/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,Name,Amount,Amount_Unit,Amount_Low,Date_Restocked,Shelf_Life,Store_ID")] Ingredient ingredient)
        {
            if (ModelState.IsValid)
            {
                db.Ingredients.Add(ingredient);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Store_ID = new SelectList(db.Stores, "ID", "Name", ingredient.Store_ID);
            ViewBag.CancelUrl = Helpers.GetCancelUrl(Request, Url);
            return View(ingredient);
        }

        // GET: Ingredients/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("Index");
            }
            Ingredient ingredient = db.Ingredients.Find(id);
            if (ingredient == null)
            {
                return HttpNotFound();
            }
            ViewBag.Store_ID = new SelectList(db.Stores, "ID", "Name", ingredient.Store_ID);
            ViewBag.CancelUrl = Helpers.GetCancelUrl(Request, Url);
            return View(ingredient);
        }

        // POST: Ingredients/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,Name,Amount,Amount_Unit,Amount_Low,Date_Restocked,Shelf_Life,Store_ID")] Ingredient ingredient)
        {
            if (ModelState.IsValid)
            {
                db.Entry(ingredient).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Store_ID = new SelectList(db.Stores, "ID", "Name", ingredient.Store_ID);
            ViewBag.CancelUrl = Helpers.GetCancelUrl(Request, Url);
            return View(ingredient);
        }

        // GET: Ingredients/Details/5
        public ActionResult Details(int? id, bool isDeleting = false)
        {
            if (id == null)
            {
                return RedirectToAction("Index");
            }
            Ingredient ingredient = db.Ingredients.Find(id);
            if (ingredient == null)
            {
                return HttpNotFound();
            }
            if (isDeleting)
                ViewBag.CancelUrl = Helpers.GetCancelUrl(Request, Url);
            return View(ingredient);
        }

        // GET: Ingredients/Delete/5
        public ActionResult Delete(int? id)
        {
            return RedirectToAction("Details", new { id = id, isDeleting = true });
        }

        // POST: Ingredients/Details/5
        [HttpPost, ActionName("Details")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Ingredient ingredient = db.Ingredients.Find(id);
            db.Ingredients.Remove(ingredient);
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
