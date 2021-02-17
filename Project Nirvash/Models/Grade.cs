using Newtonsoft.Json;
using Project_Nirvash.Helpers;
using System;
using Windows.UI.Xaml.Media;

namespace Project_Nirvash.Models
{
    /// <summary>
    /// Grade class
    /// </summary>
    public class Grade
    {
        public const string BLUE = "blue";
        public const string GREEN = "green";
        public const string RED = "red";

        [JsonProperty("subjectId")]
        public int SubjectID { get; set; }

        [JsonProperty("subjectCode")]
        public string SubjectCode { get; set; }

        [JsonProperty("subjectDesc")]
        public string SubjectDesc { get; set; }

        [JsonProperty("evtId")]
        public int ID { get; set; }

        [JsonProperty("evtCode")]
        public string Code { get; set; }

        [JsonProperty("evtDate")]
        public DateTime Date { get; set; }

        [JsonProperty("decimalValue")]
        public float? DecimalValue { get; set; }

        [JsonProperty("displayValue")]
        public string DisplayValue { get; set; }

        [JsonProperty("displaPos")]
        public int Position { get; set; }

        private string _notesForFamily;
        [JsonProperty("notesForFamily")]
        public string NotesForFamily
        {
            get => string.IsNullOrWhiteSpace(_notesForFamily) ? "EmptyGradeNotesForFamily".GetLocalized() : _notesForFamily;
            set => _notesForFamily = value;
        }

        [JsonProperty("color")]
        public string Color { get; set; }

        [JsonProperty("canceled")]
        public bool Canceled { get; set; }

        [JsonProperty("underlined")]
        public bool Underlined { get; set; }

        [JsonProperty("periodPos")]
        public int PeriodPosition { get; set; }

        [JsonProperty("periodDesc")]
        public string PeriodDescription { get; set; }

        [JsonProperty("componentPos")]
        public int ComponentPosition { get; set; }

        [JsonProperty("componentDesc")]
        public string ComponentDescription { get; set; }

        [JsonProperty("weightFactor")]
        public int WeightFactor { get; set; }

        [JsonProperty("skillId")]
        public int SkillID { get; set; }

        [JsonProperty("gradeMasterId")]
        public int GradeMasterID { get; set; }

        [JsonProperty("skillDesc")]
        public object SkillDescription { get; set; }

        [JsonProperty("skillCode")]
        public object SkillCode { get; set; }

        [JsonProperty("skillMasterId")]
        public int SkillMasterID { get; set; }

        [JsonProperty("oldskillId")]
        public int OldSkillID { get; set; }

        [JsonProperty("oldskillDesc")]
        public string OldSkillDescription { get; set; }

        public SolidColorBrush Brush
        {
            get
            {
                switch(Color)
                {
                    case GREEN:
                        return new SolidColorBrush(Windows.UI.Color.FromArgb(120, 0, 255, 0));
                    case RED:
                        return new SolidColorBrush(Windows.UI.Color.FromArgb(120, 255, 0, 0));
                    default:
                        return new SolidColorBrush(Windows.UI.Color.FromArgb(120, 0, 0, 255));
                }
            }
        }
    }
}
