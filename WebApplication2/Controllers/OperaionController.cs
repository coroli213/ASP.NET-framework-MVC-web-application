using Diplom_Autentif.Models;
using System;
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
    public class OperationController : Controller
    {
        private BankContext db = new BankContext();

        // GET: Topics
        public ActionResult Index()
        {
            if (User.IsInRole("Admin"))
            {
                return View(db.Operations.ToList());
            }

            var identity = (ClaimsPrincipal)Thread.CurrentPrincipal;
            var id = identity.Claims.Where(c => c.Type == "DbId").Select(c => c.Value).SingleOrDefault();

            var cards = db.Cards.Where(c => c.Owner.Id.ToString() == id).ToList();

            List<Bill> bills = new List<Bill>();

            foreach (var i in db.Bills)
            {
                for (int k = 0; k < cards.Count; k++)
                {
                    if (i == cards[k].Account)
                    {
                        bills.Add(i);
                    }   
                }
            }

            List<Operation> operations = new List<Operation>();

            foreach (var i in db.Operations)
            {
                foreach (var k in bills)
                {
                    if (i.Account_from?.Id == k.Id || i.Account_to.Id == k.Id)
                    {
                        operations.Add(i);
                    }
                }
            }


            return View(operations);
        }

        // GET: Topics/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Operation topic = db.Operations.Find(id);
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

            ViewData["Terminal"]     = new MultiSelectList(db.Terminals, "Id", "Id");
            ViewData["Account_from"] = new MultiSelectList(db.Bills, "Id", "Id");
            ViewData["Account_to"]   = new MultiSelectList(db.Bills, "Id", "Id");

            return View();
        }

        // POST: Topics/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Type,Amount,Terminal,Account_to,Account_from")] Operation client,
                                    int terminal, int accountto, int? accountfrom)
        {

            client.Terminal = db.Terminals.Find(terminal);
            if (accountfrom != null)
                client.Account_from = db.Bills.Find(accountfrom);
            client.Account_to = db.Bills.Find(accountto);

            db.Operations.Add(client);
            db.SaveChanges();
            return RedirectToAction("Index");


            return View(client);
        }

        // GET: Questions/Edit/5
        [Authorize(Roles = "Admin")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Client client = db.Clients.Find(id);

            if (client == null)
            {
                return HttpNotFound();
            }

            var selectedItems = " ";

            foreach (var card in client.Cards)
            {
                selectedItems += ";" + card.Number.ToString();
            }

            ViewData["SelectedCards"] =
                string.IsNullOrWhiteSpace(selectedItems)
                    ? ""
                    : selectedItems.Substring(1);

            //ViewData["Cards"] = new MultiSelectList(db.Cards, "Id", "Number");

            return View(client);
        }

        // POST: Questions/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,First_name,Second_name,Third_name,Serial,Numb")] Client client, int[] cards)
        {
            if (ModelState.IsValid)
            {
                var f = client.First_name;
                var s = client.Second_name;
                var t = client.Third_name;
                var se = client.Serial;
                var nu = client.Numb;

                // Подхватываем Вопрос из БД, ибо иначе EF не подтягивает нам связанные сущности,
                // из-за чего и не обновляется список тем.
                client = db.Clients.Find(client.Id);

                // Очищаем старый список, чтобы не было ошибок при сохранении
                // (иначе заорет на попытку добавления повторяющегося ключа).
                // client.Cards.Clear();

                client.First_name  = f;
                client.Second_name = s;
                client.Third_name  = t;
                client.Serial = se;
                client.Numb   = nu;

                var selectedTopics = new List<Card>();
                if (cards != null)
                {
                    foreach (var top in cards)
                    {
                        selectedTopics.Add(db.Cards.SingleOrDefault(to => to.Id == top));
                    }
                }

                client.Cards = selectedTopics;
                db.Entry(client).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(client);
        }


        // GET: Topics/Delete/5
        [Authorize(Roles = "Admin")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Operation topic = db.Operations.Find(id);
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
            Operation topic = db.Operations.Find(id);
            db.Operations.Remove(topic);
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