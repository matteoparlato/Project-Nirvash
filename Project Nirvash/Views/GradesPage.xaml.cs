using Project_Nirvash.Core.Helpers;
using Project_Nirvash.Helpers;
using Project_Nirvash.Models;
using Windows.UI.Xaml.Controls;

namespace Project_Nirvash.Views
{
    /// <summary>
    /// GradesPage class
    /// </summary>
    public sealed partial class GradesPage : Page
    {
        internal Profile AccountDetails = Singleton<ClientExtensions>.Instance.AccountDetails;

        /// <summary>
        /// Parameterless constructor of GradesPage class.
        /// </summary>
        public GradesPage()
        {
            InitializeComponent();
        }
    }
}
