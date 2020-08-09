using SocialMedia.Core.QueryFilters;
using SocialMedia.Infrastructure.Interfacaes;
using System;

namespace SocialMedia.Infrastructure.Services
{
    public class UriServices : IUriServices
    {
        private readonly string _baseUri;
        public UriServices(string baseUri)
        {
            _baseUri = baseUri;
        }

        public Uri GestPostPaginationUri(PostQueryFilter filter, string actionUrl)
        {
            string baseUrl = $"{_baseUri}{actionUrl}";
            return new Uri(baseUrl);
        }

    }
}
