using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace JBus.Web.Controllers
{
    public class PaymentGroupController : Controller
    {
        // GET: PaymentGroup
        public ActionResult Index()
        {
            return View();
        }

        // GET: PaymentGroup/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: PaymentGroup/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: PaymentGroup/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: PaymentGroup/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: PaymentGroup/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: PaymentGroup/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: PaymentGroup/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
