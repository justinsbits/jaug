using System;

namespace jaug_server_api_core.Controllers
{
    public interface IUriService
    {
        public Uri GetPageUri(PagedResourceParameters pagedResParams, string route);
    }
}
