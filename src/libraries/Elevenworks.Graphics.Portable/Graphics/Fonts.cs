using System.Threading;

namespace Elevenworks.Graphics
{
    public static class Fonts
    {
        private static IFontService globalService;
        private static ThreadLocal<IFontService> threadLocalService;

        /// <summary>
        /// Registers the global service to be used.
        /// </summary>
        /// <param name="service"></param>
        public static void RegisterGlobalService(IFontService service)
        {
            globalService = service ?? new VirtualFontService();
        }

        /// <summary>
        /// Registers graphics service instance to be used for the current rendering thread
        /// </summary>
        /// <param name="service"></param>
        public static void RegisterThreadLocalContext(IFontService service)
        {
            if (threadLocalService == null)
                threadLocalService = new ThreadLocal<IFontService>();

            threadLocalService.Value = service;
        }

        /// <summary>
        /// Clears the context on the local thread in one has been set.
        /// </summary>
        public static void ClearThreadLocalContext()
        {
            if (threadLocalService != null)
                threadLocalService.Value = null;
        }

        public static IFontService GlobalService
        {
            get
            {
                if (globalService == null)
                {
                    globalService = new VirtualFontService();
                    Logger.Warn("No font service was registered.  Falling back to the virtual implementation.");
                }

                return globalService;
            }
        }

        public static IFontService CurrentService
        {
            get
            {
                if (threadLocalService != null && threadLocalService.IsValueCreated)
                {
                    var localContext = threadLocalService.Value;
                    if (localContext != null)
                        return localContext;
                }

                if (globalService == null)
                {
                    globalService = new VirtualFontService();
                    Logger.Warn("No font service was registered.  Falling back to the virtual implementation.");
                }

                return globalService;
            }
        }

        public static void Register(IFontService service)
        {
            globalService = service;
        }
    }
}