using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnnoBibLibrary.Shared
{
    public static class Tools
    {
        public static string Capitalize(string input)
        {
            string[] splits = input.Split(' ');
            string formattedInput = null;

            foreach (string split in splits)
            { 
                if (split.Length > 0)
                    formattedInput += split.First()
                                           .ToString()
                                           .ToUpper();
                if(split.Length > 1)
                    formattedInput += split.Substring(1)
                                           .ToLower();

                formattedInput += " ";
            }

            formattedInput = formattedInput.Trim();

            return formattedInput;
        }
    }
}
