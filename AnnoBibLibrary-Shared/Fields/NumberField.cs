using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnnoBibLibrary.Shared.Fields
{
    public class NumberField : Field
    {
        public NumberField(string fieldName, bool storeMultiple) : base(fieldName, storeMultiple) { }

        protected override Type ValueType => typeof(int);

        public override string[] FormattedValues
        {
            get
            {
                List<string> values = new List<string>(Values.Count);
                for(int i = 0; i < Values.Count; ++i)
                {
                    values.Add(Values[i].ToString());
                }

                return values.ToArray();
            }
        }
    }
}
