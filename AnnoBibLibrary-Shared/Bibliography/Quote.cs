using System;
using System.Collections.Generic;

namespace AnnoBibLibrary.Shared.Bibliography
{
    public class Quote
    {
        public string Data { get; set; } = null;
        public int Location { get; set; } = -1;
        public HashSet<string> Keywords { get; set; } = new HashSet<string>();

        public Quote(string data, int location)
        {
            Data = data;
            Location = location;
        }

        public void SetKeywords(params string[] keywords) => Keywords = new HashSet<string>(keywords);
    }
}