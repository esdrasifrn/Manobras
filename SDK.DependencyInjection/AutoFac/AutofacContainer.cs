using Autofac;
using Autofac.Core;
using SDK.DependencyInjection.Helpers.Extensions;
using SDK.DependencyInjection.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SDK.DependencyInjection.AutoFac
{
    public class AutofacContainer : ISdkContainer, IDisposable
    {
        private bool disposedValue;
        protected ILifetimeScope _container;

        public AutofacContainer(ILifetimeScope container)
        {
            _container = container;
        }

        public ISdkContainer GetChildContainer()
        {
            return new AutofacContainer(_container.BeginLifetimeScope());
        }

        public T Resolve<T>()
        {
            var service = _container.Resolve<T>();
            return service;
        }

        public T Resolve<T>(string name)
        {
            var service = _container.ResolveNamed<T>(name);
            return service;
        }

        public IEnumerable<T> ResolveAll<T>() where T : class
        {
            var types = _container.ComponentRegistry.Registrations.Where(r => typeof(T).IsAssignableFrom(r.Activator.LimitType))
                                                                       .DistinctBy(x => x.Activator.LimitType);

            var instances = types.Select(t =>
            {
                var namedService = t.Services?.ToList()?[0] as KeyedService;
                if (namedService != null)
                    return Resolve<T>(namedService.ServiceKey.ToString());

                return _container.Resolve(t.Activator.LimitType) as T;
            }).ToList();

            return instances;
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                _container.Dispose();
                _container = null;
                if (disposing)
                {
                    // TODO: dispose managed state (managed objects)
                }

                disposedValue = true;
            }
        }

        public void Dispose()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}