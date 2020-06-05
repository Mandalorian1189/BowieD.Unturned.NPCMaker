﻿using BowieD.Unturned.NPCMaker.Configuration;
using BowieD.Unturned.NPCMaker.NPC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Animation;
using Condition = BowieD.Unturned.NPCMaker.NPC.Conditions.Condition;

namespace BowieD.Unturned.NPCMaker.Forms
{
    /// <summary>
    /// Логика взаимодействия для Universal_VendorItemEditor.xaml
    /// </summary>
    public partial class Universal_VendorItemEditor : Window
    {
        public Universal_VendorItemEditor(VendorItem startItem = null)
        {
            InitializeComponent();

            double scale = AppConfig.Instance.scale;
            gridScale.ScaleX = scale;
            gridScale.ScaleY = scale;
            Width *= scale;
            Height *= scale;
            typeBox.SelectedIndex = 0;
            sellBox.SelectedIndex = 0;
            Result = startItem ?? new VendorItem() { isBuy = !isSellSelected, type = lastType };
            txtBoxCost.Value = Result.cost;
            txtBoxID.Value = Result.id;
            txtBoxSpawnpoint.Text = Result.spawnPointID;
            foreach (ComboBoxItem cbi in typeBox.Items)
            {
                ItemType it = (ItemType)cbi.Tag;
                if (it == Result.type)
                {
                    typeBox.SelectedItem = cbi;
                    break;
                }
            }
            foreach (ComboBoxItem cbi in sellBox.Items)
            {
                if ((cbi.Tag.ToString() == "BUY" && Result.isBuy) || (cbi.Tag.ToString() == "SELL" && !Result.isBuy))
                {
                    sellBox.SelectedItem = cbi;
                    break;
                }
            }
        }
        public VendorItem Result { get; private set; }

        public DoubleAnimation DisappearAnimation(double current)
        {
            return new DoubleAnimation(current, 0, new Duration(new TimeSpan(0, 0, 1)));
        }
        public DoubleAnimation AppearAnimation(double current)
        {
            return new DoubleAnimation(current, 1, new Duration(new TimeSpan(0, 0, 1)));
        }

        private void TypeBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.AddedItems.Count == 0)
            {
                return;
            }

            if (Selected_ItemType == ItemType.ITEM)
            {
                if (AppConfig.Instance.animateControls)
                {
                    txtBoxSpawnpoint.BeginAnimation(OpacityProperty, DisappearAnimation(1));
                    labelSpawnpoint.BeginAnimation(OpacityProperty, DisappearAnimation(1));
                }
                else
                {
                    labelSpawnpoint.Opacity = 0;
                    txtBoxSpawnpoint.Opacity = 0;
                }
                txtBoxSpawnpoint.IsHitTestVisible = false;
                sellBox.IsEnabled = true;
                txtBoxSpawnpoint.Text = "";
            }
            else
            {
                if (AppConfig.Instance.animateControls)
                {
                    DoubleAnimation opacityAnimation = new DoubleAnimation(0, 1, new Duration(new System.TimeSpan(0, 0, 0, 0, 500)));
                    txtBoxSpawnpoint.BeginAnimation(OpacityProperty, AppearAnimation(0));
                    labelSpawnpoint.BeginAnimation(OpacityProperty, AppearAnimation(0));
                }
                else
                {
                    labelSpawnpoint.Opacity = 1;
                    txtBoxSpawnpoint.Opacity = 1;
                }
                txtBoxSpawnpoint.IsHitTestVisible = true;
                sellBox.IsEnabled = false;
                sellBox.SelectedIndex = 1;
            }
        }

        private ItemType Selected_ItemType => typeBox.SelectedItem is ComboBoxItem cbi && cbi.Tag is ItemType it ? it : ItemType.ITEM;
        private bool IsBuy => sellBox.SelectedItem is ComboBoxItem cbi && cbi.Tag.ToString() == "BUY";

        private void EditConditions_Click(object sender, RoutedEventArgs e)
        {
            Universal_ListView ulv = new Universal_ListView(Result.conditions.Select(d => new Controls.Universal_ItemList(d, Controls.Universal_ItemList.ReturnType.Condition, false)).ToList(), Controls.Universal_ItemList.ReturnType.Condition);
            ulv.Owner = this;
            ulv.ShowDialog();
            Result.conditions = ulv.Values.Cast<Condition>().ToList();
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Result.id = (ushort)txtBoxID.Value;
                Result.type = Selected_ItemType;
                if (Result.type == ItemType.VEHICLE)
                {
                    Result.spawnPointID = txtBoxSpawnpoint.Text;
                }

                Result.isBuy = IsBuy;
                Result.cost = (uint)txtBoxCost.Value;
                if (Result.conditions == null)
                {
                    Result.conditions = new List<Condition>();
                }

                isSellSelected = !IsBuy;
                lastType = Selected_ItemType;
                DialogResult = true;
                Close();
            }
            catch { }
        }
        private static bool isSellSelected = false;
        private static ItemType lastType = ItemType.ITEM;
    }
}
