using MediatR;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Ordering.Domain.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ordering.Infrastructure.Data.Interceptors
{
    public class DispatchDomainEventsInterceptor(IMediator mediator) : SaveChangesInterceptor
    {
        public override InterceptionResult<int> SavingChanges(DbContextEventData eventData, InterceptionResult<int> result)
        {
            DispatchDomainEvents(eventData.Context).GetAwaiter().GetResult();
            return base.SavingChanges(eventData, result);
        }



        public override async ValueTask<InterceptionResult<int>> SavingChangesAsync(DbContextEventData eventData, InterceptionResult<int> result, CancellationToken cancellationToken = default)
        {
            await DispatchDomainEvents(eventData.Context);
            return await base.SavingChangesAsync(eventData, result, cancellationToken);
        }

        private async Task DispatchDomainEvents(DbContext? context)
        {
            if (context == null) return;
            //get all aggregates that have domain events
            var aggregates = context.ChangeTracker
                .Entries<IAggregate>()
                .Where(x => x.Entity.DomainEvents.Any())
                .Select(x => x.Entity);
            //get all domain events from aggreagtes
            var domainEvents = aggregates
                .SelectMany(x => x.DomainEvents)
                .ToList();
            //clear domain events that will be published
            aggregates.ToList().ForEach(x => x.ClearDomainEvents());

            //publish or send events to all subscribed handlers like OrderCreatedEventHandler
            foreach (var domainEvent in domainEvents)
                await mediator.Publish(domainEvent);
        }
    }
}
