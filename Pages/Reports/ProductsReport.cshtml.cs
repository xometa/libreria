using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using RosaMaríaBookstore.Models;

namespace RosaMaríaBookstore.Pages.Reports
{
    public class ProductsReportModel : PageModel
    {
        public ProductsReportModel()
        {
        }
        public IActionResult OnGet()
        {
            return Page();
        }
    }

}