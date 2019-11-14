using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TmsCollectorAndroid.Views.Controls
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Header : ContentView
    {
        public Header()
        {
            InitializeComponent();

            HeaderTitle.SetBinding(Label.TextProperty, new Binding("Title", source: this));
        }

        public static readonly BindableProperty TitleProperty = BindableProperty.Create(
            "Title",        // the name of the bindable property
            typeof(string),      // the bindable property type
            typeof(Header),    // the parent object type
            string.Empty       // the default value for the property
        );      

        public string Title
        {
            get => (string)GetValue(TitleProperty);
            set => SetValue(TitleProperty, value);
        }
    }
}