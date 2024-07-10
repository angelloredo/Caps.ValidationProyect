using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace CapsValidationProyect.Application.Exceptions
{
    public class HandlerException : Exception
    {
        public HttpStatusCode Code { get; }
        public object Errors { get; }
        public HandlerException(HttpStatusCode code, object errors = null)
        {
            Code = code;
            Errors = errors;
        }
    }
}
