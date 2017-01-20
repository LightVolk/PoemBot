using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PoemBot.Poems
{
    /// <summary>
    /// Стихотворение и информация о нем
    /// </summary>
    public class Poem
    {
        public String HashId { get; set; }
        public String Title { get; set; }
        public List<string> Lines { get; set; }
        public String Url { get; set; }
        public int Hits { get; set; }
        public int Likes { get; set; }

        public Poem()
        {
            HashId = String.Empty;
            Title = String.Empty;
            Url = String.Empty;
            Lines = new List<string>();
        }
    }
}