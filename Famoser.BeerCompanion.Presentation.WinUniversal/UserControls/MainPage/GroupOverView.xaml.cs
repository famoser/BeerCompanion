﻿using System;
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
using Famoser.BeerCompanion.Business.Models;
using Famoser.BeerCompanion.View.ViewModels;
using GalaSoft.MvvmLight.Ioc;
using GalaSoft.MvvmLight.Messaging;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace Famoser.BeerCompanion.Presentation.WinUniversal.UserControls.MainPage
{
    public sealed partial class GroupOverView : UserControl
    {
        public GroupOverView()
        {
            this.InitializeComponent();
        }

        private void ListViewBase_OnItemClick(object sender, ItemClickEventArgs e)
        {
            var cycle = e.ClickedItem as DrinkerCycle;
            var vm = SimpleIoc.Default.GetInstance<MainViewModel>();
            vm.NavigateToCycle(cycle);
        }
    }
}