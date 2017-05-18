using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebAuction.Models
{
    public class Trader
    {
        
        public int trader_ID { set; get; }
        public string trader_fullName { set; get; }
        public string trader_eMail { set; get; }
        public string trader_password { set; get; }
        public double trader_score { set; get; }
        public bool? trader_isReady { set; get; }
        
    }
}