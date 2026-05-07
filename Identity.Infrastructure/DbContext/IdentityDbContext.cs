using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using SharedInfrastructure.BaseDbContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace Identity.Infrastructure.DbContext
{
    public class IdentityDbContext : BaseDbContext
    {
        public IdentityDbContext(DbContextOptions options, IHttpContextAccessor httpContextAccessor, string schema = "dbo") : base(options, httpContextAccessor, schema)
        {
        }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.ApplyConfigurationsFromAssembly(typeof(IdentityDbContext).Assembly);
        }
    }
}
