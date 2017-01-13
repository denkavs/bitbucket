using EFolderDomain.Infrastructure;
using EFolderDomain.Interfaces;
using EFolderDomain.Plugins;
using EFolderDomain.ServiceLayer;
using Ninject;
using Ninject.Extensions.ChildKernel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http.Dependencies;

namespace ConsoleBackUp.Infrastructure
{
    public class NinjectResolver : IDependencyResolver
    {
        private IKernel kernel;
        public static IConfigurationMgr ConfMgr;

        public NinjectResolver() : this(new StandardKernel()) {}
        public NinjectResolver(IKernel ninjectKernel, bool scope = false)
        {
            kernel = ninjectKernel;
            if (!scope)
            {
                AddBindings(kernel);
            }
        }
        public IDependencyScope BeginScope()
        {
            return new NinjectResolver(AddRequestBindings(new ChildKernel(kernel)), true);
        }
        public object GetService(Type serviceType)
        {
            return kernel.TryGet(serviceType);
        }
        public IEnumerable<object> GetServices(Type serviceType)
        {
            return kernel.GetAll(serviceType);
        }
        public void Dispose()
        {
            // do nothing
        }
        private void AddBindings(IKernel kernel)
        {
            // singleton and transient bindings go here
            kernel.Bind<CircuitBreaker<Stream>>().To<CircuitBreaker<Stream>>().InSingletonScope();
            kernel.Bind<IBackupRepository>().To<BackupRepository>().InSingletonScope();

            kernel.Bind<IToDoItemFetcher>().To<ToDoItemFetcher>().InTransientScope().WithConstructorArgument("baseUri", NinjectResolver.ConfMgr.GetToDoItemServiceUri())
                                                                                    .WithConstructorArgument("query", NinjectResolver.ConfMgr.GetToDoItemServicePath());
            kernel.Bind<IToDoRepository>().To<ToDoRepository>();
            kernel.Bind<IToDoItemStreamParser>().To<ToDoItemStreamParser>();

            kernel.Bind<IBackupService>().To<BackupService>().InTransientScope();
        }
        private IKernel AddRequestBindings(IKernel kernel)
        {
            // request object scope bindings go here
            return kernel;
        }
    }
}
