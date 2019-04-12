using System;
using System.Collections.Generic;
using AppKit;

namespace AnnoBibLibraryMac.DataSources
{
    // The DataSource for the Keyword Groups TableView
    // Describes the functionality of the source (with class KeywordGroup)
    // which the TableView implements
    public class DataSourceTableViewKeywordGroups : NSTableViewDataSource
    {
        public List<DataSourceTableViewKeywordGroupKeyword> Keywords = new List<DataSourceTableViewKeywordGroupKeyword>();

        public override nint GetRowCount(NSTableView tableView)
        {
            return Keywords.Count;
        }
    }

    public class DataSourceTableViewKeywordGroupKeyword
    {
        public string GroupName { get; set; }
        public bool IsNew { get; set; }
        public bool IsDeleted { get; set; }
    }
}
