using System;
using oops;
using Praedium.Services;

namespace Praedium.ViewModels
{
    public class ShellViewModel : TrackableViewModel
    {
        public ShellViewModel()
        {
#pragma warning disable 4014
            NavigationService.NavigateTo<LandingViewModel>();
#pragma warning restore 4014
        }
    }
}
