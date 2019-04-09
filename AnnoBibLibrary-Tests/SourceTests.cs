using AnnoBibLibrary.Shared;
using AnnoBibLibrary.Shared.Bibliography;
using AnnoBibLibrary.Shared.Fields;
using NUnit.Framework;
using System;

namespace AnnoBibLibrary.Tests
{ 
    [TestFixture]
    public class SourceTests
    {
        private CitationFormat format;

        public SourceTests()
        {
            GlobalResources.CreateInitialDirectories();

            format = new CitationFormat("Print");
            format.AddField("Title", typeof(WordField), false);
            format.AddField("Author", typeof(NameField), true);
            format.AddField("Publish Year", typeof(NumberField), false);
        }

        [Test]
        public void TestSourceCreation()
        {
            Source source = new Source(format);
            source["Title"].SetValues("The Lion, the Witch, and the Wardrobe");
            source["Author"].SetValues("C.S. Lewis");

            Assert.AreEqual(source["Title"].FormattedValues[0], "Lion, the Witch, and the Wardrobe, the");
            Assert.AreEqual(source["Author"].FormattedValues[0], "Lewis, C.S.");
        }

        [Test]
        public void TestSourceSaveAndLoad()
        {
            Source source = new Source(format);
            source["Title"].SetValues("The Lord of the Rings: the Two Towers");
            source["Author"].SetValues("J.R.R. Tolkien");

            source.Save(GlobalResources.SourcesDirectory);

            Source source2 = Source.Load($"{GlobalResources.SourcesDirectory}/{source.GetHashCode()}.abs");
            Assert.AreEqual(source, source2);
        }
    }

}
