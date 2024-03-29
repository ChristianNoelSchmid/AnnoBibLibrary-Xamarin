﻿using AnnoBibLibrary.Shared.Fields;
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
            foreach (var field in citationFormat.Fields)
            {
                if (field.FieldType == typeof(NameField))
                    Fields.Add(field.Name, new NameField(field.Name, field.AllowMultiple));

                if (field.FieldType == typeof(WordField))
                    Fields.Add(field.Name, new WordField(field.Name, field.AllowMultiple));

                if (field.FieldType == typeof(NumberField))
                    Fields.Add(field.Name, new NumberField(field.Name, field.AllowMultiple));

                if (field.FieldType == typeof(DateField))
                    Fields.Add(field.Name, new DateField(field.Name, field.AllowMultiple));
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

            this.Fields = fields;
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
        public Dictionary<string, Field> Fields { get; private set; } = new Dictionary<string, Field>();

        /* *************************************************
         * Common Fields that will, generally, be universal to all CitationFormats
         * If a Source does not contain the specified Field, or if there is no data in said
         * Field, a formatted string will be provided instead
         * *************************************************
         */
        public string TitleFormatted {
            get
            {
                var field = GetField("title");
                if (field == null || field.FormattedValues.Length == 0) return ">> Unknown Source <<";

                else
                    return field.FormattedValues[0];

            }
        }

        public string AuthorsFormatted
        {
            get
            {
                var authorField = GetField("author");

                if (authorField == null || authorField.FormattedValues.Length == 0) return ">> Unknown Author <<";

                else return authorField.FormattedValues.Aggregate((concat, next) => concat += " and " + next);
            }
        }

        public string PublisherFormatted
        {
            get
            {
                var publisherField = GetField("publisher");

                if (publisherField == null || publisherField.FormattedValues.Length == 0) return ">> Unknown Publisher <<";

                else return publisherField.FormattedValues[0];
            }
        }

        public string PublishYearFormatted
        {
            get
            {
                var publishYearFormatted = GetField("publish year");

                if (publishYearFormatted == null || publishYearFormatted.FormattedValues.Length == 0) return ">> Unknown Publish Date <<";

                else return publishYearFormatted.FormattedValues[0];
            }
        }
        /* **************************************************
         */
       
        // Converts all keys in the _fields dictionary into an array,
        // Capitalizing each key as it's added
        public string[] FieldNamesFormatted
        {
            get
            {
                string[] keys = new string[Fields.Count];
                int counter = 0;

                foreach (var key in Fields.Keys)
                {
                    keys[counter] = Tools.Capitalize(key);
                    ++counter;
                }

                return keys;
            }
        }

        public Field GetField(string fieldName)
        {
            if (fieldName != null)
            {
                fieldName = fieldName.ToLower().Trim();
                foreach (string key in Fields.Keys)
                {
                    if (fieldName == key.ToLower().Trim())
                        return Fields[key];
                }
            }

            return null;
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

        public bool KeywordGroupContains(string keywordGroup, string value)
        {
            if (Keywords.ContainsKey(keywordGroup))
            {
                foreach (var kwd in Keywords[keywordGroup])
                {
                    if (kwd.ToLower().Contains(value.Trim().ToLower()))
                        return true;
                }
            }

            return false;
        }

        public bool KeywordGroupContainsRange(string keywordGroup, string lower, string upper)
        {
            if (Keywords.ContainsKey(keywordGroup))
            {
                foreach (var kwd in Keywords[keywordGroup])
                {
                    if (string.Compare(kwd, lower, StringComparison.Ordinal) >= 0 
                     && string.Compare(kwd, lower, StringComparison.Ordinal) <= 0)
                        return true;
                }
            }

            return false;
        }
    }
}
