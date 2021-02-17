using Project_Nirvash.Helpers;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Project_Nirvash.Models
{
    /// <summary>
    /// Profile class
    /// </summary>
    public class Profile : Observable
    {
        private string _name;
        public string Name
        {
            get => string.Format("AccountName".GetLocalized(), _name);
            set => Set(ref _name, value);
        }

        public List<Event> AbsenceEvents { get; } = new List<Event>();

        public List<Agendum> AgendaEvents { get; } = new List<Agendum>();

        public ObservableCollection<Item> NoticeboardItems { get; internal set; } = new ObservableCollection<Item>();

        public List<Didactict> DidacticsItems { get; } = new List<Didactict>();

        public List<Grade> Grades { get; } = new List<Grade>();

        public List<Note> Notes { get; } = new List<Note>();

        public List<Subject> Subjects { get; } = new List<Subject>();

        public List<Lesson> Lessons { get; } = new List<Lesson>();
    }
}
