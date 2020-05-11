﻿using BowieD.Unturned.NPCMaker.Configuration;
using BowieD.Unturned.NPCMaker.Localization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Animation;
using Reward = BowieD.Unturned.NPCMaker.NPC.Rewards.Reward;

namespace BowieD.Unturned.NPCMaker.Forms
{
    /// <summary>
    /// Логика взаимодействия для Universal_RewardEditor.xaml
    /// </summary>
    public partial class Universal_RewardEditor : Window
    {
        public Universal_RewardEditor(Reward reward = null)
        {
            InitializeComponent();
            double scale = AppConfig.Instance.scale;
            ClearParameters();
            Height *= scale;
            Width *= scale;
            baseHeight = Height;
            heightDelta *= scale;
            gridScale.ScaleX = scale;
            gridScale.ScaleY = scale;
            bool _chosen = false;
            int _index = 0;
            foreach (Type t in Reward.GetTypes())
            {
                ComboBoxItem cbi = new ComboBoxItem
                {
                    Content = LocalizationManager.Current.Reward[$"Type{Reward.GetLocalizationKey(t.Name)}"],
                    Tag = t
                };
                typeBox.Items.Add(cbi);
                if (!_chosen && reward != null && reward.GetType() == t)
                {
                    typeBox.SelectedIndex = _index;
                    _chosen = true;
                }
                _index++;
            }
            if (reward != null)
            {
                variablesGrid.DataContext = reward;
            }

            saveButton.IsEnabled = reward != null;
        }

        public Reward Result { get; private set; }

        #region DESIGN VARS
        private readonly double baseHeight = 178;
        private readonly double heightDelta = 35;
        #endregion
        #region METHODS
        internal void ClearParameters()
        {
            variablesGrid.Children.Clear();
            Height = baseHeight;
        }
        #endregion

        private Type _CurrentRewardType = null;
        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (variablesGrid.DataContext == null)
                {
                    DialogResult = false;
                }
                else
                {
                    DialogResult = true;
                }

                Result = variablesGrid.DataContext as Reward;
                if (DialogResult == true)
                {
                    Close();
                }
            }
            catch
            {
                MessageBox.Show(LocalizationManager.Current.Interface["Editor_Reward_Fail"]);
            }
        }
        private void TypeBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            saveButton.IsEnabled = true;
            if (e.AddedItems.Count == 0)
            {
                return;
            }

            Type type = (typeBox.SelectedItem as ComboBoxItem).Tag as Type;
            Reward newReward = (Reward)Activator.CreateInstance(type);
            _CurrentRewardType = type;
            ClearParameters();
            variablesGrid.DataContext = newReward;
            IEnumerable<FrameworkElement> controls = newReward.GetControls();
            int mult = controls.Count();
            foreach (FrameworkElement c in controls)
            {
                variablesGrid.Children.Add(c);
            }

            double newHeight = (baseHeight + (heightDelta * (mult + (mult > 1 ? 1 : 0))));
            if (AppConfig.Instance.animateControls)
            {
                DoubleAnimation anim = new DoubleAnimation(Height, newHeight, new Duration(new TimeSpan(0, 0, 0, 0, 500)));
                BeginAnimation(HeightProperty, anim);
            }
            else
            {
                Height = newHeight;
            }
        }
        private FrameworkElement GetLocalizationControl()
        {
            FrameworkElement control = Util.FindVisualChildren<FrameworkElement>(variablesGrid).First(d => d.Tag != null && d.Tag.ToString() == "variable::Localization");
            return Util.FindParent<Border>(control);
        }
    }
}
