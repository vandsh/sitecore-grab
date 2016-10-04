using System;
using System.Collections.Generic;
using System.Reflection;
using Nancy;
using Nancy.Bootstrapper;
using Nancy.TinyIoc;
using Nancy.ViewEngines;
using Sitecore.Grab.Classes.Contracts.Classes;
using Sitecore.Grab.Classes.Domain;
using Sitecore.Grab.SerializedPackage;

namespace Sitecore.Grab
{
    public class DefaultGrabBootstrapper : DefaultNancyBootstrapper
    {
        protected override void ApplicationStartup(TinyIoCContainer container, IPipelines pipelines)
        {
            base.ApplicationStartup(container, pipelines);

            Nancy.Json.JsonSettings.MaxJsonLength = int.MaxValue;
        }

        protected override void ConfigureConventions(Nancy.Conventions.NancyConventions nancyConventions)
        {
            base.ConfigureConventions(nancyConventions);
            nancyConventions.ViewLocationConventions.Add(_viewNameResolver);
        }

        private string _viewNameResolver(string viewName, dynamic model, ViewLocationContext context)
        {
            var viewLocation = "";

            viewLocation = string.Format("Views/{0}/{1}", context.ModuleName, viewName.ToLower());

            return viewLocation;
        }

        protected override void ConfigureApplicationContainer(TinyIoCContainer container)
        {
            container.AutoRegister(new[]
            {
                typeof (GenerateItemModule).Assembly,
            });

            container.Register(typeof(PackegeExportSettings), new PackageExportConfigurationProvider().Settings);

            //var assembly = GetType().Assembly;
            //ResourceViewLocationProvider
            //    .RootNamespaces
            //    .Add(assembly, "Sitecore.Grab.About");
        }

        protected override IEnumerable<Func<Assembly, bool>> AutoRegisterIgnoredAssemblies
        {
            get
            {
                var ignoredAssemblies = new List<Func<Assembly, bool>>();
                ignoredAssemblies.AddRange(base.AutoRegisterIgnoredAssemblies);
                ignoredAssemblies.AddRange(
                    new Func<Assembly, bool>[]
                        {
                            asm => asm.FullName.StartsWith("Oracle", StringComparison.InvariantCulture),
                            asm => asm.FullName.StartsWith("Sitecore", StringComparison.InvariantCulture)
                        }
                    );

                ignoredAssemblies.Remove(asm => !asm.FullName.StartsWith("Sitecore.Grab", StringComparison.InvariantCulture));

                return ignoredAssemblies;
            }
        }

        protected override NancyInternalConfiguration InternalConfiguration
        {
            get
            {
                return NancyInternalConfiguration.WithOverrides(OnConfigurationBuilder);
            }
        }

        void OnConfigurationBuilder(NancyInternalConfiguration x)
        {
            x.ViewLocationProvider = typeof(ResourceViewLocationProvider);
        }
    }
}
