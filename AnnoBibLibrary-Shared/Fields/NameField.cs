using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnnoBibLibrary.Shared.Fields
{ 
    public class NameField : WordField 
    {
        public NameField(string fieldName, bool storeMultiple) : base(fieldName, storeMultiple) { }

        // Supplies the Name values, formatted so that
        // the individual's last name appears first (ie. "Smith, John")
        public override string[] FormattedValues
        {
            get
            {
                // Take each last word in the list
                // and append it to the front, with a comma
                // ie. Smith, John
                var names = new List<string>();
                for (int i = 0; i < Values.Count; ++i)
                {
                    // Convert the IComparable to string
                    string value = (string)Values[i];

                    // Put Last name first, if there is more than one word in name
                    int last_index = value.LastIndexOf(" ");
                    if (last_index == -1)
                    {
                        names.Add(value);
                    }
                    else
                    {
                        string formatted_name = "";

                        formatted_name += value.Substring(last_index + 1) + ", ";
                        if (formatted_name != value) formatted_name += value.Substring(0, last_index);

                        names.Add(formatted_name);
                    }
                }

                return names.ToArray();
            }
        }
    }
}
