using AnnoBibLibrary.Shared.Fields;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// A CitationFormat hosts the information regarding a Source's bibliography
// Also formats data based on specified string
namespace AnnoBibLibrary.Shared.Bibliography
{
    public class CitationFormat
    {
        public string Name { get; set; }
        // The Dictionary of all the Fields associated with the CitationFormat. The key holds the name of
        // the Field, while the Tuple holds the Type (ie. WordField, NumberField) and whether or not the
        // Field allows multiple values (bool)
        public List<FieldInfo> Fields { get; private set; } = new List<FieldInfo>();

        public CitationFormat(string name)
        {
            Name = name;
        }

        // The formatted print of the Source, which will fill brackets with specific field info
        // Format follows a specific arrangement of parsing. The string itself will be filled by
        // GUI input from the User
        public string Print { get; set; } = 
            "[Author Name|split=\", \"|last=\" and\"]. " +
            "[if Editor Name] ed. [Editor Name|spacer=\", \"|last=\" and \"] " +
            "[endif]. {Title: italics}. {Publisher City}: {Publisher Name:italics}, " +
            "{Publisher Year}. {Citation Format Name}";

        // Adds a new Field to the CitationFormat, formatting it as it's added
        // Adds specific type (for now, either string or int) to specify what kind of
        // Field it is
        // Throws FormatException if fieldName is empty, or if the key already exists in the CitationFormat
        public void AddField(string fieldName, Type type, bool allowMultiple)
        {
            if (string.IsNullOrEmpty(fieldName)) throw new FormatException("New field cannot be null or empty.");

            Fields.ForEach((field) =>
            {
                if (field.Name == fieldName.ToLower().Trim())
                    throw new FormatException($"Key \"{fieldName}\" already defined in CitationFormat \"{Name}\".");
            });
        

            Fields.Add(new FieldInfo {
                Name = fieldName,
                FieldType = type,
                AllowMultiple = allowMultiple
            });
        }
    }

    public struct FieldInfo
    {
        public string Name;
        public Type FieldType;
        public bool AllowMultiple;
    }
}
