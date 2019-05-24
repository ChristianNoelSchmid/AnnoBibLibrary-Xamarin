using AnnoBibLibrary.Shared.Bibliography;
using AnnoBibLibrary.Shared.Fields;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnnoBibLibrary.Shared
{
    public class SourceSorter
    {
        // The Library attached to the SourceSorter
        private readonly Library library;

        private string _sortByField = "title";
        // The Field that is being used to sort
        public string SortByField
        {
            get => _sortByField;
            set
            {
                _sortByField = value.ToLower();
                fieldGroups.Clear();
                sortedSources.ToList().ForEach((obj) => GenerateGroup(obj));
                DisplaySourceGroup(null);
            }
        }
        // The List of Field groups for every sorted Source (ie. "Author - C.S. Lewis, J.R.R. Tolkien, ...")
        private SortedSet<string> fieldGroups = new SortedSet<string>();
        // The Collection of sorted Sources
        private SortedSet<Source> sortedSources = new SortedSet<Source>();
        // The Collection of displayed Sources - a ObservableCollection, which updates the Source TableView
        public List<Source> DisplayedSources = new List<Source>();
        // The List of Filter Info associated with the SourceSorter - used so that when Sources
        // are added to the associated Library, the SourceSorter does not need to reimport filter info
        private List<SourceSorterFilterInfo> _filterInfo = new List<SourceSorterFilterInfo>();

        public EventHandler SorterUpdated { get; set; }
        public string[] FieldGroup => fieldGroups.ToArray();

        private const string SORT_BY_ALL_VALUE = "title";
        private readonly string[] NUMERICAL_SEARCH_FIELDS = new string[] { "publish year" };
        private const string KEYWORD_GROUP_START_VALUE = "KeyGroup";

        public SourceSorter(Library library)
        { 
            this.library = library;
        }

        public void ImportFilterInfo(SourceSorterFilterInfo[] filterInfo)
        {
            _filterInfo = new List<SourceSorterFilterInfo>(filterInfo);
            FilterSources();
        }

        // Resets sortedSources, and filters out any sources that do not fit
        // parameters established from filterInfo
        public void FilterSources()
        {
            // Reset sortedSources, adding all library Sources
            sortedSources = new SortedSet<Source>();

            foreach (var source in library.Sources)
            {
                var filteredIn = true;

                // Iterate through each info parameter in filterInfo
                foreach (var info in _filterInfo)
                {
                    var fieldName = info.FieldName.ToLower();

                    // Parse info, checking if it is a ranged search or
                    // a single search (determined by '-')
                    foreach (var sParam in info.Parameters.Split(','))
                    {
                        var param = sParam.ToLower().Trim();
                        if (param.Contains("-"))
                        {
                            string lower = param.Split('-')[0],
                                   upper = param.Split('-')[1];

                            if (NUMERICAL_SEARCH_FIELDS.Contains(info.FieldName))
                            {
                                if (int.TryParse(lower, out int iLower) && int.TryParse(upper, out int iUpper))
                                {
                                    if (!SourceContainsRange(source, fieldName, iLower, iUpper))
                                        filteredIn = false;
                                }
                            }
                            else
                            {
                                if (!SourceContainsRange(source, fieldName, lower, upper))
                                    filteredIn = false;
                            }
                        }
                        else
                        {
                            if (NUMERICAL_SEARCH_FIELDS.Contains(fieldName))
                            {
                                if (int.TryParse(param, out int iParam))
                                {
                                    if (!SourceContains(source, fieldName, param))
                                        filteredIn = false;
                                }
                            }

                            else
                            {
                                if (!SourceContains(source, fieldName, param))
                                    filteredIn = false;
                            }
                        }
                    }

                    if (!filteredIn) break;
                }

                if (filteredIn) sortedSources.Add(source);
                SorterUpdated(this, null);
            }

            foreach(var source in sortedSources)
            {
                GenerateGroup(source);
            }

            if(SortByField == SORT_BY_ALL_VALUE) DisplaySourceGroup(null);
            SorterUpdated(this, null);
        }

        bool SourceContains(Source source, string fieldName, IComparable value)
        {
            Field field;
            if((field = source.GetField(fieldName)) != null)
            {
                if (field.ContainsValue(value)) return true;
            }
            else if(fieldName.Contains(KEYWORD_GROUP_START_VALUE))
            {
                if (source.KeywordGroupContains(
                    fieldName.Replace($"{KEYWORD_GROUP_START_VALUE}: ", ""), value as string))
                    return true;
            }

            return false;
        }

        bool SourceContainsRange(Source source, string fieldName, IComparable lower, IComparable upper)
        {
            Field field;
            if((field = source.GetField(fieldName)) != null)
            {
                if (field.ContainsRange(lower, upper)) return true;
            }
            else if (fieldName.Contains(KEYWORD_GROUP_START_VALUE))
            {
                if (source.KeywordGroupContainsRange(
                    fieldName.Replace($"{KEYWORD_GROUP_START_VALUE}: ", ""),
                    lower as string, upper as string))
                    return true;
            }

            return false;
        }
         
        // Adds the new category to the sorted source
        private void GenerateGroup(Source source)
        {
            var sortByField = source.GetField(SortByField);
            if (sortByField != null)
            {
                foreach (var fieldValue in sortByField.FormattedValues)
                    fieldGroups.Add(fieldValue);
            }

            else 
                fieldGroups.Add($">> {SortByField} Unknown <<");
        }

        public void DisplaySourceGroup(string groupName)
        {
            DisplayedSources.Clear();

            if (SortByField == SORT_BY_ALL_VALUE)
            {
                foreach (var source in sortedSources.AsEnumerable())
                {
                    DisplayedSources.Add(source);
                }
            }
            else if(groupName != null)
            {
                if (fieldGroups.Contains(groupName))
                {
                    foreach (var source in sortedSources)
                    {
                        Field field = source.GetField(SortByField);
                        if (field == null) continue;

                        if (field.ContainsValue(groupName))
                            DisplayedSources.Add(source);
                    }
                }
            }

            SorterUpdated(this, null);
        } 
    }

    // SourceSorter filtering information.
    public struct SourceSorterFilterInfo
    {
        public string FieldName;
        public string Parameters;
    }
}
