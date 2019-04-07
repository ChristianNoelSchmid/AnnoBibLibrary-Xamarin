using AnnoBibLibrary.Shared.Bibliography;
using AnnoBibLibrary.Shared.Fields;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnnoBibLibrary.Shared
{

    // GlobalData contains readonly information that the program will load
    // and use throughout its processes
    public static class GlobalResources
    {
        public static string ExternalDataDirectory { get; } = 
            Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "AnnoBibLibrary");

        public static string SourcesDirectory { get; } = Path.Combine(ExternalDataDirectory, "Sources");
        public static string LibrariesDirectory { get; } = Path.Combine(ExternalDataDirectory, "Libraries"); 

        // A collection of common formats
        private static Dictionary<string, CitationFormat> CitationFormats = new Dictionary<string, CitationFormat>();
        public static string[] CitationFormatsFormatted => CitationFormats.Keys.ToList().Select((key) => key = Tools.Capitalize(key)).ToArray();

        // Initialize all the Resource Data, avaialble throughout the
        // entire program
        // TODO - eventually store in external data
        public static void Initialize()
        {
            CreateInitialDirectories();

            // Create a print CitationFormat
            CitationFormat printFormat = new CitationFormat("Print");
            printFormat.AddField("Title", typeof(WordField), false);
            printFormat.AddField("Author", typeof(NameField), true);
            printFormat.AddField("Editor", typeof(NameField), true);
            printFormat.AddField("Publisher Region", typeof(WordField), false);
            printFormat.AddField("Publisher Name", typeof(WordField), false);
            printFormat.AddField("Publish Year", typeof(NumberField), false);

            CitationFormats.Add("print", printFormat);

            // Create an E-Book CitationFormat
            CitationFormat ebookFormat = new CitationFormat("E-Book");
            ebookFormat.AddField("Title", typeof(WordField), false);
            ebookFormat.AddField("Author", typeof(NameField), true);
            ebookFormat.AddField("Editor", typeof(NameField), true);
            ebookFormat.AddField("Publisher Region", typeof(WordField), false);
            ebookFormat.AddField("Publisher Name", typeof(WordField), false);
            ebookFormat.AddField("Publish Year", typeof(NumberField), false);

            CitationFormats.Add("ebook", printFormat);
        }

        public static void CreateInitialDirectories()
        {
            var documentsFolder = Environment.SpecialFolder.Personal.ToString();
            if (!Directory.Exists(Path.Combine(documentsFolder, ExternalDataDirectory)))
                Directory.CreateDirectory(Path.Combine(documentsFolder, ExternalDataDirectory));

            if (!Directory.Exists(Path.Combine(documentsFolder, ExternalDataDirectory, SourcesDirectory)))
                Directory.CreateDirectory(Path.Combine(documentsFolder, ExternalDataDirectory, SourcesDirectory));

            if (!Directory.Exists(Path.Combine(documentsFolder, ExternalDataDirectory, LibrariesDirectory)))
                Directory.CreateDirectory(Path.Combine(documentsFolder, ExternalDataDirectory, LibrariesDirectory));
        }

        public static CitationFormat GetFormat(string formatName)
        {
            string lowercaseName = formatName.ToLower();

            if (!CitationFormats.ContainsKey(lowercaseName))
                throw new CitationFormatNotFoundException("Citation Format could not be found.");

            else return CitationFormats[lowercaseName];
        }
    }
}
