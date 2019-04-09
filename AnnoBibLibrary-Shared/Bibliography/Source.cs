using AnnoBibLibrary.Shared.Fields;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// The Source class stores information related to a single bibliography source. All info
// is retrieved from the designated CitationFormat (designated at construction)
// As well as CitationFormat info (ie. Title, Author, Keywords), the Source class
// also stores Note and Quote information, for the user to input
namespace AnnoBibLibrary.Shared.Bibliography
{
    [JsonObject(MemberSerialization.OptIn)]
    public class Source : IComparable
    {
        // Constructor that sets Source to provided Format. Useful for testing purposes
        public Source(CitationFormat citationFormat)
        {
            CitationFormatName = citationFormat.Name;
            foreach (var key in citationFormat.Fields.Keys)
            {
                if (citationFormat.Fields[key].Item1 == typeof(NameField))
                    fields.Add(key, new NameField(key, citationFormat.Fields[key].Item2));

                else if (citationFormat.Fields[key].Item1 == typeof(WordField))
                    fields.Add(key, new WordField(key, citationFormat.Fields[key].Item2));

                else if (citationFormat.Fields[key].Item1 == typeof(NumberField))
                    fields.Add(key, new NumberField(key, citationFormat.Fields[key].Item2));

                else
                    fields.Add(key, new DateField(key, citationFormat.Fields[key].Item2));
            }
        }

        // Private constructor used to load a Source from a Json file. Requires all
        // Json Serialized properties
        [JsonConstructor]
        private Source(string notes, List<Quote> quotes, string citationFormatName,
            Dictionary<string, Field> fields)
        {
            Notes = notes;
            Quotes = quotes;
            CitationFormatName = citationFormatName;

            this.fields = fields;
        }

        // The Source's Notes, user-specific
        [JsonProperty]
        public string Notes { get; set; }

        // All Quotes associated with the Source
        [JsonProperty]
        public List<Quote> Quotes { get; private set; }

        // All Keywords associated with the Source, with their defined keyword
        [JsonProperty]
        private Dictionary<string, List<string>> Keywords { get; set; } = new Dictionary<string, List<string>>();

        // Converts all Keyword groups into a Formatted word, for Client use
        public string[] KeywordGroupsFormatted
        {
            get
            {
                var keywordsFormatted = new List<string>(Keywords.Keys);
                keywordsFormatted.ForEach((group) => group = Tools.Capitalize(group));

                return keywordsFormatted.ToArray();
            }
        }

        /* ************************************************
         * CITATION FORMAT PROPERTIES/METHODS
         * ************************************************
         */
        // The specified format for the particular citation
        [JsonProperty]
        public string CitationFormatName { get; private set; }

        // Dictionary storage of all Fields. This is the collection that will be
        // retrieved from and manipulated by the client.
        [JsonProperty]
        private Dictionary<string, Field> fields = new Dictionary<string, Field>();
        public List<Field> Fields => fields.Values.ToList();

        /* *************************************************
         * Common Fields that will, generally, be universal to all CitationFormats
         * If a Source does not contain the specified Field, or if there is no data in said
         * Field, a formatted string will be provided instead
         * *************************************************
         */
        public string TitleFormatted {
            get
            {
                var field = this["title"];
                if (field == null || field.FormattedValues.Length == 0) return ">> Unknown Source <<";

                else
                    return field.FormattedValues[0];

            }
        }

        public string AuthorsFormatted
        {
            get
            {
                var authorField = this["author"];

                if (authorField == null || authorField.FormattedValues.Length == 0) return ">> Unknown Author <<";

                else return authorField.FormattedValues.Aggregate((concat, next) => concat += " and " + next);
            }
        }

        public string PublisherFormatted
        {
            get
            {
                var publisherField = this["publisher"];

                if (publisherField == null || publisherField.FormattedValues.Length == 0) return ">> Unknown Publisher <<";

                else return publisherField.FormattedValues[0];
            }
        }

        public string PublishYearFormatted
        {
            get
            {
                var publishYearFormatted = this["publish year"];

                if (publishYearFormatted == null || publishYearFormatted.FormattedValues.Length == 0) return ">> Unknown Publish Date <<";

                else return publishYearFormatted.FormattedValues[0];
            }
        }
        /* **************************************************
         */
        // Retrieves a Field from the Source Fields. Requires Field type in order to
        // correctly convert

