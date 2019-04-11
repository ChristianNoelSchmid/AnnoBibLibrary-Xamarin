using System;
using System.Collections.Generic;
using AppKit;

namespace AnnoBibLibraryMac.DataSources
{
    public class DataSourceTableViewKeywordGroups : NSTableViewDataSource
    {
        public List<Tuple<string, bool>> Keywords = new List<Tuple<string, bool>>();

        public override nint GetRowCount(NSTableView tableView)
        {
            return Keywords.Count;
        }
    }
}
