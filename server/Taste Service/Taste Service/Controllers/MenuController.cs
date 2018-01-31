using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;

namespace Taste_Service.Controllers
{
    [RoutePrefix("api")]
    public class MenuController : ApiController
    {
        [HttpGet]
        [Route("user")]
        public string GetUser()
        {
            return "String";
        }
    }
}