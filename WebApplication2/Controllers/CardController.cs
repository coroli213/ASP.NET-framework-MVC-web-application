using Diplom_Autentif.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace WebApplication2.Controllers
{
    [Authorize(Roles = "Admin,User")]
    public class CardController : Controller
    {
        private BankContext db = new BankContext();

        // GET: Topics
        public ActionResult Index()
        {
            if (User.IsInRole("Admin"))
            {
                return View(db.Cards.ToList());
            }

            var identity = (ClaimsPrincipal)Thread.CurrentPrincipal;
            var id = identity.Claims.Where(c => c.Type == "DbId").Select(c => c.Value).SingleOrDefault();

            return View(db.Cards.Where(c => c.Owner.Id.ToString() == id).ToList());

        }

        // GET: Topics/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Card topic = db.Cards.Find(id);
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
            ViewData["Clients"] = new MultiSelectList(db.Clients,  "Id", "Id");
            ViewData["Account"] = new MultiSelectList(db.Bills, "Id", "Id");

            
            ViewBag.Teams = new SelectList(db.Clients, "Id", "Id");

            return View();
        }

        // POST: Topics/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Number,Date_of,Csv,Pin")] Card card, int? owner,int? account)
        {
            if (ModelState.IsValid)
            {
                card.Account = db.Bills.Find(account);
                card.Owner   = db.Clients.Find(owner);

                db.Cards.Add(card);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(card);
        }

        // GET: Questions/Edit/5
        [Authorize(Roles = "Admin")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Card card = db.Cards.Find(id);

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
        public ActionResult Edit([Bind(Include = "Id,Number,Date_of,Csv,Pin")]Card card)
        {
            if (ModelState.IsValid)
            {
                var n = card.Number;
                var p = card.Pin;
                var c = card.Csv;

                card = db.Cards.Find(card.Id);

                card.Number = n;
                card.Pin = p;
                card.Csv = c;


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

            Card card = db.Cards.Find(id);

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
            Card card = db.Cards.Find(id);
            db.Cards.Remove(card);
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