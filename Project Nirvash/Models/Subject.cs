using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;

namespace Project_Nirvash.Models
{
    /// <summary>
    /// Subject class
    /// </summary>
    public class Subject
    {
        [JsonProperty("id")]
        public int ID { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("order")]
        public int Order { get; set; }

        [JsonProperty("teachers")]
        public List<Teacher> Teachers { get; set; }

        public List<Grade> FirstPeriodGrades = new List<Grade>();

        public float FirstPeriodGradesAverage => GetGradesAverage(FirstPeriodGrades);

        public List<Grade> SecondPeriodGrades = new List<Grade>();

        public float SecondPeriodGradesAverage => GetGradesAverage(SecondPeriodGrades);

        public static Subject GetSubjectFromTeacher(List<Subject> subjects, Teacher teacher)
        {
            return subjects.FirstOrDefault(item => item.Teachers.Contains(teacher));
        }

        private static float GetGradesAverage(List<Grade> gradesCollection)
        {
            List<Grade> grades = gradesCollection.Where(item => item.DecimalValue != null && item.Color != Grade.BLUE).ToList();
            return grades.Count > 0 ? grades.Average(item => (float)item.DecimalValue) : 0;
        }
    }
}
