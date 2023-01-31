using System;
using System.Net;
using System.Web.Http;
using RCMAPI.CommonUtilities;
using RCMAPI.Entites;
using RCMAPI.Messages;
namespace RCMAPI.Controllers
{
    public class BaseController : ApiController
    {
        public Base objBase = new Base();

        public static void SetErrorObject(Base objBase, Exception ex, string message)
        {
            if (ex.GetType() == typeof(ApplicationException))
                objBase.Message = ex.Message;
            else
            {
                objBase.Message = message;
                HIS.TOOLS.Logger.ErrorLog.ErrorRoutine(ex, "ALH_MobileAPPService_WebAPI", "", "");
            }

            objBase.Code = ProcessStatus.Fail;
            objBase.Status = ProcessStatus.Fail.ToString();
        }
        protected IHttpActionResult OkOrNotFound(object obj)
        {
            if (obj == null)
            {
                return NotFound();
            }
            return Ok(obj);
        }
       
        protected IHttpActionResult OkOrInternalServerError(object obj)
        {
            if (obj == null)
            {
                return InternalServerError(new Exception(Utilities.DefaultErrorMessage));
            }
            return Ok(obj);
        }

        protected IHttpActionResult OkWithBoolSuccessStatus(bool success)
        {
            return Ok(new SimpleSuccessResult
            {
                Success = success
            });
        }

        protected IHttpActionResult OkWithBoolSuccessStatus(bool success, string message)
        {
            return Ok(new SimpleSuccessResult
            {
                Success = success,
                Message = message
            });
        }
    }
}
