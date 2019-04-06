using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnnoBibLibrary.Shared.Fields
{
    public class DateField : Field
    {
        public DateField(string fieldName, bool storeMultiple) : base(fieldName, storeMultiple) { }

        protected override Type ValueType => typeof(DateTime);

        public override string[] FormattedValues
        {
            get
            {
                List<string> values = new List<string>(Values.Count);
                for (int i = 0; i < Values.Count; ++i)
                {
                    values.Add(((DateTime)Values[i]).ToShortDateString());
                }

                return values.ToArray();
            }
        }
    }
}
