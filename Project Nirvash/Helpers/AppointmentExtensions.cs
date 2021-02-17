using Project_Nirvash.Models;
using System;
using Windows.ApplicationModel.Appointments;

namespace Project_Nirvash.Helpers
{
    /// <summary>
    /// AppointmentExtensions class
    /// </summary>
    public static class AppointmentExtensions
    {
        /// <summary>
        /// Method which creates an Appointment from an Agendum object and calls
        /// default appointment provider to enable the user to add an appointment.
        /// </summary>
        /// <param name="agendum">The agenda event to add to the calendar</param>
        public async static void CreateAppointmentFromAgendum(Agendum agendum)
        {
            if (agendum != null)
            {
                Appointment appointment = new Appointment
                {
                    Subject = agendum.SubjectDescription.NullValueToEmpty(),
                    Details = agendum.Description.NullValueToEmpty(),
                    StartTime = agendum.BeginDateTime,
                    Duration = agendum.EndDateTime - agendum.BeginDateTime,
                    AllDay = agendum.FullDay,
                    Reminder = TimeSpan.FromDays(1)
                };

                await AppointmentManager.ShowEditNewAppointmentAsync(appointment);
            }
        }
    }
}
