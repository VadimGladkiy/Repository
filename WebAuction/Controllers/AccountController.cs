using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Web.Mvc;
using WebAuction.Models;
using WebAuction.Infrastructure;
using System.IO;
using System.Drawing;
using System.Windows.Forms;
using System.Threading.Tasks;

namespace WebAuction.Controllers
{
    public class AccountController : Controller
    {
        private static string PhysicalPathOfCatalog;
        private Trader _accUsr;
        private static int _lordId;
        private RepositoryForTrader _repositForAcc;
        private List<Models.Item> itemsList;
        private string Dbconnection;
        //private string PathTo_Home;
        public AccountController()
        {
            Dbconnection = WebConfigurationManager
            .ConnectionStrings["TradersDB"].ConnectionString;
            
            _repositForAcc =
            RepositControllersExtension.getReferReposit(this);
            _repositForAcc.initialDBConnect(Dbconnection);
            itemsList = new List<Item>();

        }
        // GET: Account
        public ActionResult OwnCabinet(string trader_id)
        {
            // define the user
            _accUsr = _repositForAcc
            .getItemFromDB_byParams(ChoiseTraderColumn.ID, trader_id).First();
            //
            _lordId = Int32.Parse(trader_id);
            // get all items
            itemsList = _repositForAcc.getAllItemsFromDB(trader_id, null);

            PhysicalPathOfCatalog  = CreateSubString(Request.PhysicalPath,
                (string)RouteData.Values["controller"],
                (string)RouteData.Values["action"], 
                (string)RouteData.Values["trader_id"] );

            return View(itemsList);
        }
        
        public ActionResult MayAddItem(bool SuccessfulAdding = false)
        {
            if (SuccessfulAdding == true)
            {
                return PartialView("AddingItemGetForm");
            }
            return PartialView("AddingItemAskForm");
        }
        [HttpPost]
        public ActionResult AddNewNote(HttpPostedFileBase image)
        {
            // create the unique number for trader's item
            Random random = new Random();
            string rand = random.Next(-1000, 1001).ToString(); 
            string time = DateTime.Now.ToString();
            time = time.Replace(' ','_');
            time = time.Replace(':','i');

            // define a name of file image
            string nameofFile = image.FileName;

            // create the part of unique path to the image
            string imgFuturePath = _lordId.ToString() + "r"+rand+"t"+time + nameofFile; 

            // Convert received file to Image format:
            var filename = Path.GetFileName(image.FileName);
            Image sourceImage =
            Image.FromStream(image.InputStream);

            // create a new note for DB
            Item _newItem = new Item();
            string item_price;

            // full the fields of the item's properties 
            _newItem.item_name = (string)Request.Form["name"];
            _newItem.item_description = (string)Request.Form["descript"];
            item_price = (string)Request.Form["price"];
            _newItem.item_price = double.Parse(item_price);
            _newItem.item_offtime = DateTime.Now.ToString();
            _newItem.item_category = (string)Request.Form["categ"];
            _newItem.item_pathToPict = "/PicturesContent/" + imgFuturePath; 
            _newItem.item_lord = _lordId;

            // start a server's validation
            bool isValid = ItemAddingValidation(_newItem);

            
            // try to push the item to DB 
            if (isValid == true) { 
                _repositForAcc.pushItemtoDb(_newItem,_lordId);

                // try to save the file
                try
                {
                    sourceImage.Save(PhysicalPathOfCatalog + "PicturesContent\\" + imgFuturePath);
                }
                catch (Exception e) { MessageBox.Show(e.Message); }

                // redirect  
                return RedirectToAction("OwnCabinet", new { trader_id = _lordId });
            }
            
            return RedirectToAction("OwnCabinet", new { trader_id = _lordId });
        }
        private static string CreateSubString(string mega_arg,params string [] str) 
        {
            string[] subs = mega_arg.Split('\\');
            string substring="";
            for (int i = 0; i < subs.Length-str.Length; i++)
            substring += subs[i] + '\\'; 
            
            return substring; 
        }
        public async Task<RedirectToRouteResult> ChangeItemsName()
        {
            string newName = Request.Form["newName"].ToString();
            int itemID = Int32.Parse( Request.Form["itenID"].ToString());
            return await Task.Run(() => 
            {
                _repositForAcc.changeItemsName(newName, itemID);
                 return RedirectToRoute("acc",new { trader_id = _lordId });
            });
                //_repositForAcc.changeItemsName(newName, itemID);
            //return new EmptyResult();
        }
        private static bool ItemAddingValidation(Item receivedItem)
        {
            bool result = true;
            if (receivedItem.item_name == "" || receivedItem.item_name.Length >= 30) result = false;
            if (receivedItem.item_description == "" || receivedItem.item_description.Length >= 100) result = false;
            if (receivedItem.item_price.GetType() != typeof(double)) result = false;      
            return result;
        }
    }
}