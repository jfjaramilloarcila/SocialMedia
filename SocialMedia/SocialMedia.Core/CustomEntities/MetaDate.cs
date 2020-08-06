using System;
using System.Collections.Generic;
using System.Text;

namespace SocialMedia.Core.CustomEntities
{
    public class MetaDate
    {
        public int TotalCount { get; set; }
        public int PageSize { get; set; }
        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }
        public int HasNextPage { get; set; }
        public int HasPreviousPage { get; set; }
        public int NextPageUrl { get; set; }
        public int PreviousPageUrl { get; set; }

    }
}
