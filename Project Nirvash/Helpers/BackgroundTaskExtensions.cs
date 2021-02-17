using System;
using System.Linq;
using Windows.ApplicationModel.Background;

namespace Project_Nirvash.Helpers
{
    /// <summary>
    /// BackgroundTaskExtensions class
    /// </summary>
    internal static class BackgroundTaskExtensions
    {
        private const string TASK_NAME = "UpdaterTask";

        public const string EVENTS = "Events";
        public const string APPOINTMENTS = "Appointments";
        public const string ITEMS = "Items";
        public const string DIDACTICS = "Didactics";
        public const string GRADES = "Grades";
        public const string NOTES = "Notes";

        /// <summary>
        /// Method which checks if a background task has been alredy registred
        /// for the current app.
        /// </summary>
        /// <param name="taskName">The name of the background task</param>
        /// <returns>True if the task exists, false otherwise</returns>
        public static bool IsTaskRegistered(string taskName) => BackgroundTaskRegistration.AllTasks.Any(item => item.Value.Name.Equals(taskName, StringComparison.Ordinal));

        /// <summary>
        /// Method which registers a new background task for the app using a
        /// TimeTrigger.
        /// </summary>
        /// <param name="freshnessTime">The interval of time</param>
        internal static void Register(uint freshnessTime)
        {
            if (!IsTaskRegistered(TASK_NAME))
            {
                BackgroundTaskBuilder builder = new BackgroundTaskBuilder
                {
                    Name = TASK_NAME
                };

                builder.SetTrigger(new TimeTrigger(freshnessTime, false));
                builder.Register();
            }
        }

        /// <summary>
        /// Method which unregisters all the background tasks of the app.
        /// </summary>
        internal static void Unregister()
        {
            foreach (var task in BackgroundTaskRegistration.AllTasks)
            {
                task.Value.Unregister(true);
            }
        }
    }
}
