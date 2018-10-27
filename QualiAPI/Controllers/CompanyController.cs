using AntenaServices;
using QualiAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Http.Cors;
using System.Web.Mvc;

namespace QualiAPI.Controllers
{

    [EnableCors("*", "*", "*")]
    public class CompanyController : ApiController
    {
        public IHttpActionResult Post([FromBody]Company[] companies)
        {
            Dictionary<string, double> dict = new Dictionary<string, double>();
            dict = Service1.SummaryAntennas(companies);
            return Ok(dict);
        }
        public IHttpActionResult Get()
        {
            Dictionary<string, double> dict = new Dictionary<string, double>();
            dict = Service1.StatisticsAntennas();
            return Ok(dict);
        }

    }
}