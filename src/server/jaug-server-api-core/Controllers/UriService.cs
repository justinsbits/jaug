using jaug_server_api_core.Controllers;
using Microsoft.AspNetCore.WebUtilities;
using System;

namespace jaug_server_api_core.Controllers
{
    public class UriService : IUriService
    {
        private readonly string _baseUri;
        public UriService(string baseUri)
        {
            _baseUri = baseUri;
        }
        public Uri GetPageUri(PagedResourceParameters pagedResParams, string route)
        {
            var _enpointUri = new Uri(string.Concat(_baseUri ,route));
            var modifiedUri = QueryHelpers.AddQueryString(_enpointUri.ToString(), "pageNumber", pagedResParams.PageNumber.ToString());
            modifiedUri = QueryHelpers.AddQueryString(modifiedUri, "pageSize", pagedResParams.PageSize.ToString());
            return new Uri(modifiedUri);
        }
    }
}
