using System;
using AnnoBibLibrary.Shared;
using AnnoBibLibrary.Shared.Bibliography;
using AnnoBibLibrary.Shared.Fields;
using NUnit.Framework;

namespace AnnoBibLibraryTests
{
    [TestFixture]
    public class SourceSorterTests
    {
        private Library library;
        private SourceSorter sorter;
        private Source source1, source2, source3;

        [TestFixtureSetUp]
        public void Init()
        {
            library = new Library("Source Sorter Test Library");
            sorter = new SourceSorter(library);

            CitationFormat format = new CitationFormat("Print");
            format.AddField("Title", typeof(WordField), false);
            format.AddField("Author", typeof(NameField), true);

            source1 = new Source(format);
            source1.SetValues("Title", "The Lion, the Witch, and the Wardrobe");
            source1.SetValues("Author", "C.S. Lewis");
            library.AddSource(source1);

            source2 = new Source(format);
            source2.SetValues("Title", "The Lord of the Rings");
            source2.SetValues("Author", "J.R.R. Tolkien");
            library.AddSource(source2);

            source3 = new Source(format);
            source1.SetValues("Title", "The Aspiring Adventures of Joe Bob");
            source1.SetValues("Author", "Clive Tolkien", "Justice Beaver", "Mustard Yellow");
            library.AddSource(source1);
        }

        [Test]
        public void TestSortingSingle()
        {
            sorter.SortByField = "Title";
            SourceSorter.FilterSources(sorter);

            Assert.ReferenceEquals(source3, sorter.GetSources(""));
        }
    }
}
