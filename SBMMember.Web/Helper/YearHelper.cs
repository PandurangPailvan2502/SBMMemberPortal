
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SBMMember.Web.Helper
{
    public class YearHelper
    {

        public static List<SelectListItem> GetYears()
        {
            const int numberOfYears = 15;
            var startYear = DateTime.Now.Year;
            var endYear = startYear - numberOfYears;

            var yearList = new List<SelectListItem>();
            for (var i = startYear; i >= endYear; i--)
            {
                yearList.Add(new SelectListItem() { Value = i.ToString(), Text = i.ToString() });
            }
            return yearList;
        }
    }
}
