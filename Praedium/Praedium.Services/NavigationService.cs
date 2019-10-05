using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using oops;
using oops.Interfaces;

namespace Praedium.Services
{
    public class NavigationService
    {
        // ReSharper disable InconsistentNaming
        private static readonly Dictionary<string, object> _viewLookup = new Dictionary<string, object>();
        private static readonly Stack<object> _navigationStack = new Stack<object>();
        private static readonly Dictionary<Type, Type> _registrations = new Dictionary<Type, Type>();

        /// <summary>
        /// Shows the view for the corresponding ViewModel and pushes it to the navigation stack.
        /// </summary>
        /// <typeparam name="TViewModel"></typeparam>
        /// <param name="nonSingleton"></param>
        /// <returns>False if no view found</returns>
        public static async Task<bool> NavigateTo<TViewModel>(bool nonSingleton = false) where TViewModel : TrackableViewModel
        {
            if(ViewInjector == null)
                throw new ArgumentNullException(nameof(ViewInjector));

            var viewName = typeof(TViewModel).Name.Replace(@"Model", "");

            if(!nonSingleton && _viewLookup.ContainsKey(viewName))
            {
                _navigationStack.Push(_viewLookup[viewName]);
                ViewInjector(_viewLookup[viewName]);
                return true;
            }

            var viewType = GetTypeByName(viewName);
            //if(!_registrations.TryGetValue(typeof(TViewModel), out var viewType))
            if (viewType == null)
                _registrations.TryGetValue(typeof(TViewModel), out viewType);

            if(viewType == null)
                throw new InvalidOperationException($"ViewModel {typeof(TViewModel).Name} has not been registered with a View or does not exist at {ViewAssemblyNamePrefix}!!");

            if(!typeof(IView).IsAssignableFrom(viewType))
                throw new InvalidOperationException($@"View {viewType.Name} does not inherit from IView!!");

            await Injector.GetSingleton<IDispatcher>().BeginInvoke(() =>
            {
                var view = Activator.CreateInstance(viewType) as IView;
                Debug.Assert(view != null);
                if(typeof(TViewModel).GetConstructors().All(c => c.GetParameters().Length != 0 && c.IsPublic))
                    throw new InvalidOperationException(
                        $@"ViewModel {typeof(TViewModel).Name} does not have an public empty ctor!");

                var vm = Activator.CreateInstance(typeof(TViewModel)) as TViewModel;
                Debug.Assert(vm != null);
                if(!nonSingleton)
                    _viewLookup.Add(viewName, view);
                view.DataContext = vm;
                ViewInjector(view);
                _navigationStack.Push(view);
            });
            return true;
        }

        public static Type GetTypeByName(string name)
        {
            var assembly = Assembly.GetEntryAssembly();
            if(assembly == null)
                return null;
            var tt = assembly.GetType(ViewAssemblyNamePrefix + "." + name, false, true);
            if(tt != null)
                return tt;

            return null;
        }

        /// <summary>
        /// Goes back to the previous view, if any
        /// </summary>
        /// <returns></returns>
        public static bool GoBack()
        {
            if(ViewInjector == null)
                throw new ArgumentNullException(nameof(ViewInjector));

            // first view is base/root view, so leave that there always
            if(!(_navigationStack.Count > 1))
                return false;

            var view = _navigationStack.Pop();
            ViewInjector(view);
            return true;
        }

        public static Action<object> ViewInjector { get; set; }
        public static string ViewAssemblyNamePrefix { get; set; }

        /// <summary>
        /// Can either directly register a view or put it at the ViewAssemblyNamePrefix namespace within the entry assembly
        /// </summary>
        /// <typeparam name="TViewModel"></typeparam>
        /// <typeparam name="TView"></typeparam>
        public static void Register<TViewModel, TView>()
        {
            if(_registrations.ContainsKey(typeof(TViewModel)))
                return;

            _registrations.Add(typeof(TViewModel), typeof(TView));
        }
    }
}
