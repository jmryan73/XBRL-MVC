using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using XBRLApp.DAL;

namespace XBRLApp.Controllers
{
    public class FileSecurityController : Controller
    {
        public MsGroupApprovalDAL serv = new MsGroupApprovalDAL();
        //
        // GET: /FileSecurity/
        public ActionResult Index()
        {
            var model = serv.GetAllData();
            return View(model);
        }

        //
        // GET: /FileSecurity/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        //
        // GET: /FileSecurity/Create
        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /FileSecurity/Create
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

        //
        // GET: /FileSecurity/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        //
        // POST: /FileSecurity/Edit/5
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

        //
        // GET: /FileSecurity/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        //
        // POST: /FileSecurity/Delete/5
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
