using System;
using System.Collections.Generic;
using System.Text;

namespace SDK.DependencyInjection.Interfaces
{
    public interface ISdkContainer : IDisposable
    {
        T Resolve<T>();
        T Resolve<T>(string name);
        IEnumerable<T> ResolveAll<T>() where T : class;
        ISdkContainer GetChildContainer();
    }
}
