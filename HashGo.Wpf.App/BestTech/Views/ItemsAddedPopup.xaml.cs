﻿using System;
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
using System.Windows.Shapes;

namespace HashGo.Wpf.App.BestTech.Views
{
    /// <summary>
    /// Interaction logic for ItemsAddedPopup.xaml
    /// </summary>
    public partial class ItemsAddedPopup : Window
    {
        public int UnitsCount { get; set; }
        public ItemsAddedPopup()
        {
            InitializeComponent();
        }
    }
}
