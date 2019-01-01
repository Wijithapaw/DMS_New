using DMS.Domain;

namespace DMS.WebApi
{
    public class RequestContext : IRequestContext
    {
        public int UserId { get; set; }
    }
}
