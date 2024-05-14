using ETLMegaStore.Models;
using Microsoft.EntityFrameworkCore;

namespace ETLMegaStore.Services
{
    public class OriginDBContext : DbContext
    {
        public DbSet<SalesOrder> SalesOrders { get; set; }

        public OriginDBContext(DbContextOptions options) : base(options)
        {
        }
    }
}
