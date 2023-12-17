using MYTICKET.BASE.API;
using MYTICKET.BASE.IdentityServer.StartUp;
using MYTICKET.UTILS.Localization;
using MYTICKET.WEB.API.MiddleWare;
using MYTICKET.WEB.Infrastructure.Persistence;
using MYTICKET.WEB.SERVICE;
using MYTICKET.WEB.SERVICE.AuthModule.Abstracts;
using MYTICKET.WEB.SERVICE.AuthModule.Implements;
using MYTICKET.WEB.SERVICE.Common.Localization;
using MYTICKET.WEB.SERVICE.EventModule.Abstracts;
using MYTICKET.WEB.SERVICE.EventModule.Implements;
using MYTICKET.WEB.SERVICE.EventTypeModule.Abstracts;
using MYTICKET.WEB.SERVICE.EventTypeModule.Implements;
using MYTICKET.WEB.SERVICE.FileModule.Abstracts;
using MYTICKET.WEB.SERVICE.FileModule.Implements;
using MYTICKET.WEB.SERVICE.MailService.Abstracts;
using MYTICKET.WEB.SERVICE.MailService.Implements;
using MYTICKET.WEB.SERVICE.OrderModule.Abstracts;
using MYTICKET.WEB.SERVICE.OrderModule.Implements;
using MYTICKET.WEB.SERVICE.TicketModule.Abstracts;
using MYTICKET.WEB.SERVICE.TicketModule.Implements;
using MYTICKET.WEB.SERVICE.VenueModule.Abstracts;
using MYTICKET.WEB.SERVICE.VenueModule.Implements;
using MYTICKET.WEB.SERVICE.VnPayService.Abstracts;
using MYTICKET.WEB.SERVICE.VnPayService.Implements;

namespace MyTicket.API

{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.ConfigureCors();
            builder.ConfigureServices(isIdentityServer: true);
            builder.Services.AddCommonIdentityServer<MyTicketDbContext>(builder.Configuration);
            builder.Services.AddSingleton<LocalizationBase, JVFLocalization>();
            builder.Services.AddSingleton<MapErrorCodeBase>();


            builder.Services.AddScoped<IUserService, UserService>();
            builder.Services.AddScoped<IPermissionService, PermissionService>();
            builder.Services.AddScoped<IRoleService, RoleService>();
            builder.Services.AddScoped<ICustomerService, CustomerService>();
            builder.Services.AddScoped<ISupplierService, SupplierService>();
            builder.Services.AddScoped<IFileService, FileService>();
            builder.Services.AddScoped<IVenueService, VenueService>();
            builder.Services.AddScoped<IEventTypeService, EventTypeService>();
            builder.Services.AddScoped<IEventService, EventService>();
            builder.Services.AddScoped<IOrderService, OrderService>();
            builder.Services.AddScoped<IVnpayService, VnpayService>();
            builder.Services.AddScoped<IEmailSenderService, EmailSenderService>();
            builder.Services.AddScoped<ITicketService, TicketService>();


            var app = builder.Build();
            app.Configure();
            app.UseCheckUser();
            app.ConfigureEndpoint();
            app.Run();
            //app.TestResolveService(builder.Services);
        }
    }
}