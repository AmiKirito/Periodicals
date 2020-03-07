using Autofac;
using Autofac.Integration.Mvc;
using System.Web.Mvc;
using DAL.Repositories;
using BLL.IRepositories;
using DAL;
using BLL.Services;
using BLL.IServices;

namespace Client.Utils
{
    public class AutofacConfig
    {
        public static void ConfigureContainer() 
        {
            var builder = new ContainerBuilder();

            builder.RegisterControllers(typeof(MvcApplication).Assembly);
            builder.RegisterType<AppDbContext>().AsSelf();

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