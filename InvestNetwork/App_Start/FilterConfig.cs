﻿using InvestNetwork.Core;
using System.Web;
using System.Web.Mvc;

namespace InvestNetwork
{
    public class FilterConfig
    {
        /// <summary>
        /// Регестрирует все глобальные фильтры </summary>
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new CustomHandleError(), 1);
            filters.Add(new HandleErrorAttribute(), 2);
        }
    }
}
