using System;
using System.Collections.Generic;
using System.Web;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using Autofac;
using System.Reflection;
using Conseg.Administracao.DataAccessLayer.dbContext;
using Conseg.Administracao.Domain.Core;
using System.Web.Mvc;
using AutoMapper;
using Owin;
using Autofac.Integration.Mvc;
using Autofac.Integration.WebApi;
using Administracao.GenericController;

namespace Conseg.Administracao.Framework
{
    public partial class Startup
    {
        public static IContainer _container;
        public static AssemblyService _assemblyService;

        public static void Configure(IAppBuilder app, HttpConfiguration config, StartupConfiguration startupConfiguration)
        {

            _assemblyService = new AssemblyService();
            _assemblyService.LoadAssemblies();

            var builder = new ContainerBuilder();

            builder.RegisterInstance(_assemblyService).As<IAssemblyService>().SingleInstance();

            foreach (Assembly ass in _assemblyService.Assemblies)
            {
                // Register your MVC controllers.
                if (startupConfiguration.UseMvc)
                {
                    //builder.RegisterControllers(ass).AsImplementedInterfaces().InstancePerHttpRequest();
                    builder.RegisterControllers(ass).AsImplementedInterfaces().InstancePerRequest();
                    builder.RegisterModelBinders(ass);
                }

                // Register your WEBAPI controllers.
                if (startupConfiguration.UseWebapi)
                {
                    builder.RegisterApiControllers(ass).InstancePerRequest();
                }

                //if (startupConfiguration.UseGenericAutoMapper)
                //{

                //    builder.RegisterAssemblyTypes(ass).AssignableTo(typeof(Profile)).As<Profile>();
                //}

                //Registrando o repositorio e os servicos
                builder.RegisterAssemblyTypes(ass).AsSelf();
                builder.RegisterAssemblyTypes(ass).AsImplementedInterfaces();
            }


            //OWIN Authentication
            //builder.Register<IAuthenticationManager>(c => HttpContext.Current.GetOwinContext().Authentication).InstancePerRequest();
            //builder.Register<IOwinContext>(c => HttpContext.Current.GetOwinContext()).InstancePerRequest();

            builder.Register<IdbContext>(c => new dbContext(startupConfiguration.NameOrConnectionString)).InstancePerLifetimeScope();
            builder.RegisterGeneric(typeof(EFRrepository<>)).As(typeof(IRepository<>)).InstancePerLifetimeScope();

            builder.Register(s => new ControllerFactory(_assemblyService)).As<IControllerFactory>();

            builder.RegisterGeneric(typeof(GenericController<,>)).AsSelf().InstancePerLifetimeScope();
            //builder.RegisterGeneric(typeof(GerenicApiController<,>)).AsSelf().InstancePerLifetimeScope();

            //if (startupConfiguration.UseGenericODataController)
            //{
            //    builder.RegisterGeneric(typeof(GenericODataController<>)).AsSelf().InstancePerLifetimeScope();
            //}

            if (startupConfiguration.UseGenericAutoMapper)
            {
                // builder.RegisterGeneric(typeof(AutoMapperGeneric<,>)).AsSelf().InstancePerLifetimeScope();


                builder.RegisterAssemblyTypes().AssignableTo(typeof(Profile)).As<Profile>();
                builder.Register(c => new MapperConfiguration(cfg =>
                {
                    foreach (var entity in _assemblyService.Entities)
                    {
                        var model = _assemblyService.Models.FirstOrDefault(x => x.Name == entity.Name + "Model");

                        if (model != null)
                        {
                            cfg.CreateMap(model, entity);
                            cfg.CreateMap(entity, model);
                        }
                    }
                    //cfg.CreateMap<>();
                    //foreach (var profile in c.Resolve<IEnumerable<Profile>>())
                    //{
                    //    cfg.AddProfile(profile);
                    //}
                })).AsSelf().SingleInstance();

                builder.Register(c => c.Resolve<MapperConfiguration>().CreateMapper(c.Resolve)).As<IMapper>().InstancePerLifetimeScope();

            }

            builder.RegisterInstance(config).As<HttpConfiguration>().SingleInstance();

            builder.RegisterType<AutofacDependencyResolver>()
            .AsSelf()
            .InstancePerLifetimeScope();

            // builder.Update(container);

            //register dependencies provided by other assemblies
            //builder = new ContainerBuilder();
            //var drTypes = typeFinder.FindClassesOfType<IDependencyRegistrar>();
            //var drInstances = new List<IDependencyRegistrar>();
            //foreach (var drType in drTypes)
            //    drInstances.Add((IDependencyRegistrar)Activator.CreateInstance(drType));
            ////sort
            //drInstances = drInstances.AsQueryable().OrderBy(t => t.Order).ToList();
            //foreach (var dependencyRegistrar in drInstances)
            //    dependencyRegistrar.Register(builder, typeFinder, new SSPConfig());

            //builder.Register<IDataProtectionProvider>(c => app.GetDataProtectionProvider()).InstancePerRequest();

            //builder.RegisterType<UserService>().As<IUserService>().InstancePerRequest();
            //builder.RegisterType<Service.IdentityAuthenticationService>().As<IAuthenticationService>().InstancePerRequest();
            //builder.RegisterType<Service.AccountService>().AsSelf().InstancePerRequest();
            //builder.RegisterType<Service.IdentityAuthenticationService>().AsSelf().InstancePerRequest();

            //builder.Register<IDbContext>(c => new ZCObjectContext("Data Source=GS0390279/SQL2014;Initial Catalog=TesteSecurity;Integrated Security=True;Persist Security Info=False")).InstancePerLifetimeScope();
            //builder.RegisterGeneric(typeof(EfRepository<>)).As(typeof(IRepository<>)).InstancePerLifetimeScope();

            // Run other optional steps, like registering model binders,
            // web abstractions, etc., then set the dependency resolver
            // to be Autofac.


            // OPTIONAL: Register model binders that require DI.            
            //builder.RegisterModelBinders(typeof(MvcApplication).Assembly);
            builder.RegisterModelBinderProvider();
            // OPTIONAL: Register web abstractions like HttpContextBase.
            builder.RegisterModule<AutofacWebTypesModule>();
            // OPTIONAL: Enable property injection in view pages.
            builder.RegisterSource(new ViewRegistrationSource());
            // OPTIONAL: Enable property injection into action filters.
            builder.RegisterFilterProvider();

            var container = builder.Build();

            _container = container;

            if (startupConfiguration.UseMvc)
                DependencyResolver.SetResolver(new AutofacDependencyResolver(container));

            if (startupConfiguration.UseWebapi)
                config.DependencyResolver = new AutofacWebApiDependencyResolver(container);

            //EngineContext.Initialize(false);
            //EngineContext.Current.ContainerManager = new ContainerManager(container);

            // OWIN MVC SETUP:
            // Register the Autofac middleware FIRST, then the Autofac MVC middleware.
            app.UseAutofacMiddleware(container);

            if (startupConfiguration.UseMvc)
            {
                app.UseAutofacMvc();
            }

            if (startupConfiguration.UseWebapi)
            {
                //app.UseAutofacWebApi(config);
            }

            // ...then register your other middleware not registered
            // with Autofac.
            //app.SetDefaultSignInAsAuthenticationType(CookieAuthenticationDefaults.AuthenticationType);

            //if (startupConfiguration.UseCookieAuthentication)
            //{
            //    app.UseCookieAuthentication(startupConfiguration.CookieAuthenticationOptions);
            //}

            if (startupConfiguration.UseExternalSignInCookie)
            {
                //app.UseExternalSignInCookie(DefaultAuthenticationTypes.ExternalCookie);
            }
            // Enables the application to temporarily store user information when they are verifying the second factor in the two-factor authentication process.
            //app.UseTwoFactorSignInCookie(DefaultAuthenticationTypes.TwoFactorCookie, TimeSpan.FromMinutes(5));

            //// Enables the application to remember the second login verification factor such as phone or email.
            //// Once you check this option, your second step of verification during the login process will be remembered on the device where you logged in from.
            //// This is similar to the RememberMe option when you log in.
            //app.UseTwoFactorRememberBrowserCookie(DefaultAuthenticationTypes.TwoFactorRememberBrowserCookie);

            //if (startupConfiguration.UseMicrosoftAccountAuthentication)
            //{
            //    // Uncomment the following lines to enable logging in with third party login providers
            //    app.UseMicrosoftAccountAuthentication(
            //    clientId: startupConfiguration.MicrosoftClientId,
            //    clientSecret: startupConfiguration.MicrosoftClientSecret);
            //}
            //if (startupConfiguration.UseTwitterAuthentication)
            //{
            //    app.UseTwitterAuthentication(
            //   consumerKey: startupConfiguration.TwitterConsumerKey,
            //   consumerSecret: startupConfiguration.TwitterConsumerSecret);
            //}
            //if (startupConfiguration.UseFacebookAuthentication)
            //{
            //    app.UseFacebookAuthentication(
            //   appId: startupConfiguration.FacebookAppId,
            //   appSecret: startupConfiguration.FacebookAppSecret);
            //}
            //if (startupConfiguration.UseGoogleAuthentication)
            //{
            //    app.UseGoogleAuthentication(new GoogleOAuth2AuthenticationOptions()
            //    {
            //        ClientId = startupConfiguration.GoogleClientId,
            //        ClientSecret = startupConfiguration.GoogleClientSecret
            //    });
            //}

            //Execução de atividades no start da aplicação
            var startUpTaskTypes = _assemblyService.StartupTasks;
            var startUpTasks = new List<IStartupTask>();
            foreach (var startUpTaskType in _assemblyService.StartupTasks)
            {
                startUpTasks.Add((IStartupTask)Activator.CreateInstance(startUpTaskType));
            }
            foreach (var startUpTask in startUpTasks)
            {
                startUpTask.Execute(container);
            }


        }



    }
}
