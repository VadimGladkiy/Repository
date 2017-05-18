using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Configuration;
using System.Web.Mvc;
using WebAuction.Infrastructure;
using WebAuction.Models;

namespace WebAuction.Controllers
{
    public class BaseController : Controller
    {
        private TradersActivity _currTraderActivity;
        private Trader _currTrader;
        private bool TraderState;
        private RepositoryForTrader _repositForCurrTrader;
        private List<Models.Item> baseList;
        private string Dbconnection;
        // post: 
        public BaseController()
        {
            //connectionString = WebConfigurationManager
            // .ConnectionStrings["TradersDB"].ConnectionString;
            Dbconnection = WebConfigurationManager
            .ConnectionStrings["TradersDB"].ConnectionString; 
            _repositForCurrTrader = new RepositoryForTrader();
            _repositForCurrTrader.initialDBConnect(Dbconnection);

            Infrastructure.
            RepositControllersExtension.setReferenceOnRepos(this,_repositForCurrTrader);
            TraderState = false;
            baseList = new List<Item>();
        }
        public ActionResult Registration(Models.TradersValidator _newDataFromForm)
        { 
            // take a new trader's info
            // start a validation on server 
            var newTradersValidator = new TradersValidator();

            newTradersValidator = _newDataFromForm;
            // 
            bool? IsChecked = newTradersValidator.InitTraderValid();
            if (IsChecked == true)
            {
                _repositForCurrTrader.addItemToDB(newTradersValidator.Forming_aTrader());
                return RedirectToAction("BaseAuction");
            } 
            return View(newTradersValidator);
        }

        // do view for All users
        public ActionResult BaseAuction(string category = null)
        {
            // to prepare variable to start searching by trader's name
            List<Trader> namesByQuery;
            string []idsByQuery;
            int count = 0;
            string name;
            try
            {
                 name = Request.Form["filter_items_by_trader's_name"].ToString();
            }
            catch (Exception e) { name = null; }
            // if searching by trader's name was started
            // get all the traders by name
            if (!String.IsNullOrEmpty(name))
            {
            // where is the [new] operator for this ?
            namesByQuery = _repositForCurrTrader
            .getItemFromDB_byParams(ChoiseTraderColumn.Name,name);
                count = namesByQuery.Count();
                idsByQuery = new string[count];
                for (var i=0; i < count; i++)
                {
                    idsByQuery[i] =  namesByQuery.First().trader_ID.ToString();
                    baseList.AddRange(_repositForCurrTrader
                        .getAllItemsFromDB(idsByQuery[i], category));
                }
                // output a small list
                return View(baseList);
            }

            // output a big list  
            baseList =
            _repositForCurrTrader.getAllItemsFromDB(null,category);
            return View(baseList);
        }
    }
    
}