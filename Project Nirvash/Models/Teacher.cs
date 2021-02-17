using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace Project_Nirvash.Models
{
    /// <summary>
    /// Teacher class
    /// </summary>
    public class Teacher : IEquatable<Teacher>
    {
        [JsonProperty("teacherId")]
        public string ID { get; set; }

        [JsonProperty("teacherName")]
        public string Name { get; set; }

        public Teacher(string id, string name)
        {
            ID = id;
            Name = name;
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as Teacher);
        }

        public bool Equals(Teacher other)
        {
            return other != null
                && (string.Compare(ID, other.ID, true) == 0 || string.Compare(Name, other.Name, true) == 0);
        }

        public override int GetHashCode()
        {
            int hashCode = 1479869798;
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(ID);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Name);
            return hashCode;
        }
    }
}
