﻿using Microsoft.AspNetCore.Mvc.Rendering;


namespace RetailSales.Models
{
    public class City
    {
        //binding countrty and state
        public City() 
        {
            this.colist = new List<SelectListItem>();
            this.stlist = new List<SelectListItem>();
        }
        public List<SelectListItem> colist;
        public List<SelectListItem> stlist;


        public string? ID { get; set; }
        public string? CityName { get; set; }
        public string? StateId { get; set; }
        public string? CountryId { get; set; }

        // used for enabled/disabled dropdownlist
        public string? ddlstatus { get; set; }
        
    }

    // used to store values in database
    public class Citygrid
    {
        public string? id { get; set; }
        public string? ciname { get; set; }
        public string? statid { get; set; }
        public string? counid { get; set; }
        public string? edit { get; set; }
        public string? delete { get; set; }
    }
}