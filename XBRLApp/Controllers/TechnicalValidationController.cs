using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using XBRLApp.DAL;

namespace XBRLApp.Controllers
{
    public class TechnicalValidationController : Controller
    {
        private XbrlDbCsEntities db = new XbrlDbCsEntities();
        //
        // GET: /TechnicalValidation/
        public ActionResult Index()
        {
            return View();
        }

        //public ActionResult GetList(string sidx, string sord, int page, int rows)
        //{
        //    // GetTodoLists(sidx, sord ,page, rows);

        //    return View();
        //}

        public JsonResult GetLists(string sidx, string sord, int page, int rows)  //Gets the todo Lists.
        {
            int pageIndex = Convert.ToInt32(page) - 1;
            int pageSize = rows;
            var listsResults = (from l in db.xbrl_Form01
                                    select l);
            int totalRecords = listsResults.Count();
            var totalPages = (int)Math.Ceiling((float)totalRecords / (float)rows);
            if (sord.ToUpper() == "DESC")
            {
                listsResults = listsResults.OrderByDescending(s => s.ID);
                listsResults = listsResults.Skip(pageIndex * pageSize).Take(pageSize);
            }
            else
            {
                listsResults = listsResults.OrderBy(s => s.ID);
                listsResults = listsResults.Skip(pageIndex * pageSize).Take(pageSize);
            }
            var jsonData = new
            {
                total = totalPages,
                page,
                records = totalRecords,
                rows = listsResults
            };
            return Json(jsonData, JsonRequestBehavior.AllowGet);
        }



	}
}