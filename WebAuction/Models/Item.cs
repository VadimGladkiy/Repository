using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Media;
using System.Drawing;
using System.ComponentModel.DataAnnotations;

namespace WebAuction.Models
{
    public class Item
    {
       public int item_ID { set; get; }
       public string item_name { set; get; }
       public string item_description { set; get;}
       public double item_price { set; get; } 
       public string item_offtime { set; get; } 
       public string item_category { set; get; }
       public string item_pathToPict { set; get; } 
       public int item_lord { set; get; }
    }
}