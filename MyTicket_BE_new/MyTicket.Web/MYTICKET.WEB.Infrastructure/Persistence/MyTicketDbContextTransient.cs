using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace MYTICKET.WEB.Infrastructure.Persistence
{
    public class MyTicketDbContextTransient : MyTicketDbContext
    {
        public MyTicketDbContextTransient(DbContextOptions<MyTicketDbContext> options, IHttpContextAccessor httpContextAccessor) : base(options, httpContextAccessor)
        {
        }
    }
}
