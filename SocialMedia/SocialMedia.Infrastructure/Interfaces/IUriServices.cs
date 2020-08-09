using SocialMedia.Core.QueryFilters;
using System;

namespace SocialMedia.Infrastructure.Interfacaes
{
    public interface IUriServices
    {
        Uri GestPostPaginationUri(PostQueryFilter filter, string actionUrl);
    }
}