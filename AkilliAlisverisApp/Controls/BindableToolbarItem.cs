using Microsoft.Maui.Controls;

namespace AkilliAlisverisApp.Controls
{
    public class BindableToolbarItem : ToolbarItem
    {
        public static readonly BindableProperty IsVisibleProperty =
            BindableProperty.Create(nameof(IsVisible), typeof(bool), typeof(BindableToolbarItem), true, propertyChanged: OnIsVisibleChanged);

        public bool IsVisible
        {
            get => (bool)GetValue(IsVisibleProperty);
            set => SetValue(IsVisibleProperty, value);
        }

        private static void OnIsVisibleChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var item = bindable as BindableToolbarItem;
            if (item == null || item.Parent == null) return;

            var toolbarItems = ((Page)item.Parent).ToolbarItems;

            if ((bool)newValue && !toolbarItems.Contains(item))
                toolbarItems.Add(item);
            else if (!(bool)newValue && toolbarItems.Contains(item))
                toolbarItems.Remove(item);
        }
    }
}