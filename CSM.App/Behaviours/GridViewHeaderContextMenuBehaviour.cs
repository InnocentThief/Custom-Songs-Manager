using System.ComponentModel;
using System.Windows;
using System.Windows.Data;
using Telerik.Windows;
using Telerik.Windows.Controls;
using Telerik.Windows.Controls.GridView;

namespace CSM.App.Behaviours
{
    internal class GridViewHeaderContextMenuBehaviour(RadGridView grid)
    {
        private readonly RadGridView grid = grid;
        public static readonly DependencyProperty IsEnabledProperty
            = DependencyProperty.RegisterAttached("IsEnabled", typeof(bool), typeof(GridViewHeaderContextMenuBehaviour),
                new PropertyMetadata(new PropertyChangedCallback(OnIsEnabledPropertyChanged)));

        public static void SetIsEnabled(DependencyObject dependencyObject, bool enabled)
        {
            dependencyObject.SetValue(IsEnabledProperty, enabled);
        }

        public static bool GetIsEnabled(DependencyObject dependencyObject)
        {
            return (bool)dependencyObject.GetValue(IsEnabledProperty);
        }

        private static void OnIsEnabledPropertyChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs e)
        {
            if (dependencyObject is RadGridView grid)
            {
                if ((bool)e.NewValue)
                {
                    // Create new GridViewHeaderMenu and attach RowLoaded event.
                    GridViewHeaderContextMenuBehaviour menu = new GridViewHeaderContextMenuBehaviour(grid);
                    menu.Attach();
                }
            }
        }

        private void Attach()
        {
            if (grid != null)
            {
                // create menu
                RadContextMenu contextMenu = new();
                // set menu Theme
                StyleManager.SetTheme(contextMenu, StyleManager.GetTheme(grid));

                contextMenu.Opened += OnMenuOpened;
                contextMenu.ItemClick += OnMenuItemClick;

                RadContextMenu.SetContextMenu(grid, contextMenu);
            }
        }

        void OnMenuOpened(object sender, RoutedEventArgs e)
        {
            RadContextMenu menu = (RadContextMenu)sender;
            GridViewHeaderCell cell = menu.GetClickedElement<GridViewHeaderCell>();

            if (cell != null)
            {
                menu.Items.Clear();

                RadMenuItem item = new RadMenuItem
                {
                    Header = String.Format(@"Sort Ascending by ""{0}""", cell.Column.Header)
                };
                menu.Items.Add(item);

                item = new RadMenuItem
                {
                    Header = String.Format(@"Sort Descending by ""{0}""", cell.Column.Header)
                };
                menu.Items.Add(item);

                item = new RadMenuItem
                {
                    Header = String.Format(@"Clear Sorting by ""{0}""", cell.Column.Header)
                };
                menu.Items.Add(item);

                item = new RadMenuItem
                {
                    Header = String.Format(@"Group by ""{0}""", cell.Column.Header)
                };
                menu.Items.Add(item);

                item = new RadMenuItem
                {
                    Header = String.Format(@"Ungroup ""{0}""", cell.Column.Header)
                };
                menu.Items.Add(item);

                item = new RadMenuItem
                {
                    Header = "Choose Columns:"
                };
                menu.Items.Add(item);

                // create menu items
                foreach (GridViewColumn column in grid.Columns)
                {
                    RadMenuItem subMenu = new RadMenuItem
                    {
                        Header = column.Header,
                        IsCheckable = true,
                        IsChecked = true
                    };

                    Binding isCheckedBinding = new Binding("IsVisible")
                    {
                        Mode = BindingMode.TwoWay,
                        Source = column
                    };

                    // bind IsChecked menu item property to IsVisible column property
                    subMenu.SetBinding(RadMenuItem.IsCheckedProperty, isCheckedBinding);

                    item.Items.Add(subMenu);
                }
            }
            else
            {
                menu.IsOpen = false;
            }
        }

        void OnMenuItemClick(object sender, RoutedEventArgs e)
        {
            RadContextMenu menu = (RadContextMenu)sender;

            GridViewHeaderCell cell = menu.GetClickedElement<GridViewHeaderCell>();
            RadMenuItem? clickedItem = ((RadRoutedEventArgs)e).OriginalSource as RadMenuItem;
            GridViewColumn column = cell.Column;

            if (clickedItem == null) return;

            if (clickedItem.Parent is RadMenuItem)
                return;

            string header = Convert.ToString(clickedItem.Header);
            if (header == null) return;

            using (grid.DeferRefresh())
            {
                ColumnSortDescriptor sd = (from d in grid.SortDescriptors.OfType<ColumnSortDescriptor>()
                                           where object.Equals(d.Column, column)
                                           select d).FirstOrDefault();

                if (header.Contains("Sort Ascending"))
                {
                    if (sd != null)
                    {
                        grid.SortDescriptors.Remove(sd);
                    }

                    ColumnSortDescriptor newDescriptor = new ColumnSortDescriptor();
                    newDescriptor.Column = column;
                    newDescriptor.SortDirection = ListSortDirection.Ascending;

                    grid.SortDescriptors.Add(newDescriptor);
                }
                else if (header.Contains("Sort Descending"))
                {
                    if (sd != null)
                    {
                        grid.SortDescriptors.Remove(sd);
                    }

                    ColumnSortDescriptor newDescriptor = new ColumnSortDescriptor();
                    newDescriptor.Column = column;
                    newDescriptor.SortDirection = ListSortDirection.Descending;

                    grid.SortDescriptors.Add(newDescriptor);
                }
                else if (header.Contains("Clear Sorting"))
                {
                    if (sd != null)
                    {
                        grid.SortDescriptors.Remove(sd);
                    }
                }
                else if (header.Contains("Group by"))
                {
                    ColumnGroupDescriptor gd = (from d in grid.GroupDescriptors.OfType<ColumnGroupDescriptor>()
                                                where object.Equals(d.Column, column)
                                                select d).FirstOrDefault();

                    if (gd == null)
                    {
                        ColumnGroupDescriptor newDescriptor = new ColumnGroupDescriptor();
                        newDescriptor.Column = column;
                        newDescriptor.SortDirection = ListSortDirection.Ascending;
                        grid.GroupDescriptors.Add(newDescriptor);
                    }
                }
                else if (header.Contains("Ungroup"))
                {
                    ColumnGroupDescriptor gd = (from d in grid.GroupDescriptors.OfType<ColumnGroupDescriptor>()
                                                where object.Equals(d.Column, column)
                                                select d).FirstOrDefault();
                    if (gd != null)
                    {
                        grid.GroupDescriptors.Remove(gd);
                    }
                }
            }
        }
    }
}
