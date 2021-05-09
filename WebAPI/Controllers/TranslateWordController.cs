using CoreAPI;
using EntitiesPOJO;
using Newtonsoft.Json;
using Exceptions;
using WebAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace WebAPI.Controllers
{
    public class TranslateWordController : ApiController
    {
        ApiResponse apiResponse = new ApiResponse();

        [HttpPost]
        public IHttpActionResult TranslateWord(Traductor traductor)
        {
            try
            {
                var mng = new TraductorManager();

                traductor = mng.TranslateWord(traductor);
                apiResponse.Data = traductor;

                return Ok(apiResponse.Data);
            }
            catch (BussinessException bex)
            {
                return InternalServerError(new Exception(bex.ExceptionId + "-" + bex.AppMessage.Message));
            }
        }
    }
}
