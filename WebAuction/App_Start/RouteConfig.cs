using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace WebAuction
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "",
                url: "BaseController/Registration",
                defaults: new
                {
                    controller = "Base",
                    action = "Registration",
                }
            );

            routes.MapRoute(
                name: "Auction",
                url: "BaseController/BaseAuction/{category}",
                defaults: new
                {
                    controller = "Base",
                    action = "BaseAuction",
                    category = UrlParameter.Optional
                }

            );
            routes.MapRoute(
                name: null,
                url: "",
                defaults: new
                {
                    controller = "Base",
                    action = "BaseAuction",
                    category = UrlParameter.Optional
                }

            );

            routes.MapRoute(
                            "log",
                            url: "LogController/LogInOut/{forgotFields}",
                            defaults: new
                            {
                                controller = "Log",
                                action = "LogInOut",
                                forgotFields = UrlParameter.Optional
                            }
                        );

            routes.MapRoute(
                            null,
                            url: "LogController/PassRepair",
                            defaults: new
                            {
                                controller = "Log",
                                action = "PassRepair"
                                
                            }
                        );
            routes.MapRoute(
                name: "acc",
                url: "AccountController/OwnCabinet/{trader_id}",
                defaults: new
                {
                    controller = "Account",
                    action = "OwnCabinet",
                    trader_id = UrlParameter.Optional
                }
            );
            routes.MapRoute(
                name: null,
                url: "AccountController/MayAddItem/{SuccessfulAdding}",
                defaults: new
                {
                    controller = "Account",
                    action = "MayAddItem",
                    SuccessfulAdding = UrlParameter.Optional
                }
            );
            routes.MapRoute(
                name: null,
                url: "AccountController/addNewNote",
                defaults: new
                {
                    controller = "Account",
                    action = "addNewNote"
                   
                }
            );
            routes.MapRoute(
                name: null,
                url: "AccountController/ChangeItemsName",
                defaults: new
                {
                    controller = "Account",
                    action = "ChangeItemsName",


                }
            );
            
                
        }
    }
}
