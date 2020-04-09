using Diplom_Autentif.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.Owin.Security;
using WebApplication2.Models;
using System.Security.Claims;
using System.Threading;

namespace WebApplication2.Controllers
{
   // [Authorize(Roles = "Admin,User")]
    public class BillController : Controller
    {
        private BankContext db = new BankContext();

        // GET: Topics
        public ActionResult Index()
        {
            if (User.IsInRole("Admin"))
            {
                return View(db.Bills.ToList());
            }

            var identity = (ClaimsPrincipal)Thread.CurrentPrincipal;
            var id = identity.Claims.Where(c => c.Type == "DbId").Select(c => c.Value).SingleOrDefault();

            var value = db.Cards.Where(c => c.Owner.Id.ToString() == id).ToList();

            List<Bill> bills = new List<Bill>();

            foreach(var i in db.Bills)
            {
                for(int k = 0; k < value.Count; k++)
                {
                    if (i == value[k].Account)
                    {
                        bills.Add(i);
                    }

                }
            }

            return View(bills);
        }

        // GET: Topics/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Bill topic = db.Bills.Find(id);
            if (topic == null)
            {
                return HttpNotFound();
            }
            return View(topic);
        }

        // GET: Topics/Create
        [Authorize(Roles = "Admin")]
        public ActionResult Create()
        {
            return View();
        }

        // POST: Topics/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Balance,Bank_owner,Cards,Operations")] Bill bill)
        {
            if (ModelState.IsValid)
            {
                bill.Cards = new List<Card>();
                bill.Operations = new List<Operation>();


                db.Bills.Add(bill);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(bill);
        }

        // GET: Questions/Edit/5
        [Authorize(Roles = "Admin")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Bill card = db.Bills.Find(id);

            if (card == null)
            {
                return HttpNotFound();
            }

            var selectedItems = " ";

            // foreach (var card in client.Cards)
            // {
            //     selectedItems += ";" + card.Number.ToString();
            // }

            ViewData["SelectedCards"] =
                string.IsNullOrWhiteSpace(selectedItems)
                    ? ""
                    : selectedItems.Substring(1);

            //ViewData["Cards"] = new MultiSelectList(db.Cards, "Id", "Number");

            return View(card);
        }

        // POST: Questions/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Balance,Bank_owner,Cards,Operations")]Bill card)
        {
            if (ModelState.IsValid)
            {
                var n = card.Balance;
                var p = card.Bank_owner;

                card = db.Bills.Find(card.Id);

                card.Balance = n;
                card.Bank_owner = p;


                db.Entry(card).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(card);
        }

        // GET: Topics/Delete/5
        [Authorize(Roles = "Admin")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Bill card = db.Bills.Find(id);

            if (card == null)
            {
                return HttpNotFound();
            }
            return View(card);
        }

        // POST: Topics/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Bill card = db.Bills.Find(id);
            db.Bills.Remove(card);
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