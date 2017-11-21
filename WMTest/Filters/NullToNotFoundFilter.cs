using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Filters;

namespace WMTest.Filters
{

    public class NullToNotFoundFilter : ActionFilterAttribute
    {
        public override void OnActionExecuted(HttpActionExecutedContext actionExecutedContext)
        {
            var response = actionExecutedContext.Response;

            object responseValue;
            bool hasContent = response.TryGetContentValue(out responseValue);

            if (!hasContent)
                throw new HttpResponseException(HttpStatusCode.NotFound);
        }
    }
}