using Project_Nirvash.Core.Helpers;
using Project_Nirvash.Helpers;
using Project_Nirvash.Models;
using Windows.UI.Xaml.Controls;

namespace Project_Nirvash.Views
{
    /// <summary>
    /// NotesPage class
    /// </summary>
    public sealed partial class NotesPage : Page
    {
        internal Profile AccountDetails = Singleton<ClientExtensions>.Instance.AccountDetails;

        /// <summary>
        /// Parameterless constructor of NotesPage class.
        /// </summary>
        public NotesPage()
        {
            InitializeComponent();
        }
    }
}
