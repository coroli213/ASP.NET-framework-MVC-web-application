using System;
using WebApplication2.Models;
using System.Collections.Generic;
using System.Data.Entity;
using System.IdentityModel.Claims;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Diplom_Autentif.Models;
using System.Threading;
using System.Security.Claims;

namespace WebApplication2.Controllers
{
    [Authorize(Roles = "Admin,User")]
    public class ClientController : Controller
    {
        private BankContext db = new BankContext();

        // GET: Topics
        public ActionResult Index()
        {
            if (User.IsInRole("Admin"))
            {
                return View(db.Clients.ToList());
            }

            var identity = (ClaimsPrincipal)Thread.CurrentPrincipal;
            var id = identity.Claims.Where(c => c.Type == "DbId").Select(c => c.Value).SingleOrDefault();

            return View(db.Clients.Where(c => c.Id.ToString() == id).ToList());
        }

        // GET: Topics/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Client topic = db.Clients.Find(id);
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
        public ActionResult Create([Bind(Include = "Id,First_name,Second_name,Third_name,Serial,Numb")] Client client)
        {
            if (ModelState.IsValid)
            {
                db.Clients.Add(client);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

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
            Client topic = db.Clients.Find(id);
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
            Client topic = db.Clients.Find(id);
            db.Clients.Remove(topic);
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