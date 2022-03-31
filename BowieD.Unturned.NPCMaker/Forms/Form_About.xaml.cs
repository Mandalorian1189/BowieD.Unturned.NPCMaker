﻿using BowieD.Unturned.NPCMaker.Configuration;
using BowieD.Unturned.NPCMaker.Localization;
using MahApps.Metro.Controls;
using System.Windows;
using System.Windows.Media.Animation;

namespace BowieD.Unturned.NPCMaker.Forms
{
    /// <summary>
    /// Логика взаимодействия для Form_About.xaml
    /// </summary>
    public partial class Form_About : MetroWindow
    {
        public Form_About()
        {
            InitializeComponent();
            string aboutText = LocalizationManager.Current.Interface.Translate("App_About", LocalizationManager.Current.Author, App.Version);
            mainText.Text = aboutText;
            double scale = AppConfig.Instance.scale;
            Height *= scale;
            Width *= scale;
            gridScale.ScaleX = scale;
            gridScale.ScaleY = scale;

            if (AppConfig.Instance.animateControls)
            {
                DoubleAnimation da = new DoubleAnimation(0, 1, new Duration(new System.TimeSpan(0, 0, 1)));
                authorText.BeginAnimation(OpacityProperty, da);
            }

            foreach (string patron in App.Package.Patrons)
            {
                patronsList.Items.Add(patron);
            }

            foreach (System.Collections.Generic.KeyValuePair<string, string> credit in App.Package.Credits)
            {
                creditsList.Items.Add(credit);
            }
        }
    }
}
