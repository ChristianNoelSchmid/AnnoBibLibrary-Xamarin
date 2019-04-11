using System;
using System.Collections.Generic;
using AnnoBibLibrary.Shared;
using AnnoBibLibrary.Shared.Bibliography;
using NUnit.Framework;

namespace AnnoBibLibrary.Tests
{
    [TestFixture]
    public class LibraryTests
    {
        private readonly Library _library;
        private readonly Source  _source1, _source2;
    
        public LibraryTests()
        {
            GlobalResources.Initialize();

            _library = new Library("test");
            _library.SetKeywordGroups("People", "Places", "Concepts");

            var format = GlobalResources.GetFormat("Print");

            _source1 = new Source(format);
            _source1["Title"].SetValues("The Lion, the Witch, and the Wardrobe");
            _source1["Author"].SetValues("C.S. Lewis");
            _source1["Publish Year"].SetValues(1950);

            _source2 = new Source(format);
            _source2["Title"].SetValues("The Fellowship of the Ring");
            _source2["Author"].SetValues("J.R.R. Tolkien");
            _source2["Publish Year"].SetValues(1947);

            _library.AddSource(_source1);
            _library.AddSource(_source2);

            _source1.SetKeywordGroup(_library.KeywordGroupsFormatted[0], "Aslan", "Edmund", "Susan");
            _source2.SetKeywordGroup(_library.KeywordGroupsFormatted[1], "Middle Earth", "The Shire");
        }
    
        [Test]
        public void TestLibraryChangeKeywordGroup()
        {
            _library.SetKeywordGroups("People", "Locations", "Concepts");
            Assert.AreEqual(_library.KeywordGroupsFormatted, new string[] { "People", "Locations", "Concepts" });
            Assert.AreEqual(_source2.GetKeywords("Locations"), new string[] { "Middle Earth", "The Shire" });
        }
    }
}
