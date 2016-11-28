using System.Net;
using System.Net.Http;
using System.Web.Http;
using ToolKit.DataAnnotation.Test.Models;

namespace ToolKit.DataAnnotation.Test.Controllers
{
    [RoutePrefix("api/account")]
    public class ApiAccountController : ApiController
    {
        [Route("")]
        [HttpPost]
        public HttpResponseMessage InsertAccount([FromBody] Account account)
        {
            if (!ModelState.IsValid)
                return Request.CreateResponse(HttpStatusCode.BadRequest, ModelState);

            return Request.CreateResponse(HttpStatusCode.OK);
        }
    }
}