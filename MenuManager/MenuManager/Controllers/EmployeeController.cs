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
    public class EmployeeController : Controller
    {
        private MenuManagerDBEntities db = new MenuManagerDBEntities();

        // GET: Employees
        public ActionResult Index(int? s)
        {
            var employees = db.Employees.Include(e => e.Store);
            if (s != null)
                employees = employees.Where(e => (e.Store_ID == s));
            employees = employees.OrderBy(e => e.Store.Name).ThenBy(e => e.Position).ThenBy(e => e.LName);
            return View(employees.ToList());
        }

        // GET: Employees/Create
        public ActionResult Create()
        {
            ViewBag.Store_ID = new SelectList(db.Stores, "ID", "Name");
            ViewBag.CancelUrl = Helpers.GetCancelUrl(Request, Url);
            return View();
        }

        // POST: Employees/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,FName,LName,Position,Wage,Phone,Store_ID")] Employee employee)
        {
            if (ModelState.IsValid)
            {
                db.Employees.Add(employee);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Store_ID = new SelectList(db.Stores, "ID", "Name", employee.Store_ID);
            ViewBag.CancelUrl = Helpers.GetCancelUrl(Request, Url);
            return View(employee);
        }

        // GET: Employees/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("Index");
            }
            Employee employee = db.Employees.Find(id);
            if (employee == null)
            {
                return HttpNotFound();
            }
            ViewBag.Store_ID = new SelectList(db.Stores, "ID", "Name", employee.Store_ID);
            ViewBag.CancelUrl = Helpers.GetCancelUrl(Request, Url);
            return View(employee);
        }

        // POST: Employees/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,FName,LName,Position,Wage,Phone,Store_ID")] Employee employee)
        {
            if (ModelState.IsValid)
            {
                db.Entry(employee).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Store_ID = new SelectList(db.Stores, "ID", "Name", employee.Store_ID);
            ViewBag.CancelUrl = Helpers.GetCancelUrl(Request, Url);
            return View(employee);
        }

        // GET: Employees/Details/5
        public ActionResult Details(int? id, bool isDeleting = false)
        {
            if (id == null)
            {
                return RedirectToAction("Index");
            }
            Employee employee = db.Employees.Find(id);
            if (employee == null)
            {
                return HttpNotFound();
            }
            if (isDeleting)
                ViewBag.CancelUrl = Helpers.GetCancelUrl(Request, Url);
            return View(employee);
        }

        // GET: Employees/Delete/5
        public ActionResult Delete(int? id)
        {
            return RedirectToAction("Details", new { id = id, isDeleting = true });
        }

        // POST: Employees/Details/5
        [HttpPost, ActionName("Details")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Employee employee = db.Employees.Find(id);
            db.Employees.Remove(employee);
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
