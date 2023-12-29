using Hangfire;
using Microsoft.AspNetCore.Authentication.Cookies;
using MYTICKET.BASE.API;
using MYTICKET.BASE.API.Filters;
using MYTICKET.BASE.IdentityServer.StartUp;
using MYTICKET.UTILS.ConstantVaribale.Db;
using MYTICKET.UTILS.Localization;
using MYTICKET.WEB.API.MiddleWare;
using MYTICKET.WEB.Infrastructure.Persistence;
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
using MYTICKET.WEB.SERVICE.Shared.Authorization;
using MYTICKET.WEB.SERVICE.SystemModule.Abstracts;
using MYTICKET.WEB.SERVICE.SystemModule.Implements;
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
            builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
            .AddCookie(c =>
            {
                c.LoginPath = AuthenticationPath.AuthenticateLogin;
            });
            string connectionString = builder.Configuration.GetConnectionString("Default")
                ?? throw new InvalidOperationException("Không tìm thấy connection string \"Default\" trong appsettings.json");
            builder.ConfigureCors();
            builder.ConfigureServices(isIdentityServer: true);
            builder.ConfigureHangfire(connectionString, DbSchemas.Default);
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
            builder.Services.AddScoped<ISystemService, SystemService>();


            var app = builder.Build();
            using (var scope = app.Services.CreateScope())
            {
                var backgroundJobService = scope.ServiceProvider.GetRequiredService<ISystemService>();
                //Chúc sinh nhật
                RecurringJob.AddOrUpdate("happy-birth-day-customer", () => backgroundJobService.HappyBirthDayCustomerNotification(), "15 3 * * *");

                //RecurringJob.AddOrUpdate("not-refund-order", () => backgroundJobService.NotRefundNotificationForAdmin(), "0 8 * * *");

                //RecurringJob.AddOrUpdate("not-refund-transfer", () => backgroundJobService.NotRefundTransferNotificationForAdmin(), "0 9 * * *");

                //RecurringJob.AddOrUpdate("not-refund-exchange", () => backgroundJobService.NotRefundExchangeNotificationForAdmin(), "0 10 * * *");

            }
            app.Configure();
            app.UseCheckUser();
            app.ConfigureEndpoint();
            app.UseHangfireDashboard("/api/hangfire", new DashboardOptions
            {
                Authorization = new[] { new HangfireAuthorizationFilter() }
            });
            app.Run();
        }
    }
}