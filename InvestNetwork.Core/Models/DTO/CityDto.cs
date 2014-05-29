using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace InvestNetwork.Core
{
    public class CityDTO
    {
        public int CityID { get; set; }
        public string CityName { get; set; }
        public int RegionID { get; set; }
        public int CountryID { get; set; }
    }
}