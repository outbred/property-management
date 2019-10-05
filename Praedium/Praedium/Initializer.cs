using System.Windows.Threading;
using oops.Interfaces;
using Praedium.Services;
using Praedium.ViewModels;

namespace Praedium
{
    public static class Initializer
    {
        public static void Initialize()
        {
            WpfDispatcher.Initialize(Dispatcher.CurrentDispatcher);
            oops.Globals.Dispatcher = Injector.GetSingleton<WpfDispatcher>();
            Injector.Register<IDispatcher, WpfDispatcher>();
            NavigationService.Register<LandingViewModel, LandingView>();
            NavigationService.Register<EvaluatePropertyViewModel, EvaluatePropertyView>();
        }
    }
}