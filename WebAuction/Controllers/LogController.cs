using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebAuction.Models;
using WebAuction.Infrastructure;

namespace WebAuction.Controllers
{
    public class LogController : Controller
    {
        private Trader _logUsr;
        private RepositoryForTrader _repositForLog;
        // GET: Log
        public LogController()
        {
            _repositForLog =
            RepositControllersExtension.getReferReposit(this);
        }
        
        public PartialViewResult LogInOut(bool forgotFields = false)
        {
            ViewData["forgotenfields"] = false;
            if (forgotFields == true){
                ViewData["forgotenfields"] = true; 
            }
            string a = (string)Request.Form["em"];
            string b = (string)Request.Form["ps"];

            if (a == null || b == null) return PartialView();

        _logUsr = _repositForLog
        .getItemFromDB_byParams(ChoiseTraderColumn.EMailAndPassword,a,b).First();

            return PartialView(_logUsr);
        }
        public void PassRepair()
        {
            //
        }
    }
}