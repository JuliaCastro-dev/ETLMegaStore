using ETLMegaStore.Models;
using Microsoft.EntityFrameworkCore;

namespace ETLMegaStore.Services
{
    public class DestinyDBContext : DbContext
    {
        public DbSet<SalesOrder> SalesOrders { get; set; }
        public DestinyDBContext(DbContextOptions<DestinyDBContext> options) : base(options) { 
        
        }
    }
}
