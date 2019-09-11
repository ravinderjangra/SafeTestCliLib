using System;

namespace SafeCliBindings
{
    public static class AppResolver
    {
#if !NETSTANDARD
        private static readonly Lazy<AppBindings> Implementation = new Lazy<AppBindings>(
          CreateBindings,
          System.Threading.LazyThreadSafetyMode.PublicationOnly);
#endif

        public static AppBindings Current
        {
            get
            {
#if NETSTANDARD
                throw NotImplementedInReferenceAssembly();
#else
                return Implementation.Value;
#endif
            }
        }

#if !NETSTANDARD
        private static AppBindings CreateBindings()
        {
            return new AppBindings();
        }
#endif

        private static Exception NotImplementedInReferenceAssembly()
        {
            return new NotImplementedException(
              "This functionality is not implemented in the portable version of this assembly.  You should reference the NuGet package from your main application project in order to reference the platform-specific implementation.");
        }
    }
}
