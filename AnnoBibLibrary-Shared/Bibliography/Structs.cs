using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnnoBibLibrary.Shared.Bibliography
{
    public struct Location
    {
        // The city of relevance
        public string City;
        // The region of relevance: ie. state, country, province, etc.
        public string Region;

        public Location(string city, string region)
        {
            City = city;
            Region = region;
        }
    }
}
