using AnnoBibLibrary.Shared.Bibliography;
using AnnoBibLibrary.Shared.Fields;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnnoBibLibrary.Shared
{
    public class SourceSorter
    {
        private Source[] _sources;
        public SortedList<string, SortedSet<Source>> SourcesSorted { get; private set; } = new SortedList<string, SortedSet<Source>>();
        public string SortByField { get; set; }

        public SourceSorter(Library library)
        {
            _sources = library.Sources;
        }

        // Sorts through the Source collection, filtering for particular sources
        // Can have multiple values to sort through: if this is the case, all values must 
        // be found in single source
        // The list values act as AND while the function itself acts as OR
        public void FilterSourcesSingle(string fieldName, IComparable value)
        {
            fieldName = fieldName.ToLower();

            foreach (var source in _sources)
            {
                Field field = source[fieldName];
                if (field != null)
                {
                    if (field.ContainsValue(value))
                        AddToSortedSources(source);
                }
            }
        }

        // Sorts through the Source collection, filtering for a source that falls
        // within the bounds of lower and upper
        public void FilterSourceRange(string fieldName, IComparable lower, IComparable upper)
        {
            fieldName = fieldName.ToLower();

            foreach (var source in _sources)
            {
                Field field = source[fieldName];
                if (field != null)
                {
                    if (field.ContainsRange(lower, upper))
                        AddToSortedSources(source);
                }
            }
        }

        private void AddToSortedSources(Source source)
        {
            if (SortByField.ToLower() == "all")
            {
                if (!SourcesSorted.ContainsKey("all"))
                    SourcesSorted.Add("all", new SortedSet<Source>());

                SourcesSorted["all"].Add(source);
            }

            else
            {
                Field sortByField = source[SortByField];
                if (sortByField != null)
                {
                    string[] sortByValues = sortByField.FormattedValues;

                    foreach (var val in sortByValues)
                    {
                        if (!SourcesSorted.ContainsKey(val))
                            SourcesSorted.Add(val, new SortedSet<Source>());

                        SourcesSorted[val].Add(source);
                    }
                }
                else
                {
                    var newKey = $">>{Tools.Capitalize(SortByField)} Unknown<<";
                    if (!SourcesSorted.ContainsKey(newKey))
                        SourcesSorted.Add(newKey, new SortedSet<Source>());

                    SourcesSorted[newKey].Add(source);
                }
            }
        }
    }
}
