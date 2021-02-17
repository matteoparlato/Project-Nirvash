using Windows.UI.Xaml.Controls;

namespace Project_Nirvash.Dialogs
{
    /// <summary>
    /// ContentMessageDialog class
    /// </summary>
    public sealed partial class ContentMessageDialog : ContentDialog
    {
        string Message { get; }

        /// <summary>
        /// Constructor which initializes a ContentMessageDialog object with passed information.
        /// </summary>
        /// <param name="message">The content message</param>
        /// <param name="title">The name of the content</param>
        public ContentMessageDialog(string message, string title)
        {
            this.InitializeComponent();

            Message = message;
            this.Title = title;
        }
    }
}