        public Field this[string key]
        {
            get
            {
                key = key.ToLower();
                if (fields.ContainsKey(key)) return fields[key];
                else return null;
            }
            set
            {
                key = key.ToLower();
                if (fields.ContainsKey(key))
                {
                    if (value is IComparable comp)
                    {
                        fields[key] += comp;
                        return;
                    }
                }
                throw new FieldNotFoundException("Field not found, and cannot be added to.");
            }
        }

        // Converts all keys in the _fields dictionary into an array,
        // Capitalizing each key as it's added
        public string[] FieldNamesFormatted
        {
            get
            {
                string[] keys = new string[fields.Count];
                int counter = 0;

                foreach (var key in fields.Keys)
                {
                    keys[counter] = Tools.Capitalize(key);
                    ++counter;
                }

                return keys;
            }
        }
        /* ************************************************
         * ************************************************
         */


        /* ************************************************
         * EQUALS AND HASHCODE METHODS
         * ************************************************
         */
        // A Source is different if its Title, Author, or Publish Date is different
        // Because of this, those values will be compared in GetHashCode and Equals
        public override int GetHashCode()
        {
            return TitleFormatted.ToLower().GetHashCode() +
                   AuthorsFormatted.ToLower().GetHashCode() * 3 +
                   PublishYearFormatted.GetHashCode();
        }
        public override bool Equals(object obj)
        {
            if (obj is Source source)
            {
                if (source.TitleFormatted.ToLower() == TitleFormatted.ToLower() &&
                    source.AuthorsFormatted.ToLower() == AuthorsFormatted.ToLower() &&
                    source.PublishYearFormatted.ToLower() == PublishYearFormatted.ToLower())
                    return true;

                else return false;
            }

            else return false;
        }
        /* *************************************************
         */

        // Compare the Source to another Source, using the "title" field as a comparator
        // If "Title" does not exist, or if object being compared to isn't a Source, return 0
        public int CompareTo(object obj)
        {
            if (obj is Source source)
            {
                return String.Compare(TitleFormatted, source.TitleFormatted);
            }
            else return 0;
        }

        // Add a new quote to the Source
        public void AddQuote(string quote, int location, params string[] keywords)
        {
            Quotes.Add(new Quote(quote, location));
            Quotes[Quotes.Count - 1].SetKeywords(keywords);
        }

        // Saves the source into the specified directory
        // Saves all Field data
        // Uses Newtonsoft Json
        public void Save(string directory)
        {
            using (StreamWriter writer = new StreamWriter(Path.Combine(directory, $"{GetHashCode().ToString()}.abs")))
            {
                JsonSerializerSettings settings = new JsonSerializerSettings
                {
                    Formatting = Formatting.Indented,
                    TypeNameHandling = TypeNameHandling.Auto
                };
                var output = JsonConvert.SerializeObject(this, settings);
                writer.Write(output);
            }
        }

        // Deserializes a Json file, converting it into a Source
        public static Source Load(string directory)
        {
            Source source = null;
            using (StreamReader reader = new StreamReader(directory))
            {
                string input = reader.ReadToEnd();
                source = JsonConvert.DeserializeObject<Source>(input, new JsonSerializerSettings
                {
                    Formatting = Formatting.Indented,
                    TypeNameHandling = TypeNameHandling.Auto
                });
            }

            return source;
        }

        // Sets all values within a specific KeywordGroup
        public void SetKeywordGroup(string group, params string[] keywords)
        {
            group = group.ToLower().Trim();

            if (Keywords.ContainsKey(group)) Keywords[group].Clear();
            if (keywords == null) return;

            if (!Keywords.ContainsKey(group.ToLower())) Keywords.Add(group, new List<string>(keywords));
            else Keywords[group.ToLower()].AddRange(keywords);
        }

        public string[] GetKeywords(string group)
        {
            group = group.ToLower().Trim();

            if (Keywords.ContainsKey(group)) return Keywords[group].ToArray();
            else throw new FieldNotFoundException("Could not find Keyword Group in source");
        }

        public void RemoveKeywordGroup(string group)
        {
            group = group.ToLower().Trim();

            if (!Keywords.ContainsKey(group.ToLower())) return;
            Keywords.Remove(group.ToLower());
        }

        public void RenameKeywordGroup(string originalName, string newName)
        {
            originalName = originalName.ToLower().Trim();
            newName = newName.ToLower().Trim();

            if (Keywords.ContainsKey(originalName))
            {
                if (!Keywords.ContainsKey(newName)) Keywords.Add(newName, new List<string>(Keywords[originalName]));
                else Keywords[originalName].ForEach((keyword) => Keywords[newName].Add(keyword));
            }
        }

    }
}
