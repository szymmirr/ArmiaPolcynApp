using System;
using System.Windows;
using System.Windows.Controls;

using Microsoft.Phone.Controls;

namespace AppStudio.Controls
{
    public class LongListSelectorEx : LongListSelector
    {
        public LongListSelectorEx()
        {
            SelectionChanged += OnSelectionChanged;
        }

        private void OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            this.SelectedItem = base.SelectedItem;
        }

        public new object SelectedItem
        {
            get { return GetValue(SelectedItemProperty); }
            set { SetValue(SelectedItemProperty, value); }
        }

        public static readonly DependencyProperty SelectedItemProperty =
        DependencyProperty.Register(
            "SelectedItem",
            typeof(object),
            typeof(LongListSelectorEx),
            new PropertyMetadata(null, OnSelectedItemChanged)
        );

        private static void OnSelectedItemChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var selector = (LongListSelectorEx)d;
            selector.SetSelectedItem(e);
        }

        private void SetSelectedItem(DependencyPropertyChangedEventArgs e)
        {
            base.SelectedItem = e.NewValue;
        }
    }
}
