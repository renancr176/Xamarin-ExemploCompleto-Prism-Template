﻿using System;
using TmsCollectorAndroid.Enums;
using TmsCollectorAndroid.Extensions;
using TmsCollectorAndroid.Interfaces.Services;
using TmsCollectorAndroid.ViewModels.Controls;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TmsCollectorAndroid.Views.Controls
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class TimeEntry : ContentView
    {
        public TimeEntry()
        {
            InitializeComponent();

            CustomEntry.SetBinding(Entry.TextProperty, new Binding(nameof(Time), source: this));
            CustomEntry.SetBinding(Entry.IsEnabledProperty, new Binding(nameof(IsEnabled), source: this));
            CustomEntry.SetBinding(Entry.IsReadOnlyProperty, new Binding(nameof(IsReadOnly), source: this));
            CustomEntry.TextChanged += OnTextChanged;
            CustomEntry.Unfocused += OnUnfocused;
        }

        private INotificationService _notificationService => ((TimeEntryViewModel) BindingContext)._notificationService;

        public static BindableProperty TimeProperty = BindableProperty.Create(
            nameof(Time),
            typeof(string),
            typeof(TimeEntry),
            null);

        public string Time
        {
            get => (string)GetValue(TimeProperty);
            set => SetValue(TimeProperty, value);
        }

        public static BindableProperty IsEnabledProperty = BindableProperty.Create(
            nameof(IsEnabled),
            typeof(bool),
            typeof(DateEntry),
            true);

        public bool IsEnabled
        {
            get => (bool)GetValue(IsEnabledProperty);
            set => SetValue(IsEnabledProperty, value);
        }

        public static BindableProperty IsReadOnlyProperty = BindableProperty.Create(
            nameof(IsReadOnly),
            typeof(bool),
            typeof(DateEntry),
            false);

        public bool IsReadOnly
        {
            get => (bool)GetValue(IsReadOnlyProperty);
            set => SetValue(IsReadOnlyProperty, value);
        }

        public event EventHandler<TextChangedEventArgs> TextChanged;

        public event EventHandler<FocusEventArgs> Unfocused;

        public void Focus()
        {
            CustomEntry.Focus();
        }

        private void OnTextChanged(object sender, TextChangedEventArgs e)
        {
            TextChanged?.Invoke(sender, e);
        }

        private async void OnUnfocused(object sender, FocusEventArgs e)
        {
            if (!IsValid(Time))
            {
                await _notificationService.NotifyAsync($"A hora digitada {Time} é inválida.", SoundEnum.Erros);
                Time = String.Empty;
            }

            Unfocused?.Invoke(sender, e);
        }

        private bool IsValid(string strTime)
        {
            return (!string.IsNullOrEmpty(strTime) && strTime.IsTimeFormated() &&
                    TimeSpan.TryParse(strTime, out var time));
        }
    }
}