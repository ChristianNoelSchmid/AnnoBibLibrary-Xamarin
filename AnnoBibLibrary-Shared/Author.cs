using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// The class that contians information about an Author's name
namespace AnnoBibLibrary.Shared
{
    public class Author
    {
        public static readonly Author Unknown = new Author(null, null);
        public Author(string firstName, string lastName)
        {
            FirstName = firstName;
            LastName = lastName;
        }
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}
