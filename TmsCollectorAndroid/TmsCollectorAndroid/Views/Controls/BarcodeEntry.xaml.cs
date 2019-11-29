using System;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using TmsCollectorAndroid.ViewModels.Controls;
using Xamarin.Forms;

namespace TmsCollectorAndroid.Views.Controls
{
    public partial class BarcodeEntry : ContentView
    {
        private readonly BarcodeEntryViewModel _viewModel;

        public BarcodeEntry()
        {
            InitializeComponent();

            _viewModel = (BarcodeEntryViewModel) BindingContext;

            CustomEntry.SetBinding(Entry.TextProperty, new Binding(nameof(Text), source: this));
            CustomEntry.SetBinding(Entry.IsEnabledProperty, new Binding(nameof(IsEnabled), source: this));
            CustomEntry.SetBinding(Entry.IsReadOnlyProperty, new Binding(nameof(IsReadOnly), source: this));
            CustomEntry.TextChanged += OnTextChanged;
            CustomEntry.Unfocused += OnUnfocused;
        }

        public static BindableProperty TextProperty = BindableProperty.Create(
            nameof(Text),
            typeof(string),
            typeof(BarcodeEntry),
            null);

        public string Text
        {
            get => (string)GetValue(TextProperty);
            set => SetValue(TextProperty, value?.Trim());
        }

        public static BindableProperty IsEnabledProperty = BindableProperty.Create(
            nameof(IsEnabled),
            typeof(bool),
            typeof(BarcodeEntry),
            true);

        public bool IsEnabled
        {
            get => (bool)GetValue(IsEnabledProperty);
            set => SetValue(IsEnabledProperty, value);
        }

        public static BindableProperty IsReadOnlyProperty = BindableProperty.Create(
            nameof(IsReadOnly),
            typeof(bool),
            typeof(BarcodeEntry),
            false);

        public bool IsReadOnly
        {
            get => (bool)GetValue(IsReadOnlyProperty);
            set => SetValue(IsReadOnlyProperty, value);
        }

        public void Focus()
        {
            Task.Run(async () =>
            {
                await Task.Delay(TimeSpan.FromMilliseconds(0.5));
                CustomEntry.Focus();
            });
        }

        public event EventHandler<TextChangedEventArgs> TextChanged;

        public event EventHandler<FocusEventArgs> Unfocused;

        private void OnTextChanged(object sender, TextChangedEventArgs e)
        {
            if (TextChanged != null 
            && !string.IsNullOrEmpty(e.NewTextValue) 
            && (
                _viewModel.LabelValidationService.ValidateCommonLabel(e.NewTextValue)
                || _viewModel.LabelValidationService.ValidateMotherLabel(e.NewTextValue)))
            {
                CustomEntry.Unfocus();

                Task.Run(async () =>
                {
                    await Task.Delay(TimeSpan.FromMilliseconds(100));

                    Device.BeginInvokeOnMainThread(() =>
                    {
                        TextChanged?.Invoke(sender, e);
                    });
                });
            }
        }

        private void OnUnfocused(object sender, FocusEventArgs e)
        {
            Unfocused?.Invoke(sender, e);
        }
    }
}
