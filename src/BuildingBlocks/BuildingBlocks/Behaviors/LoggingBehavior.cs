using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BuildingBlocks.Behaviors
{
    public class LoggingBehavior<TRequest, TResponse>(ILogger<LoggingBehavior<TRequest, TResponse>> logger)
        : IPipelineBehavior<TRequest, TResponse>
        where TRequest : notnull, IRequest<TResponse>
        where TResponse : notnull
    {
        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            logger.LogInformation($"[START] handle requestName={typeof(TRequest).Name} - ResponseName={typeof(TResponse).Name} and Request={request}");

            var timer = new Stopwatch();
            timer.Start();

            var response = await next();

            timer.Stop();
            var timeTaken=timer.Elapsed;
            if(timeTaken.Seconds>3)
            {
                logger.LogWarning($"[PERFORMANCE] the request={typeof(TRequest).Name} took {timeTaken}");
            }

            logger.LogInformation($"[END] handled request={typeof(TRequest).Name} with response ={typeof(TResponse).Name} ");
            return response;
        }
    }
}
