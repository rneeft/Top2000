using Chroomsoft.Top2000.WindowsApp.Common;
using MediatR;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace Chroomsoft.Top2000.WindowsApp.TrackInformation
{
    public sealed partial class TrackInformationPage : UserControl
    {
        public TrackInformationPage()
        {
            this.InitializeComponent();

            ViewModel = App.GetService<TrackInformationViewModel>();
        }

        public TrackInformationViewModel ViewModel { get; set; }

        

    }

    public class TrackInformationViewModel : ObservableBase
    {
        private readonly IMediator mediator;

        public TrackInformationViewModel(IMediator mediator)
        {
            this.mediator = mediator;
        }
    }
}
