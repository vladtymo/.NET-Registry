using Microsoft.Win32;
using System;
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
using System.Windows.Navigation;
using System.IO;

namespace Reg_viewer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            RegistryKey[] rootKeys = new[]
            {
                Registry.ClassesRoot,
                Registry.CurrentUser,
                Registry.LocalMachine,
                Registry.Users,
                Registry.CurrentConfig
            };

            foreach (var k in rootKeys)
            {
                var item = new TreeViewItem()
                {
                    Header = k.Name,
                    IsExpanded = false,
                    Tag = k
                };
                item.Expanded += SubItem_Expanded;

                tree.Items.Add(item);
                LoadSubKeys(item);
            }
        }

        private void LoadSubKeys(TreeViewItem item)
        {
            //return Task.Run(() =>
            //{
            RegistryKey key = null;
            //this.Dispatcher.Invoke(() =>
            //{
                key = (item.Tag as RegistryKey);
            //});

            if (key == null) return;
            if (key.SubKeyCount <= 0) return;

            try
            {
                foreach (var name in key.GetSubKeyNames())
                {
                    try
                    {
                        var subKey = key.OpenSubKey(name);
                        //this.Dispatcher.Invoke(() =>
                        //{
                            var subItem = new TreeViewItem()
                            {
                                IsExpanded = false,
                                Header = Path.GetFileName(subKey.Name),
                                Tag = subKey
                            };
                            subItem.Expanded += SubItem_Expanded;

                            item.Items.Add(subItem);
                        //});
                    }
                    catch { }
                }
            }
            catch { }
            //});
        }

        private void SubItem_Expanded(object sender, RoutedEventArgs e)
        {
            var item = (sender as TreeViewItem);
            foreach (TreeViewItem sub in item.Items)
            {
                LoadSubKeys(sub);
            }
        }

        private void Tree_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            try
            {
                if (tree.SelectedItem == null) return;
                var key = (RegistryKey)((TreeViewItem)tree.SelectedItem).Tag;

                currentPath.Text = key.Name;

                propertyGrid.ItemsSource = key.GetValueNames().Select(name => new
                {
                    Name = name,
                    Kind = key.GetValueKind(name),
                    Value = key.GetValue(name)
                });

            }
            catch { }
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            // TODO
        }
    }
}
