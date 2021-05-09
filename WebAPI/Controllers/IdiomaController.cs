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
    public class IdiomaController : ApiController
    {
        ApiResponse apiResponse = new ApiResponse();

        public IHttpActionResult Post(Idioma idioma)
        {
            try
            {
                var mng = new IdiomaManager();
                mng.Create(idioma);

                apiResponse = new ApiResponse();
                apiResponse.Message = "Accion hecha con exito";

                return Ok(apiResponse);
            }
            catch(BussinessException bex)
            {
                return InternalServerError(new Exception(bex.ExceptionId + "-"
                    + bex.AppMessage.Message));
            }
        }

        public IHttpActionResult Get()
        {
            try
            {
                var mng = new IdiomaManager();
                apiResponse.Data = mng.RetrieveAll();

                return Ok(apiResponse.Data);
            }
            catch (BussinessException bex)
            {
                return InternalServerError(new Exception(bex.ExceptionId + "-" + bex.AppMessage.Message));
            }
        }

        public IHttpActionResult Get(string idioma)
        {
            try
            {
                var mng = new IdiomaManager();
                var envio = new Idioma();
                envio.IdiomaNuevo = idioma;

                envio = mng.RetrieveById(envio);
                apiResponse.Data = envio;
                return Ok(apiResponse.Data);
            }
            catch (BussinessException bex)
            {
                return InternalServerError(new Exception(bex.ExceptionId + "-" + bex.AppMessage.Message));
            }
        }
    }
}
