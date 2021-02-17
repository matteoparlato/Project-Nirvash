using Project_Nirvash.Helpers;
using Project_Nirvash.Models;
using System.Collections.Generic;
using System.Linq;
using Windows.UI.Xaml.Controls;

namespace Project_Nirvash.Dialogs
{
    /// <summary>
    /// LessonsDetailsDialog class
    /// </summary>
    public sealed partial class LessonsDetailsDialog : ContentDialog
    {
        internal List<Lesson> Lessons { get; }

        /// <summary>
        /// Constructor which initializes a LessonsDetailsDialog object with passed information.
        /// </summary>
        /// <param name="lessons">The list of lessons to display</param>
        public LessonsDetailsDialog(List<Lesson> lessons)
        {
            this.InitializeComponent();

            Title = string.Format("LessonsDetailsTitle".GetLocalized(), lessons[0].LessonDate.Date.ToShortDateString());
            Lessons = lessons.OrderBy(item => item.Position).ToList();
        }
    }
}
