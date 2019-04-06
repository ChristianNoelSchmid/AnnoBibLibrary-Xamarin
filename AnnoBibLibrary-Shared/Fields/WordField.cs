using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnnoBibLibrary.Shared.Fields
{
    public class WordField : Field
    {
        public WordField(string fieldName, bool storeMultiple) : base(fieldName, storeMultiple) { }

        protected override Type ValueType => typeof(string);

        public override string[] FormattedValues
        {
            get
            {
                string[] values = new string[Values.Count];
                for(int i = 0; i < Values.Count; ++i)
                {
                    values[i] = (string)Values[i];
                }

                return values;
            }
        }

        // Override method for ContainsValue, which accomodates for strings
        // partial matches will return true, case-insensitive
        public override bool ContainsValue(IComparable value)
        {
            if (value.GetType() != typeof(string)) return false;

            string val = value.ToString().ToLower();
            for(int i = 0; i < Values.Count; ++i)
            {
                if (((string)Values[i]).ToLower().Contains(val)) return true;
            }

            // Also search through formatted values, as user may search in this format
            // (ie. "lewis, c.s." vs. "C.S. Lewis")
            string[] formattedValues = FormattedValues;
            for(int i = 0; i < formattedValues.Length; ++i)
            {
                if (formattedValues[i].ToLower().Contains(val)) return true;
            }

            return false;
        }

        public override bool ContainsRange(IComparable lower, IComparable upper)
        {
            if (lower.GetType() != typeof(string) || upper.GetType() != typeof(string)) return false;

            string low = lower.ToString().ToLower();
            string up = upper.ToString().ToLower();

            // First, compare lower and upper bound values to
            // all values in collection
            for (int i = 0; i < Values.Count; ++i)
            {
                int compareLower = string.Compare(((string)Values[i]).ToLower(), low.ToLower());
                int compareUpper = string.Compare(((string)Values[i]).ToLower(), up.ToLower());

                if (compareLower >= 0 && compareUpper <= 0) return true;
            }

            // Then, compare lower and upper bound values to
            // all FormattedValues
            string[] formattedValues = FormattedValues;
            for (int i = 0; i < FormattedValues.Length; ++i)
            {
                int compareLower = string.Compare(formattedValues[i].ToLower(), low.ToLower());
                int compareUpper = string.Compare(formattedValues[i].ToLower(), up.ToLower());

                if (compareLower >= 0 && compareUpper <= 0) return true;
            }

            return false;
        }
    }
}
