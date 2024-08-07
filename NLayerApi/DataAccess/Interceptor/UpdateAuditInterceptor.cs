using DataAccess.Entities;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace DataAccess.Interceptor
{
    public sealed class UpdateAuditInterceptor : SaveChangesInterceptor
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UpdateAuditInterceptor(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public override ValueTask<InterceptionResult<int>> SavingChangesAsync(DbContextEventData eventData, InterceptionResult<int> result,
            CancellationToken cancellationToken = new CancellationToken())
        {
            if (eventData.Context is not null)
            {
                UpdateAuditableEntities(eventData.Context);
            }
            return base.SavingChangesAsync(eventData, result, cancellationToken);
        }


        public override InterceptionResult<int> SavingChanges(DbContextEventData eventData, InterceptionResult<int> result)
        {
            if (eventData.Context is not null)
            {
                UpdateAuditableEntities(eventData.Context);
            }
            return base.SavingChanges(eventData, result);
        }

        private void UpdateAuditableEntities(DbContext eventDataContext)
        {
            DateTime utcNow = DateTime.Now;
            var entities = eventDataContext.ChangeTracker.Entries<Audit>().ToList();
            var user = _httpContextAccessor.HttpContext.User.Identity.Name;
            foreach (var entry in entities)
            {
                if (entry.State == EntityState.Added)
                {
                    entry.Property(s => s.CreatedBy).CurrentValue = user;
                    entry.Property(s => s.CreatedDate).CurrentValue = utcNow;

                }

                if (entry.State == EntityState.Modified)
                {
                    entry.Property(s => s.UpdatedBy).CurrentValue = user;
                    entry.Property(s => s.UpdatedDate).CurrentValue = utcNow;
                }
            }
        }
    }
}