using System;
using System.Collections.Generic;
using AppKit;

namespace AnnoBibLibraryMac.DataSources
{
    public class DataSourceTableViewKeywordGroups : NSTableViewDataSource
    {
        public List<string> Keywords = new List<string>();

        public override nint GetRowCount(NSTableView tableView)
        {
            return Keywords.Count;
        }
    }
}
