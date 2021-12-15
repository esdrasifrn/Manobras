using SDK.DependencyInjection.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace SDK.DependencyInjection
{
    public class SdkDI
    {
        public static ISdkContainer Resolver { get; private set; }

        public static void SetGlobalResolver(ISdkContainer container)
        {
            Resolver = container;
        }
    }
}
