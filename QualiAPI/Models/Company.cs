using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MongoDB.Bson;
namespace QualiAPI.Models
{
    public class Company
    {   public ObjectId id { get; set; }
        public string CompanyName { get; set; }
        public double Density { get; set; }
    }
}