using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebAuction.Models
{
    public class TradersValidator
    {
        private Trader _newTrader;
        public TradersValidator()
        {
        }
        public string tr_fName_check { set; get; }
        public string _newTrader_fullName { set; get; }

        public string tr_eMail_check { set; get; }
        public string _newTrader_eMail { set; get; }

        public string tr_firPass_check { set; get; }
        public string _newTrader_password { set; get; }

        public string tr_secPass_check { set; get; }
        public string try_newTrader_pass { set; get; }
        private bool? tryStrings_inDataBase_OnCopies()
        {
            return true;
        }
        public bool? InitTraderValid()
        {
            // that will be a result
            bool? order = true;

            //the begin of the server validation is here
            // fullName is checking
            
            if (String.IsNullOrEmpty(_newTrader_fullName) 
                || _newTrader_fullName.Length >= 50)
            {
                tr_fName_check = "the field fullName is empty or oversize";
                order = false;
            }else tr_fName_check = "the field fullName is right";

            // eMeil is checking 
            // order may be false

            if (String.IsNullOrEmpty(_newTrader_eMail)
                || _newTrader_eMail.Length >= 50)
            {
                tr_eMail_check = "the field eMail is empty or oversized";
                order = false;
            }
            else {
                tr_eMail_check = "the field eMail is right";
                order = _newTrader_eMail.Contains("@");
            }

            // password is checking 
            
            
            if (String.IsNullOrEmpty(_newTrader_password)
                || _newTrader_password.Length >= 50)
            {
                tr_firPass_check = "the first field password is empty or oversized";
                order = false;
            }else tr_firPass_check = "the first field password is right";

            // password is checking in second time
            if (String.IsNullOrEmpty(try_newTrader_pass)
                || try_newTrader_pass.Length >= 50)
            {
                tr_secPass_check = "the second field password is empty or oversized";
                order = false;
            }
            else tr_secPass_check = "the second field password is right";

            return order;
        }
        public Trader Forming_aTrader()
        {
            _newTrader = new Trader();
            _newTrader.trader_fullName = _newTrader_fullName;
            _newTrader.trader_eMail = _newTrader_eMail;
            _newTrader.trader_password = _newTrader_password;
            _newTrader.trader_score = 0.0;
            _newTrader.trader_isReady = false;
            return _newTrader;
        }
    }

}