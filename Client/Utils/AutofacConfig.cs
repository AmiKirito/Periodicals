using Autofac;
using Autofac.Integration.Mvc;
using System.Web.Mvc;
using DAL.Repositories;
using BLL.IRepositories;
using DAL;
using BLL.Services;
using BLL.IServices;
using Serilog;
using System;
using Client.App_Start;
using Microsoft.AspNet.Identity.EntityFramework;
using DAL.ModelsEntities;
using Microsoft.AspNet.Identity.Owin;

namespace Client.Utils
{
    public class AutofacConfig
    {
        public static void ConfigureContainer() 
        {
            var builder = new ContainerBuilder();

            builder.RegisterControllers(typeof(MvcApplication).Assembly);
            builder.Register<ILogger>((c, p) =>
                {
                    return new LoggerConfiguration()
                    .WriteTo.RollingFile(
                        AppDomain.CurrentDomain.BaseDirectory.ToString() + "App_Data" + "\\" + "Logs" +
                                                "\\" + $"Log-{DateTime.UtcNow.ToShortDateString()}.txt"
                        ).CreateLogger();
                }).SingleInstance();

            builder.RegisterType<AppDbContext>().AsSelf();

            builder.RegisterType<AdminService>().As<IAdminService>();
            builder.RegisterType<AdminRepository>().As<IAdminRepository>();

            builder.RegisterType<PublisherService>().As<IPublisherService>();
            builder.RegisterType<PublisherRepository>().As<IPublisherRepository>();

            builder.RegisterType<SubscriptionService>().As<ISubscriptionService>();
            builder.RegisterType<SubscriptionRepository>().As<ISubscriptionRepository>();

            builder.RegisterType<AccountService>().As<IAccountService>();
            builder.RegisterType<AccountRepository>().As<IAccountRepository>();

            var container = builder.Build();
            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));
        }
    }
}