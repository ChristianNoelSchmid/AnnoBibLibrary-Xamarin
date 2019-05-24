using System;
using System.Collections.Generic;
using AppKit;

namespace AnnoBibLibraryMac.DataSources
{
    public class DataSourceTableViewKeywords : NSTableViewDataSource
    {
        public List<Tuple<string, string>> Keywords = new List<Tuple<string, string>>();

        public override nint GetRowCount(NSTableView tableView)
        {
            return Keywords.Count;
        }
    }
}
