using System;
using System.Collections.Generic;
using System.DirectoryServices;
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

namespace LDAP_Auth
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            PopulateLDAPdata();
        }

        public void PopulateLDAPdata()
        {
            string userName = System.Environment.UserName;

            List<string> lstUserData = new List<string>();
            using (DirectoryEntry de = new DirectoryEntry(DomainManager.RootPath))
            {
                using (DirectorySearcher adSearch = new DirectorySearcher(de))
                {
                    adSearch.Filter = String.Format("(sAMAccountName={0})", userName);
                    SearchResult adSearchResult = adSearch.FindOne();

                    
                    if (adSearchResult != null)
                    {
                        DirectoryEntry entry = adSearchResult.GetDirectoryEntry();
                        lstUserData.Add(Convert.ToString(entry.Properties["userPrincipalName"].Value));
                        lstUserData.Add(Convert.ToString(entry.Properties["sAMAccountName"].Value));
                        lstUserData.Add(Convert.ToString(entry.Properties["initials"].Value));
                        lstUserData.Add(Convert.ToString(entry.Properties["Given-Name"].Value));
                        lstUserData.Add(Convert.ToString(entry.Properties["sn"].Value));
                        lstUserData.Add(Convert.ToString(entry.Properties["mail"].Value));
                        lstUserData.Add(Convert.ToString(entry.Properties["mobile"].Value));
                        lstUserData.Add(Convert.ToString(entry.Properties["telephoneNumber"].Value));
                        lstUserData.Add(Convert.ToString(entry.Properties["streetAddress"].Value));
                        lstUserData.Add(Convert.ToString(entry.Properties["postalCode"].Value));
                    }
                    
                }
            }
            lstUserDetails.ItemsSource = lstUserData;
        }

    }
}
