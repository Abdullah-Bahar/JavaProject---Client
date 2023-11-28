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
using System.Windows.Shapes;

namespace JavaProject___Client.MVVM.View
{
    /// <summary>
    /// HomeView_Users.xaml etkileşim mantığı
    /// </summary>
    public partial class HomeViewUsers : UserControl
    {
        public HomeViewUsers()
        {
            InitializeComponent();
            Users.SelectionChanged += Users_SelectionChanged;
        }

        private void Users_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (Users.SelectedItem != null)
            {
                if (sohbet.Items.Count > 0)
                {
                    sohbet.ScrollIntoView(sohbet.Items[sohbet.Items.Count - 1]);
                }
            }
        }

    }
}
