using System;
using System.ComponentModel.DataAnnotations;
using IronWebScraper;


namespace ShopApp.Models
{
    public class Notebook
    {        
        public int ID { get; set; }
        public string Name { get; set; }
        public string  GPU { get; set; }
        public decimal RAM { get; set; }
        public string Processor { get; set; }

        [Display(Name = "Screen size")]
        [DataType(DataType.Text)]
        public decimal ScreenSizeInch { get; set; }   
        public string RouteToImage { get; set; }
    }
}
