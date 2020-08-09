using SocialMedia.Core.CustomEntities;

namespace SocialMedia.Api.Responses
{
    public class ApiResponse<T>
    {
        public ApiResponse(T data)
        {
            Data = data;
        }
        public T Data { get; set; }

        public MetaDate Meta { get; set; }
    }
}
