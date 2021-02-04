using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VNQuiz.Alice
{
    public class HandleFailedRequestsMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<HandleFailedRequestsMiddleware> _logger;
        public HandleFailedRequestsMiddleware(RequestDelegate next, ILoggerFactory loggerFactory)
        {
            _next = next;
            _logger = loggerFactory?.CreateLogger<HandleFailedRequestsMiddleware>() ??
            throw new ArgumentNullException(nameof(loggerFactory));
        }
        public async Task InvokeAsync(HttpContext context)
        {
            string request = await FormatRequestAsync(context.Request).ConfigureAwait(false);

            await _next(context);

            if(context.Response.StatusCode != 200 
                && context.Response.StatusCode != 404)
            {
                _logger.LogError($"Not successful {context.Response.StatusCode} response returned from request: {request}");
            }
        }

        private static async Task<string> FormatRequestAsync(HttpRequest request)
        {
            string requestBody = "";

            request.EnableBuffering();

            // Leave the body open so the next middleware can read it.
            using (var reader = new StreamReader(
                request.Body,
                encoding: Encoding.UTF8,
                detectEncodingFromByteOrderMarks: false,
                bufferSize: -1,
                leaveOpen: true))
            {
                requestBody = await reader.ReadToEndAsync();
                // Do some processing with body…

                // Reset the request body stream position so the next middleware can read it
                request.Body.Position = 0;
            }
            return requestBody;
        }
    }
}
