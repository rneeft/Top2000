using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace Chroomsoft.Top2000.WindowsApp.TrackInformation
{
    public sealed partial class AppearanceIndicator : UserControl
    {
        public static readonly DependencyProperty TitleProperty = DependencyProperty.Register(
            nameof(Title),
            typeof(string),
            typeof(AppearanceIndicator),
            new PropertyMetadata(string.Empty));

        public static readonly DependencyProperty ValueProperty = DependencyProperty.Register(
            nameof(Value),
            typeof(int),
            typeof(AppearanceIndicator),
            new PropertyMetadata(0));

        public static readonly DependencyProperty MaxValueProperty = DependencyProperty.Register(
            nameof(MaxValue),
            typeof(int),
            typeof(AppearanceIndicator),
            new PropertyMetadata(0));

        public AppearanceIndicator()
        {
            this.InitializeComponent();
        }

        public string Title
        {
            get { return (string)GetValue(TitleProperty); }
            set { SetValue(TitleProperty, value); }
        }

        public double Value
        {
            get { return (double)GetValue(ValueProperty); }
            set { SetValue(ValueProperty, value); }
        }

        public double MaxValue
        {
            get { return (double)GetValue(MaxValueProperty); }
            set { SetValue(MaxValueProperty, value); }
        }

        private void ReCalculate()
        {
            if (MaxValue > 0 && double.TryParse(value.Text, out double dValue))
                progress.Value = dValue / MaxValue * 100;
        }

        private void HackOnValueChange(object sender, TextChangedEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(hack.Text))
                ReCalculate();
        }
    }
}
