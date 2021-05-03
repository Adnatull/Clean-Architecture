using System.Collections.Generic;

namespace Core.Domain.Persistence.Response
{
    public class PersistenceResponse<T>
    {
        public bool Succeeded { get; protected set; }
        public T Data { get; protected set; }
        public string Message { get; protected set; }
        public List<string> Errors { get; set; }

        public static PersistenceResponse<T> Success()
        {
            var result = new PersistenceResponse<T> {Succeeded = true};
            return result;
        }
        public static PersistenceResponse<T> Success(string message)
        {
            var result = new PersistenceResponse<T> { Succeeded = true, Message  = message};
            return result;
        }

        public static PersistenceResponse<T> Success(T data, string message)
        {
            var result = new PersistenceResponse<T> { Succeeded = true, Data = data, Message = message };
            return result;
        }

        public static PersistenceResponse<T> Fail()
        {
            var result = new PersistenceResponse<T> { Succeeded = false };
            return result;
        }

        public static PersistenceResponse<T> Fail(string message)
        {
            var result = new PersistenceResponse<T> { Succeeded = false, Message = message};
            return result;
        }

        public static PersistenceResponse<T> Fail(string message, List<string> errors)
        {
            var result = new PersistenceResponse<T> { Succeeded = false, Message = message, Errors = errors};
            return result;
        }

        public static PersistenceResponse<T> Fail(List<string> errors)
        {
            var result = new PersistenceResponse<T> { Succeeded = false, Errors = errors };
            return result;
        }

        public override string ToString()
        {
            return Succeeded ? Message : Errors == null || Errors.Count == 0 ? Message : $"{Message} : {string.Join(",", Errors)}";
        }
    }
}
