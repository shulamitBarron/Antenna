using AntenaServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Http.Cors;

namespace QualiAPI.Controllers
{
    [EnableCors("*", "*", "*")]
    public class RadiusController : ApiController
    {
        public IHttpActionResult Get(double x, double y)
        { Dictionary<string, double> dict = new Dictionary<string, double>();
       dict= Service1.SearchAntennas(x, y);
            dict.ToList().Sort((pair1, pair2) => pair1.Value.CompareTo(pair2.Value));
            return Ok(dict);
        }

      
    }
}