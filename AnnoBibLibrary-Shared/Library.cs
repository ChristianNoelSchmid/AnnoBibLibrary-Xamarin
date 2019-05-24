using AnnoBibLibrary.Shared.Bibliography;
using AnnoBibLibrary.Shared.Fields;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
        public SortedSet<Source> Sources { get; private set; }
            = new SortedSet<Source>();

        [JsonProperty]
        public string Name { get; private set; }

        [JsonProperty]
        public string Version { get; private set; }

        // All default Keywords associated with this Library
        // Sources will contain their own seperate list of keywords and groups, but
        // These keywords will be avaialble as default for a new Source, or if the client
        // wishes to add to a Source being edited
        [JsonProperty]
        private List<string> KeywordGroups { get; set; } = new List<string>();
        public string[] KeywordGroupsFormatted
        {
            get => KeywordGroups.Select((group) => group = Tools.Capitalize(group)).ToArray();
        }

        public Library(string name)
        {
            Name = name;
            Version = "0.0.2";
            SetKeywordGroups("Keywords");
        }

        [JsonConstructor]
        public Library(string name, string version, string[] defaultKeywordGroups, int[] sourceHashCodes)
        {
            Name = name;
            Version = version;
            if (defaultKeywordGroups.Length == 0) SetKeywordGroups("Keywords");
            else KeywordGroups = new List<string>(defaultKeywordGroups);

            foreach (int hashCode in sourceHashCodes)
                Sources.Add(Source.Load($"C:\\Users\\Christian\\Documents\\AnnoBibLibrary\\Sources\\{hashCode}.abs"));
        }

        public bool AddSource(Source source)
        {
            if (Sources.Contains(source)) return false;

            Sources.Add(source); 
            return true;
        }

        // Adds new Keyword groups to the Library
        // Automatically compares supplied array with current Keyword Group list
        // Replacing each Keyword in the Source collection with the new keyword
        // Checking for confirmation will be required in the User Interface
        public bool SetKeywordGroups(params string[] groupNames)
        {
            // Index to both collections - must be local to entire method, as its used past the
            // for loop
            int index;
            for (index = 0; index < groupNames.Length && index < KeywordGroups.Count; ++index)
            {
                // Check each string in groupNames to see if it does not match the Keyword Group
                // If it doesn't, replace the keyword group for all sources
                if(groupNames[index].ToLower().Trim() != KeywordGroups[index])
                    RenameKeywordGroup(KeywordGroups[index], groupNames[index].ToLower().Trim());
            }

            // Remove any extra Keyword Groups at the end of the list
            if (index < KeywordGroups.Count)
                KeywordGroups.RemoveRange(index, KeywordGroups.Count - (index));

            // Add any extra new group names to Keyword Groups
            else if(index < groupNames.Length)
            {
                for(; index < groupNames.Length; ++index)
                    KeywordGroups.Add(groupNames[index].ToLower().Trim());
            }
    
            // At the end, when all is said and done, sort the list
            KeywordGroups.Sort();

            Save(GlobalResources.LibrariesDirectory, true);

            return true;
        }

        /// <summary>
        /// Renames a keyword group,
        /// changes each Source in the library to reflect the change
        /// </summary>
        private void RenameKeywordGroup(string originalName, string newName)
        {
            originalName = originalName.ToLower().Trim();
            newName = newName.ToLower().Trim();

            for(int i = 0; i < KeywordGroups.Count; ++i)
            {
                if(KeywordGroups[i] == originalName)
                {
                    KeywordGroups[i] = newName;

                    foreach(var source in Sources)
                        source.RenameKeywordGroup(originalName, newName);

                    return;
                }
            }

            throw new KeyNotFoundException("The supplied keyword group was not found.");
        }

        public bool RemoveKeywordGroup(string groupName)
        {
            return KeywordGroups.Remove(groupName);
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
                return Sources.Select((source) => source.GetHashCode()).ToArray();
            }
        }

        public bool Save(string directory, bool overwrite)
        {
            if (File.Exists(Path.Combine(directory, $"{Name}.able")) && !overwrite)
                throw new IOLibraryAlreadyExistsException("Library already exists");

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
