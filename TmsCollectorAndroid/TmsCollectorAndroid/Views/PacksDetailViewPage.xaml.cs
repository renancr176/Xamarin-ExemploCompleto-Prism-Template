using TmsCollectorAndroid.Models;
using TmsCollectorAndroid.ViewModels;
using TmsCollectorAndroid.Views.Controls;
using Xamarin.Forms;

namespace TmsCollectorAndroid.Views
{
    public partial class PacksDetailViewPage : ContentPage
    {
        private readonly PacksDetailViewPageViewModel _viewModel;

        public PacksDetailViewPage()
        {
            InitializeComponent();

            _viewModel = (PacksDetailViewPageViewModel) BindingContext;

            NavigationPage.SetHasNavigationBar(this, false);

            ProcessDefaultButton.SetBinding(ProcessDefaultButton.FirstButtonCommandProperty, new Binding("ConfirmationCommand", source: BindingContext));
            ProcessDefaultButton.SetBinding(ProcessDefaultButton.SecondButtonCommandProperty, new Binding("GoBackCommand", source: BindingContext));
        }

        private void LvBillOfLadingPack_OnItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            var item = e.SelectedItem as BillOfLadingPackLvItem;
            item?.Toggle();

            LvBillOfLadingPack.SelectedItem = null;

            _viewModel.Model.SetAmountSelected();
        }

        private void Switch_OnChanged(object sender, ToggledEventArgs e)
        {
            _viewModel.Model.SetAmountSelected();
        }
    }
}
