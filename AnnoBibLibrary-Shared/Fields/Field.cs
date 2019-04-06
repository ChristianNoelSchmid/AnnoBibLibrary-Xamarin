using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// The abstract class Field
// The base class for all possible Field types in Sources
// Stores information related to a particular Source Info
// Stores information as IComparable elements, but elements are further
// Defined in children classes (ie. string, int, DateTime)
// (ie. title, author(s), editor(s))
namespace AnnoBibLibrary.Shared.Fields
{
    [JsonObject(MemberSerialization.OptIn)]
    public abstract class Field
    {
       protected virtual Type ValueType { get; }

        [JsonProperty]
        public string FieldName { get; protected set; }

        public virtual string[] FormattedValues { get; }

        // Can the Field store multiple elements?
        [JsonProperty]
        public bool StoreMultiple { get; private set; }

        // Searches for value in the Field collection, checking the field type
        // before searching (using fieldType)
        public virtual bool ContainsValue(IComparable value)
        {
            if (ValueType != value.GetType()) return false;
            foreach (var val in Values)
            {
                if (val.CompareTo(value) == 0) return true;
            }

            return false;
        }

        // Searches for a range of values the Field collection, using a
        // lower and upper bound (of same type), checking
        // both lower and upper value types before searching (using fieldType)
        public virtual bool ContainsRange(IComparable lower, IComparable upper)
        {
            if (ValueType != lower.GetType() || ValueType != upper.GetType()) return false;

            for (int i = 0; i < Values.Count; ++i)
            {
                int compareLower = Values[i].CompareTo(lower);
                int compareUpper = Values[i].CompareTo(upper);

                if (compareLower >= 0 && compareUpper <= 0) return true;
            }

            return false;
        }

        // Constructor - sets value collection to new List,
        // and sets multiple value storage to supplied boolean
        public Field(string fieldName, bool storeMultiple)
        {
            FieldName = fieldName;
            Values = new List<IComparable>();
            StoreMultiple = storeMultiple;
        } 

        // The collection of values represented in the particular Field
        // (ie. "Tolkien, J.R.R.")
        [JsonProperty]
        protected List<IComparable> Values { get; set; }

        public static Field operator +(Field field, IComparable value)
        {
            if (field.ValueType != value.GetType())
                throw new InvalidOperationException($"Cannot add a {value.GetType()} type to a field of type {field.GetType()}.");

            field.Values.Add(value);
            return field;
        }

        // Adds a value to the Field collection, checking the field type
        // before adding (using fieldType)
        public virtual void SetValues(params IComparable[] values)
        {
            Values.Clear();

            if (values == null || values.Length == 0) 
                return;

            if (ValueType != values[0].GetType())
                throw new FieldValueTypeMismatch($"Values of type {values[0].GetType().ToString()} cannot be added to Field of type {ValueType.ToString()}.");

            if (values.Length > 1 && !StoreMultiple)
                throw new MultipleValuesInFieldNotAllowedException("Cannot store multiple Values in Field where !StoreMultiple");

            foreach (IComparable value in values)
            {
                if (!Values.Contains(value))
                    Values.Add(value);
            }
        }

    }
}
