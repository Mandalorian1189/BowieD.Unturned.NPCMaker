﻿using BowieD.Unturned.NPCMaker.Localization;
using BowieD.Unturned.NPCMaker.NPC.Rewards.Attributes;
using BowieD.Unturned.NPCMaker.XAML;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Xml.Serialization;

namespace BowieD.Unturned.NPCMaker.NPC.Rewards
{
    [XmlInclude(typeof(RewardExperience))]
    [XmlInclude(typeof(RewardReputation))]
    [XmlInclude(typeof(RewardFlagBool))]
    [XmlInclude(typeof(RewardFlagShort))]
    [XmlInclude(typeof(RewardFlagShortRandom))]
    [XmlInclude(typeof(RewardQuest))]
    [XmlInclude(typeof(RewardItem))]
    [XmlInclude(typeof(RewardItemRandom))]
    [XmlInclude(typeof(RewardAchievement))]
    [XmlInclude(typeof(RewardVehicle))]
    [XmlInclude(typeof(RewardTeleport))]
    [XmlInclude(typeof(RewardEvent))]
    [XmlInclude(typeof(RewardFlagMath))]
    [XmlInclude(typeof(RewardCurrency))]
    [XmlInclude(typeof(RewardHint))]
    public abstract class Reward : IHasUIText
    {
        [RewardSkipField]
        public string Localization { get; set; }
        public abstract RewardType Type { get; }
        public abstract string UIText { get; }
        public IEnumerable<FrameworkElement> GetControls()
        {
            PropertyInfo[] props = GetType().GetProperties();
            foreach (PropertyInfo prop in props)
            {
                if (!prop.CanWrite || !prop.CanRead)
                {
                    continue;
                }

                string propName = prop.Name;
                Type propType = prop.PropertyType;

                string localizedName;

                string key1 = $"{Type}_{propName}";
                string key2 = $"Shared_{propName}";

                if (LocalizationManager.Current.Reward.ContainsKey(key1))
                    localizedName = LocalizationManager.Current.Reward.Translate(key1);
                else if (LocalizationManager.Current.Reward.ContainsKey(key2))
                    localizedName = LocalizationManager.Current.Reward.Translate(key2);
                else
                    localizedName = key1;

                Grid borderContents = new Grid();
                Label l = new Label
                {
                    Content = localizedName
                };
                RewardTooltipAttribute rewardTooltip = prop.GetCustomAttribute<RewardTooltipAttribute>();
                if (rewardTooltip != null)
                {
                    l.ToolTip = rewardTooltip.Text;
                }
                borderContents.Children.Add(l);
                FrameworkElement valueControl = null;
                if (propType == typeof(ushort))
                {
                    valueControl = new MahApps.Metro.Controls.NumericUpDown()
                    {
                        Maximum = ushort.MaxValue,
                        Minimum = ushort.MinValue,
                        ParsingNumberStyle = System.Globalization.NumberStyles.Integer,
                        HideUpDownButtons = true
                    };
                    (valueControl as MahApps.Metro.Controls.NumericUpDown).SetBinding(MahApps.Metro.Controls.NumericUpDown.ValueProperty, propName);
                }
                else if (propType == typeof(uint))
                {
                    valueControl = new MahApps.Metro.Controls.NumericUpDown()
                    {
                        Maximum = uint.MaxValue,
                        Minimum = uint.MinValue,
                        ParsingNumberStyle = System.Globalization.NumberStyles.Integer,
                        HideUpDownButtons = true
                    };
                    (valueControl as MahApps.Metro.Controls.NumericUpDown).SetBinding(MahApps.Metro.Controls.NumericUpDown.ValueProperty, propName);
                }
                else if (propType == typeof(int))
                {
                    valueControl = new MahApps.Metro.Controls.NumericUpDown()
                    {
                        Maximum = int.MaxValue,
                        Minimum = int.MinValue,
                        ParsingNumberStyle = System.Globalization.NumberStyles.Integer,
                        HideUpDownButtons = true
                    };
                    (valueControl as MahApps.Metro.Controls.NumericUpDown).SetBinding(MahApps.Metro.Controls.NumericUpDown.ValueProperty, propName);
                }
                else if (propType == typeof(short))
                {
                    valueControl = new MahApps.Metro.Controls.NumericUpDown()
                    {
                        Maximum = short.MaxValue,
                        Minimum = short.MinValue,
                        ParsingNumberStyle = System.Globalization.NumberStyles.Integer,
                        HideUpDownButtons = true
                    };
                    (valueControl as MahApps.Metro.Controls.NumericUpDown).SetBinding(MahApps.Metro.Controls.NumericUpDown.ValueProperty, propName);
                }
                else if (propType == typeof(ushort))
                {
                    valueControl = new MahApps.Metro.Controls.NumericUpDown()
                    {
                        Maximum = ushort.MaxValue,
                        Minimum = ushort.MinValue,
                        ParsingNumberStyle = System.Globalization.NumberStyles.Integer,
                        HideUpDownButtons = true
                    };
                    (valueControl as MahApps.Metro.Controls.NumericUpDown).SetBinding(MahApps.Metro.Controls.NumericUpDown.ValueProperty, propName);
                }
                else if (propType == typeof(float))
                {
                    valueControl = new MahApps.Metro.Controls.NumericUpDown()
                    {
                        Maximum = float.MaxValue,
                        Minimum = float.MinValue,
                        ParsingNumberStyle = System.Globalization.NumberStyles.Float,
                        HideUpDownButtons = true
                    };
                    (valueControl as MahApps.Metro.Controls.NumericUpDown).SetBinding(MahApps.Metro.Controls.NumericUpDown.ValueProperty, propName);
                }
                else if (propType == typeof(byte))
                {
                    valueControl = new MahApps.Metro.Controls.NumericUpDown()
                    {
                        Maximum = byte.MaxValue,
                        Minimum = byte.MinValue,
                        ParsingNumberStyle = System.Globalization.NumberStyles.Integer,
                        HideUpDownButtons = true
                    };
                    (valueControl as MahApps.Metro.Controls.NumericUpDown).SetBinding(MahApps.Metro.Controls.NumericUpDown.ValueProperty, propName);
                }
                else if (propType == typeof(string))
                {
                    valueControl = new TextBox()
                    {
                        MaxWidth = 100,
                        TextWrapping = TextWrapping.Wrap
                    };
                    (valueControl as TextBox).SetBinding(TextBox.TextProperty, propName);
                }
                else if (propType.IsEnum)
                {
                    ComboBox cBox = new ComboBox();
                    Array values = Enum.GetValues(propType);
                    foreach (object eValue in values)
                    {
                        cBox.Items.Add(LocalizationManager.Current.Reward[$"{Type}_{prop.Name}_{eValue.ToString()}"]);
                    }
                    cBox.SetBinding(ComboBox.SelectedItemProperty, new Binding()
                    {
                        Converter = new EnumItemsSource()
                        {
                            Dictionary = LocalizationManager.Current.Reward,
                            LocalizationPrefix = $"{Type}_{prop.Name}_",
                            Type = propType
                        },
                        Path = new PropertyPath(propName),
                        UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged
                    });
                    valueControl = cBox;
                }
                else if (propType == typeof(bool))
                {
                    valueControl = new CheckBox() { };
                    (valueControl as CheckBox).SetBinding(CheckBox.IsCheckedProperty, propName);
                }
                else
                {
                    App.Logger.Log($"{propName} does not have required type '{propType.FullName}'");
                }
                valueControl.HorizontalAlignment = HorizontalAlignment.Right;
                valueControl.VerticalAlignment = VerticalAlignment.Center;
                borderContents.Children.Add(valueControl);
                valueControl.Tag = "variable::" + propName;
                Border b = new Border
                {
                    Child = borderContents
                };
                b.Margin = new Thickness(0, 5, 0, 5);
                if (prop.GetCustomAttribute<RewardOptionalAttribute>() != null)
                {
                    b.Opacity = 0.75;
                }

                yield return b;
            }
        }
        public static IEnumerable<Type> GetTypes()
        {
            foreach (Type t in Assembly.GetExecutingAssembly().GetTypes())
            {
                if (t != null && t.IsSealed && t.IsSubclassOf(typeof(Reward)))
                {
                    yield return t;
                }
            }
        }
        public static string GetLocalizationKey(string typeName)
        {
            string s1 = typeName.Substring(6);
            StringBuilder sb = new StringBuilder();
            foreach (char c in s1)
            {
                if (char.IsUpper(c))
                {
                    sb.Append("_");
                }
                sb.Append(c);
            }
            return sb.ToString();
        }

        public abstract void Give(Simulation simulation);
        public virtual string FormatReward(Simulation simulation)
        {
            return null;
        }
    }
}
