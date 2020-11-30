using System.ComponentModel;
using Xamarin.Forms;
using XamarinApp.ViewModels;

namespace XamarinApp.Views
{
    public partial class ItemDetailPage : ContentPage
    {
        public ItemDetailPage()
        {
            InitializeComponent();
            BindingContext = new ItemDetailViewModel();
        }
    }
}