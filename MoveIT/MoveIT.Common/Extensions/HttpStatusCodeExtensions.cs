using System.Net;
using static MoveIT.Common.Constants;

namespace MoveIT.Common.Extensions
{
    public static class HttpStatusCodeExtensions
    {
        public static string ToMessage(this HttpStatusCode code)
        {
            switch (code)
            {
                case HttpStatusCode.BadRequest:
                    return BAD_REQUEST_ERROR_MESSAGE;
                case HttpStatusCode.Forbidden:
                    return FORBIDDEN_ERROR_MESSAGE;
                case HttpStatusCode.NotFound:
                    return NOT_FOUND_ERROR_MESSAGE;
                case HttpStatusCode.InternalServerError:
                    return INTERNAL_ERROR_MESSAGE;
                default:
                    return UNEXPECTED_ERROR_MESSAGE;
            }
        }
    }
}
