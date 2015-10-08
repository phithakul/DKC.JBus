using AutoMapper;
using JBus.Web.Models;
using JBus.Web.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace JBus.Web.Controllers
{
    public class BusOperatorController : Controller
    {
        // GET: BusOperator
        public ActionResult Index()
        {
            var busOperators = Current.DB.BusOperators.All();
            var model = Mapper.Map<IEnumerable<BusOperator>, IEnumerable<BusOperatorViewModel>>(busOperators);
            return View(model);
        }

        // GET: BusOperator/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: BusOperator/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: BusOperator/Create
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

        // GET: BusOperator/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: BusOperator/Edit/5
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

        // GET: BusOperator/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: BusOperator/Delete/5
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