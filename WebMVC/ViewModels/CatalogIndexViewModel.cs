﻿using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebMVC.Models;

namespace WebMVC.ViewModels
{
    public class CatalogIndexViewModel
    {
        public PaginationInfo PaginationInfo { get; set; }
        public IEnumerable<SelectListItem> Categories { get; set; }
        public IEnumerable<SelectListItem> Types { get; set; }
        public IEnumerable<SelectListItem> Locations { get; set; }
        public IEnumerable<CatalogEvent> CatalogEvents { get; set; }

        public int? CategoryFilterApplied { get; set; }
        public int? TypesFilterApplied { get; set; }
        public int? LocationFilterApplied { get; set; }
    }
}
