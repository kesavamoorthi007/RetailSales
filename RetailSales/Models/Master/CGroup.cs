﻿using Microsoft.AspNetCore.Mvc.Rendering;

namespace RetailSales.Models
{
    public class CGroup
    {
        public string ID {  get; set; }
        public string CustomerGroup {  get; set; }
        public string Description {  get; set; }
        public string ddlStatus {  get; set; }
    }
    public class CGroupgrid
    {
        public string id { get; set; }
        public string group { get; set; }
        public string des { get; set; }
        public string editrow { get; set; }
        public string delrow { get; set; }

    }
}