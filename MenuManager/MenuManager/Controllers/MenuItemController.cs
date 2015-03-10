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
    public class MenuItemController : Controller
    {
        private MenuManagerDBEntities db = new MenuManagerDBEntities();

        // GET: MenuItem
        public ActionResult Index(int? s, int? m, int? i)
        {
            var menuItems = db.MenuItems.Include(mi => mi.Menu);
            if (s != null)
                menuItems = menuItems.Where(mi => (mi.Menu.Store_ID == s));
            if (m != null)
                menuItems = menuItems.Where(mi => (mi.Menu_ID == m));
            if (i != null)
                menuItems = menuItems.Where(mi => mi.Ingredients.Any(ing => (ing.ID == i)));

            menuItems = menuItems.OrderBy(mi => mi.Menu.Store.Name).ThenBy(mi => mi.Menu.Name);
            return View(menuItems.ToList());
        }

        // GET: MenuItem/Create
        public ActionResult Create()
        {
            ViewBag.Menu_ID = new SelectList(db.Menus, "ID", "StoreAndName");
            ViewBag.Ingredients = new MultiSelectList(db.Ingredients.OrderBy(i => i.Name), "ID", "Name", "Store.Name");
            ViewBag.CancelUrl = Helpers.GetCancelUrl(Request, Url);
            return View();
        }

        // POST: MenuItem/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,Name,Price,Menu_ID,Calories,Is_Vegetarian,Is_Vegan")] MenuItem menuItem)
        {
            if (ModelState.IsValid)
            {
                menuItem.Ingredients = Helpers.GetSelectedIngredients(Request, db.Ingredients);
                db.MenuItems.Add(menuItem);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Menu_ID = new SelectList(db.Menus, "ID", "StoreAndName", menuItem.Menu_ID);
            ViewBag.Ingredients = new MultiSelectList(db.Ingredients.OrderBy(i => i.Name), "ID", "Name", "Store.Name");
            ViewBag.CancelUrl = Helpers.GetCancelUrl(Request, Url);
            return View(menuItem);
        }

        // GET: MenuItem/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("Index");
            }
            MenuItem menuItem = db.MenuItems.Find(id);
            if (menuItem == null)
            {
                return HttpNotFound();
            }
            ViewBag.Menu_ID = new SelectList(db.Menus, "ID", "StoreAndName", menuItem.Menu_ID);
            ViewBag.Ingredients = new MultiSelectList(db.Ingredients.OrderBy(i => i.Name), "ID", "Name", "Store.Name", Helpers.GetIngredientIDs(menuItem.Ingredients));
            ViewBag.CancelUrl = Helpers.GetCancelUrl(Request, Url);
            return View(menuItem);
        }

        // POST: MenuItem/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,Name,Price,Menu_ID,Calories,Is_Vegetarian,Is_Vegan")] MenuItem menuItem)
        {
            if (ModelState.IsValid)
            {
                db.Entry(menuItem).State = EntityState.Modified;
                db.SaveChanges();
                menuItem = db.MenuItems.Include(mi => mi.Ingredients).Single(mi => (mi.ID == menuItem.ID));
                menuItem.Ingredients = Helpers.GetSelectedIngredients(Request, db.Ingredients);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Menu_ID = new SelectList(db.Menus, "ID", "StoreAndName", menuItem.Menu_ID);
            ViewBag.Ingredients = new MultiSelectList(db.Ingredients.OrderBy(i => i.Name), "ID", "Name", "Store.Name", (string)Request["Ingredients"]);
            ViewBag.CancelUrl = Helpers.GetCancelUrl(Request, Url);
            return View(menuItem);
        }

        // GET: MenuItem/Details/5
        public ActionResult Details(int? id, bool isDeleting = false)
        {
            if (id == null)
            {
                return RedirectToAction("Index");
            }
            MenuItem menuItem = db.MenuItems.Find(id);
            if (menuItem == null)
            {
                return HttpNotFound();
            }
            if (isDeleting)
                ViewBag.CancelUrl = Helpers.GetCancelUrl(Request, Url);
            return View(menuItem);
        }

        // GET: MenuItem/Delete/5
        public ActionResult Delete(int? id)
        {
            return RedirectToAction("Details", new { id = id, isDeleting = true });
        }

        // POST: MenuItem/Details/5
        [HttpPost, ActionName("Details")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            MenuItem menuItem = db.MenuItems.Find(id);
            db.MenuItems.Remove(menuItem);
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
