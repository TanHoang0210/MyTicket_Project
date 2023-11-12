using Microsoft.EntityFrameworkCore;
using MYTICKET.UTILS.ConstantVariables.User;
using MYTICKET.UTILS.Security;
using MYTICKET.WEB.DOMAIN.Entities;

namespace MYTICKET.WEB.Infrastructure.Persistence
{
    public static class MyTicketDbContextExtension
    {
        public static void SeedData(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().HasData(
                new User { Id = 1, Username = "admin", Password = CryptographyUtils.CreateMD5("123qwe"), UserType = UserTypes.ADMIN, Status = UserStatus.ACTIVE });
        }
    }
}
