using System.Collections.Generic;
using System.Net.Http;
using System.Web.Http.Routing;

namespace IssueTracker.Attributes
{
    public abstract class HttpVerbConstrainedRouteAttribute : RouteFactoryAttribute
    {
        protected HttpVerbConstrainedRouteAttribute(string template, HttpMethod method)
            : base(template)
        {
            Method = method;
        }

        public HttpMethod Method { get; private set; }

        public override IDictionary<string, object> Constraints
        {
            get
            {
                return new HttpRouteValueDictionary
                {
                    {"method", new HttpVerbConstraint(Method)}
                };
            }
        }
    }

    public class GetRouteAttribute : HttpVerbConstrainedRouteAttribute
    {
        public GetRouteAttribute(string template) : base(template, HttpMethod.Get) { }
    }

    public class PostRouteAttribute : HttpVerbConstrainedRouteAttribute
    {
        public PostRouteAttribute(string template) : base(template, HttpMethod.Post) { }
    }

    public class DeleteRouteAttribute : HttpVerbConstrainedRouteAttribute
    {
        public DeleteRouteAttribute(string template) : base(template, HttpMethod.Delete) { }
    }

    public class PutRouteAttribute : HttpVerbConstrainedRouteAttribute
    {
        public PutRouteAttribute(string template) : base(template, HttpMethod.Put) { }
    }

    class HttpVerbConstraint : IHttpRouteConstraint
    {
        public HttpMethod Method { get; private set; }

        public HttpVerbConstraint(HttpMethod method)
        {
            Method = method;
        }

        public bool Match(HttpRequestMessage request, IHttpRoute route, string parameterName, IDictionary<string, object> values, HttpRouteDirection routeDirection)
        {
            return request.Method == Method;
        }
    }
}