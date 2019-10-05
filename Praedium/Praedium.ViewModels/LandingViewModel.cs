using System;
using System.Collections.Generic;
using System.Text;
using oops;
using Praedium.Services;

namespace Praedium.ViewModels
{
    public class LandingViewModel : TrackableViewModel
    {
        public AsyncCommand OnEvaluateProperty => new AsyncCommand(async () => await NavigationService.NavigateTo<EvaluatePropertyViewModel>());
        public AsyncCommand OnManageRenters => new AsyncCommand(async () => await NavigationService.NavigateTo<EvaluatePropertyViewModel>());
        public AsyncCommand OnManageProperties => new AsyncCommand(async () => await NavigationService.NavigateTo<EvaluatePropertyViewModel>());
    }

    public class EvaluatePropertyViewModel : TrackableViewModel
    {

    }
}
