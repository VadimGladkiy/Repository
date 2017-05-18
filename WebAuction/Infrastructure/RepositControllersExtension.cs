using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebAuction.Infrastructure
{
    public static class RepositControllersExtension
    {
        private static RepositoryForTrader _refRepos;
        private static string PhysicalPathOfCatalog; 
        public static void setReferenceOnRepos(this System.Web.Mvc.Controller
            setReposRefer , RepositoryForTrader _ref)
        {
            _refRepos = _ref;
        }
        public static RepositoryForTrader getReferReposit(this System.Web.Mvc.Controller
            setReposRefer)
        {
            return _refRepos;
        } 
    }
}