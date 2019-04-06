using AnnoBibLibrary.Shared.Bibliography;
using AnnoBibLibrary.Shared.Fields;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// The Library class stores a collection of Sources, and allows the
// filtering of Sources based on supplied keywords, and the sorting
// of sources based on a shared Field
namespace AnnoBibLibrary.Shared
{
    [JsonObject(MemberSerialization.OptIn)]
    public class Library
    {
        private HashSet<Source> _sources = new HashSet<Source>();
        public Source[] Sources => _sources.ToArray(); 

        [JsonProperty]
        public string Name { get; private set; }

        // All default Keywords associated with this Library
        // Sources will contain their own seperate list of keywords and groups, but
        // These keywords will be avaialble as default for a new Source, or if the client
        // wishes to add to a Source being edited
        [JsonProperty]
        private List<string> DefaultKeywordGroups { get; set; }
        public string[] DefaultKeywordGroupsFormatted
        {
            get => DefaultKeywordGroups.Select((group) => group = Tools.Capitalize(group)).ToArray();
        }

        public Library(string name)
        {
            List<string> strings = new List<string>();
            Name = name;
        }

        [JsonConstructor]
        public Library(string name, string[] defaultKeywordGroups, int[] sourceHashCodes)
        {
            Name = name;
            if (defaultKeywordGroups.Length == 0) DefaultKeywordGroups = new List<string>(new string[] { "Keywords" });
            else DefaultKeywordGroups = new List<string>(defaultKeywordGroups);

            foreach (int hashCode in sourceHashCodes)
                _sources.Add(Source.Load($"C:\\Users\\Christian\\Documents\\AnnoBibLibrary\\Sources\\{hashCode}.abs"));
        }

        public bool AddSource(Source source) => _sources.Add(source);

        // Adds a new Default Keyword Group to the Library, to be considered for each new and edited Source
        public bool SetDefaultKeywordGroups(params string[] groupNames)
        {
            DefaultKeywordGroups = new List<string>();

            foreach (string groupName in groupNames)
                DefaultKeywordGroups.Add(groupName);

            return true;
        }

        public bool RemoveDefaultKeywordGroup(string groupName)
        {
            return DefaultKeywordGroups.Remove(groupName);
        }


/* ************************************************
 * EXTERNAL DATA MEMBERS
 * ************************************************
 */
        // Creates an array of the the Sources hash codes, to be used during
        // serialization. The Library external data file will contain a list of all
        // Source hash codes, which will be used to load the Sources upon loading the Library
        [JsonProperty]
        public int[] SourceHashCodes
        {
            get
            {
                return _sources.Select((source) => source.GetHashCode()).ToArray();
            }
        }

        public bool Save(string directory)
        {
            using (StreamWriter writer = new StreamWriter(Path.Combine(directory, $"{Name}.abl")))
            {
                JsonSerializerSettings settings = new JsonSerializerSettings { Formatting = Formatting.Indented };
                var output = JsonConvert.SerializeObject(this, settings);
                writer.Write(output);
            }

            return true;
        }
    }
}
