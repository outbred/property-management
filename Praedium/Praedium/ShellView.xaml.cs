using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using oops.Collections;
using oops.Interfaces;
using Praedium.ViewModels;
using NavigationService = Praedium.Services.NavigationService;

namespace Praedium
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class ShellView : Window
    {
        public ShellView()
        {
            InitializeComponent();
            NavigationService.ViewInjector = view => this.Content = view;

            DataContext = new ShellViewModel();
        }
    }
}
