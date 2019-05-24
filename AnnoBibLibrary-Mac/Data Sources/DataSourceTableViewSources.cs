using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using AnnoBibLibrary.Shared;
using AnnoBibLibrary.Shared.Bibliography;
using AppKit;

namespace AnnoBibLibraryMac.DataSources
{
    public class DataSourceTableViewSources : NSTableViewDataSource
    {
        public List<Source> DisplayedSources;

        public DataSourceTableViewSources(SourceSorter sorter)
        {
            DisplayedSources = sorter.DisplayedSources;
        }

        public override nint GetRowCount(NSTableView tableView)
        {
            return DisplayedSources.Count;
        }
    }
}
