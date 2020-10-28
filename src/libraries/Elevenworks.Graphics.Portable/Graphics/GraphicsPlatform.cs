using System.Threading;

namespace Elevenworks.Graphics
{
    public static class GraphicsPlatform
    {
        private static IGraphicsService globalService;
        private static ThreadLocal<IGraphicsService> threadLocalService;

        /// <summary>
        /// Registers the global service to be used.
        /// </summary>
        /// <param name="service"></param>
        public static void RegisterGlobalService(IGraphicsService service)
        {
            globalService = service ?? new VirtualGraphicsPlatform();
        }

        /// <summary>
        /// Registers graphics service instance to be used for the current rendering thread
        /// </summary>
        /// <param name="service"></param>
        public static void RegisterThreadLocalContext(IGraphicsService service)
        {
            if (threadLocalService == null)
                threadLocalService = new ThreadLocal<IGraphicsService>();

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

        public static IGraphicsService GlobalService
        {
            get
            {
                if (globalService == null)
                {
                    globalService = new VirtualGraphicsPlatform();
                    Logger.Warn("No graphics platform was registered.  Falling back to the virtual implementation.");
                }

                return globalService;
            }
        }

        public static IGraphicsService CurrentService
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
                    globalService = new VirtualGraphicsPlatform();
                    Logger.Warn("No graphics platform was registered.  Falling back to the virtual implementation.");
                }

                return globalService;
            }
        }

        public static void Register(IGraphicsService service)
        {
            globalService = service;
        }

        public static bool IsRetina => CurrentService.IsRetina;
    }
}