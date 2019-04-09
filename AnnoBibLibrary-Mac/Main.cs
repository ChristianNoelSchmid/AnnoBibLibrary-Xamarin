using AnnoBibLibrary.Shared;
using AppKit;

namespace AnnoBibLibraryMac
{
    static class MainClass
    {
        static void Main(string[] args)
        {
            GlobalResources.Initialize();

            NSApplication.Init();
            NSApplication.Main(args);
        }
    }
}
