using System.Collections.Generic;

namespace Project_Nirvash.Models
{
    /// <summary>
    /// NotesResponse class
    /// </summary>
    public class NotesResponse
    {
        public List<Note> NTTE { get; set; }
        public List<Note> NTCL { get; set; }
        public List<Note> NTWN { get; set; }
        public List<Note> NTST { get; set; }
    }
}
