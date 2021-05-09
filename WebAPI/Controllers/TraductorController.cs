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
    [ExceptionFilter]
    public class TraductorController : ApiController
    {
        ApiResponse apiResponse = new ApiResponse();

        public IHttpActionResult Post(Traductor traductor)
        {
            try
            {
                var mng = new TraductorManager();
                mng.Create(traductor);

                apiResponse = new ApiResponse();
                apiResponse.Message = "Accion hecha con exito";

                return Ok(apiResponse);
            }
            catch (BussinessException bex)
            {
                return InternalServerError(new Exception(bex.ExceptionId + "-"
                    + bex.AppMessage.Message));
            }
        }

        [HttpPut]
        public IHttpActionResult GetWord(Traductor traductor)
        {
            try
            {
                var mng = new TraductorManager();

                traductor = mng.RetrieveById(traductor);
                apiResponse.Data = traductor;

                return Ok(apiResponse.Data);
            }
            catch (BussinessException bex)
            {
                return InternalServerError(new Exception(bex.ExceptionId + "-" + bex.AppMessage.Message));
            }
        }

        [HttpGet]
        public IHttpActionResult RetrieveAll()
        {
            try
            {
                var mng = new TraductorManager();

                apiResponse.Data = mng.RetrieveAll();

                return Ok(apiResponse.Data);
            }
            catch (BussinessException bex)
            {
                return InternalServerError(new Exception(bex.ExceptionId + "-" + bex.AppMessage.Message));
            }
        }
    }
}
