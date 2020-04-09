using Diplom_Autentif.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace WebApplication2.Controllers
{
    [Authorize(Roles = "Admin,User")]
    public class TerminalController : Controller
    {
        private BankContext db = new BankContext();

        // GET: Topics
        public ActionResult Index()
        {
            return View(db.Terminals.ToList());
        }

        // GET: Topics/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Terminal topic = db.Terminals.Find(id);
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
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Balance,Filial_owner")] Terminal terminal)
        {
            if (ModelState.IsValid)
            {
                terminal.Operations = new List<Operation>();
                db.Terminals.Add(terminal);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(terminal);
        }

        // GET: Questions/Edit/5
        [Authorize(Roles = "Admin")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Terminal client = db.Terminals.Find(id);

            if (client == null)
            {
                return HttpNotFound();
            }

        //    var selectedItems = " ";
        //
        //    foreach (var card in client.Terminals)
        //    {
        //        selectedItems += ";" + card.Number.ToString();
        //    }
        //
        //    ViewData["SelectedCards"] =
        //        string.IsNullOrWhiteSpace(selectedItems)
        //            ? ""
        //            : selectedItems.Substring(1);

            //ViewData["Cards"] = new MultiSelectList(db.Cards, "Id", "Number");

            return View(client);
        }

        // POST: Questions/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Balance,Filial_owner")] Terminal terminal)
        {
            if (ModelState.IsValid)
            {
                var b = terminal.Balance;
                var f = terminal.Filial_owner;

                // Подхватываем Вопрос из БД, ибо иначе EF не подтягивает нам связанные сущности,
                // из-за чего и не обновляется список тем.
                // terminal = db.Clients.Find(client.Id);

                // Очищаем старый список, чтобы не было ошибок при сохранении
                // (иначе заорет на попытку добавления повторяющегося ключа).
                // client.Cards.Clear();

                terminal.Balance = b;
                terminal.Filial_owner = f;

             //  var selectedTopics = new List<Card>();
             //  if (cards != null)
             //  {
             //      foreach (var top in cards)
             //      {
             //          selectedTopics.Add(db.Cards.SingleOrDefault(to => to.Id == top));
             //      }
             //  }

              //  client.Cards = selectedTopics;
                db.Entry(terminal).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(terminal);
        }


        // GET: Topics/Delete/5
        [Authorize(Roles = "Admin")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Terminal topic = db.Terminals.Find(id);
            if (topic == null)
            {
                return HttpNotFound();
            }
            return View(topic);
        }

        // POST: Topics/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Terminal topic = db.Terminals.Find(id);
            db.Terminals.Remove(topic);
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