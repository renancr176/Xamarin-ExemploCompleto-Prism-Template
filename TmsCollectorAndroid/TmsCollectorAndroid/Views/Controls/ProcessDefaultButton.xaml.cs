using System;
using System.Collections.Generic;
using System.Linq;
using Prism.Commands;
using Prism.Services;
using TmsCollectorAndroid.Enums;
using TmsCollectorAndroid.Extensions;
using TmsCollectorAndroid.Interfaces.Services;
using TmsCollectorAndroid.ViewModels.Controls;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TmsCollectorAndroid.Views.Controls
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ProcessDefaultButton : ContentView
    {
        private readonly ProcessDefaultButtonViewModel _viewModel;
        public ProcessDefaultButton()
        {
            InitializeComponent();

            _viewModel = (ProcessDefaultButtonViewModel) BindingContext;

            MenuButtonText = "Menu";
            CloseMenuText = "Fechar Menu";

            #region First Button Bindings
            FirstButton.SetBinding(Button.TextProperty, new Binding(nameof(FirstButtonText), source: this));
            FirstButton.SetBinding(Button.IsVisibleProperty, new Binding(nameof(FirstButtonIsVisible), source: this));
            FirstButton.SetBinding(Button.CommandProperty, new Binding(nameof(FirstButtonCommand), source: this));
            #endregion

            #region Second Button Bindings
            SecondButton.SetBinding(Button.TextProperty, new Binding(nameof(SecondButtonText), source: this));
            SecondButton.SetBinding(Button.IsVisibleProperty, new Binding(nameof(SecondButtonIsVisible), source: this));
            SecondButton.SetBinding(Button.CommandProperty, new Binding(nameof(SecondButtonCommand), source: this));
            #endregion

            #region Menu Button Bindings
            MenuButton.SetBinding(Button.TextProperty, new Binding(nameof(MenuButtonText), source: this));
            MenuButton.SetBinding(Button.IsVisibleProperty, new Binding(nameof(MenuButtonIsVisible), source: this));
            MenuButton.Clicked += MenuButtonOnClicked;
            #endregion
        }

        private IPageDialogService _dialogService => _viewModel._dialogService;
        private INotificationService _notificationService => _viewModel._notificationService;
        private IUserService _userService => _viewModel._userService;

        #region First Button

        public static readonly BindableProperty FirstButtonTextProperty = BindableProperty.Create(
            nameof(FirstButtonText),
            typeof(string),
            typeof(ProcessDefaultButton),
            string.Empty
        );

        public string FirstButtonText
        {
            get
            {
                var firstButtonText = (string) GetValue(FirstButtonTextProperty);
                FirstButtonIsVisible = !string.IsNullOrEmpty(firstButtonText.Trim());
                return firstButtonText;
            }
            set => SetValue(FirstButtonTextProperty, value);
        }

        public static readonly BindableProperty FirstButtonIsVisibleProperty = BindableProperty.Create(
            nameof(FirstButtonIsVisible),
            typeof(bool),
            typeof(ProcessDefaultButton),
            true
        );

        public bool FirstButtonIsVisible
        {
            get => (bool)GetValue(FirstButtonIsVisibleProperty);
            set => SetValue(FirstButtonIsVisibleProperty, value);
        }

        public static readonly BindableProperty FirstButtonCommandProperty = BindableProperty.Create(
            nameof(FirstButtonCommand),
            typeof(DelegateCommand),
            typeof(ProcessDefaultButton),
            null
        );

        public DelegateCommand FirstButtonCommand
        {
            get => (DelegateCommand)GetValue(FirstButtonCommandProperty);
            set => SetValue(FirstButtonCommandProperty, value);
        }

        public void FirstButtonFocus()
        {
            FirstButton.Focus();
        }

        #endregion

        #region Second Button

        public static readonly BindableProperty SecondButtonTextProperty = BindableProperty.Create(
            nameof(SecondButtonText),
            typeof(string),
            typeof(ProcessDefaultButton),
            string.Empty
        );

        public string SecondButtonText
        {
            get
            {
                var secondButtonText = (string) GetValue(SecondButtonTextProperty);
                SecondButtonIsVisible = !string.IsNullOrEmpty(secondButtonText.Trim());
                return secondButtonText;
            }
            set => SetValue(SecondButtonTextProperty, value);
        }

        public static readonly BindableProperty SecondButtonIsVisibleProperty = BindableProperty.Create(
            nameof(SecondButtonIsVisible),
            typeof(bool),
            typeof(ProcessDefaultButton),
            false
        );

        public bool SecondButtonIsVisible
        {
            get => (bool)GetValue(SecondButtonIsVisibleProperty);
            set => SetValue(SecondButtonIsVisibleProperty, value);
        }

        public static readonly BindableProperty SecondButtonCommandProperty = BindableProperty.Create(
            nameof(SecondButtonCommand),
            typeof(DelegateCommand),
            typeof(ProcessDefaultButton),
            null
        );

        public DelegateCommand SecondButtonCommand
        {
            get => (DelegateCommand)GetValue(SecondButtonCommandProperty);
            set => SetValue(SecondButtonCommandProperty, value);
        }

        private void SecondButtonFocus()
        {
            SecondButton.Focus();
        }

        #endregion

        #region Menu Button

        private async void MenuButtonOnClicked(object sender, EventArgs e)
        {
            var selectedOption = await _dialogService.DisplayActionSheetAsync(
                "Menu",
                CloseMenuText,
                null,
                GetMenuOptions());

            if (!selectedOption.Equals(CloseMenuText))
            {
                switch (selectedOption.GetEnumByDescription<MenuOptionEnum>())
                {
                    case MenuOptionEnum.LogOut:
                        if (await _notificationService.AskQuestionAsync("Deseja trocar de usuário?", SoundEnum.Alert))
                            _userService.LogOut();
                        break;
                    case MenuOptionEnum.Clear:
                        ClearCommand?.Execute();
                        break;
                    case MenuOptionEnum.GoBack:
                        GoBackCommand?.Execute();
                        break;
                    case MenuOptionEnum.Exit:
                        if (await _notificationService.AskQuestionAsync("Deseja sair do sistema?", SoundEnum.Alert))
                            System.Diagnostics.Process.GetCurrentProcess().CloseMainWindow();
                        break;
                    default:
                        if (MenuAdtionalButtons != null)
                        {
                            if (MenuAdtionalButtons.TryGetValue(selectedOption, out DelegateCommand command))
                            {
                                command.Execute();
                            }
                            else
                            {
                                await _notificationService.NotifyAsync("Comando não implementado.", SoundEnum.Falha);
                            }
                        }
                        break;
                }
            }
        }

        private string[] GetMenuOptions()
        {
            var options = new List<string>();

            if (MenuAdtionalButtons != null && MenuAdtionalButtons.Any())
            {
                options.AddRange(MenuAdtionalButtons.Select(mab => mab.Key).ToList());
            }

            options.AddRange(Enum.GetValues(typeof(MenuOptionEnum))
                .Cast<MenuOptionEnum>()
                .Select(en => en.GetDescription())
                .ToList());

            return options.ToArray();
        }

        public string _menuButtonText;
        public string MenuButtonText
        {
            get { return _menuButtonText;}
            private set
            {
                _menuButtonText = value;
                OnPropertyChanged(nameof(MenuButtonText));
            }
        }

        private string _closeMenuText;
        public string CloseMenuText
        {
            get { return _closeMenuText; }
            private set
            {
                _closeMenuText = value;
                OnPropertyChanged(nameof(MenuButtonText));
            }
        }

        public static readonly BindableProperty MenuButtonIsVisibleProperty = BindableProperty.Create(
            nameof(MenuButtonIsVisible),        // the name of the bindable property
            typeof(bool),      // the bindable property type
            typeof(ProcessDefaultButton),    // the parent object type
            true      // the default value for the property
        );

        public bool MenuButtonIsVisible
        {
            get => (bool)GetValue(MenuButtonIsVisibleProperty);
            set => SetValue(MenuButtonIsVisibleProperty, value);
        }

        public static readonly BindableProperty ClearCommandProperty = BindableProperty.Create(
            nameof(ClearCommand),
            typeof(DelegateCommand),
            typeof(ProcessDefaultButton),
            null
        );

        public DelegateCommand ClearCommand
        {
            get => (DelegateCommand)GetValue(ClearCommandProperty);
            set => SetValue(ClearCommandProperty, value);
        }

        public static readonly BindableProperty GoBackCommandProperty = BindableProperty.Create(
            nameof(GoBackCommand),
            typeof(DelegateCommand),
            typeof(ProcessDefaultButton),
            null
        );

        public DelegateCommand GoBackCommand
        {
            get => (DelegateCommand)GetValue(GoBackCommandProperty);
            set => SetValue(GoBackCommandProperty, value);
        }

        public static readonly BindableProperty MenuAdtionalButtonsProperty = BindableProperty.Create(
            nameof(MenuAdtionalButtons),
            typeof(Dictionary<string, DelegateCommand>),
            typeof(ProcessDefaultButton),
            new Dictionary<string, DelegateCommand>()
        );

        public Dictionary<string, DelegateCommand> MenuAdtionalButtons
        {
            get => (Dictionary<string, DelegateCommand>)GetValue(MenuAdtionalButtonsProperty);
            set => SetValue(MenuAdtionalButtonsProperty, value);
        }

        #endregion
    }
}