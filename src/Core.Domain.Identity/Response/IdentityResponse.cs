using System.Collections.Generic;

namespace Core.Domain.Identity.Response
{
    public class IdentityResponse
    {
        public bool Succeeded { get; protected set; }

        public string Message { get; protected set; }
        public List<string> Errors { get; set; }

        public static IdentityResponse Success()
        {
            var result = new IdentityResponse {Succeeded = true};
            return result;
        }
        public static IdentityResponse Success(string message)
        {
            var result = new IdentityResponse { Succeeded = true, Message  = message};
            return result;
        }

      

        public static IdentityResponse Fail()
        {
            var result = new IdentityResponse { Succeeded = false };
            return result;
        }

        public static IdentityResponse Fail(string message)
        {
            var result = new IdentityResponse { Succeeded = false, Message = message};
            return result;
        }

        public static IdentityResponse Fail(string message, List<string> errors)
        {
            var result = new IdentityResponse { Succeeded = false, Message = message, Errors = errors};
            return result;
        }

        public static IdentityResponse Fail(List<string> errors)
        {
            var result = new IdentityResponse { Succeeded = false, Errors = errors };
            return result;
        }

        public override string ToString()
        {
            return Succeeded ? Message : Errors == null || Errors.Count == 0 ? Message : $"{Message} : {string.Join(",", Errors)}";
        }
    }
}
