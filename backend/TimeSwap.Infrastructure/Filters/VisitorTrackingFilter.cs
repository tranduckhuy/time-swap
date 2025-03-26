using Microsoft.AspNetCore.Mvc.Filters;
using TimeSwap.Application.Visistor;

namespace TimeSwap.Infrastructure.Filters
{
    public class VisitorTrackingFilter : IAsyncActionFilter
    {
        private readonly IVisitorService _visitorService;

        public VisitorTrackingFilter(IVisitorService visitorService)
        {
            _visitorService = visitorService;
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            await _visitorService.IncreaseVisitCount();

            await next();
        }
    }

}
