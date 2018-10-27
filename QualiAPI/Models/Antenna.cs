using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QualiAPI.Models
{
    public class Antenna
    {
        //public int _id { get; set; }
        //public int OBJECTID { get; set; }
        //public int ID { get; set; }
        public string company { get; set; }
        public string site_num { get; set; }
        public string city  { get; set; }
        public string address { get; set; }
        public string local_auth { get; set; }
        public string shiput { get; set; }
        public double X   { get; set; }
        public double Y { get; set; }
        //public string type_ { get; set; }
        //public DateTime hakama_dat { get; set; }
        //public DateTime hafala_dat { get; set; }
        //public DateTime last_bedik { get; set; }
        //public string permit { get; set; }
        //public string max_intens { get; set; }
        //public string hakama_fil { get; set; }
        ////public string hafala_fil { get; set; }
       public double intensity { get; set; }
        public double percent_ { get; set; }
        //public int _full_count { get; set; }
        //public float rank { get; set; }
        public double result { get; set; }
        public double Distance { get; set; }
    }
}