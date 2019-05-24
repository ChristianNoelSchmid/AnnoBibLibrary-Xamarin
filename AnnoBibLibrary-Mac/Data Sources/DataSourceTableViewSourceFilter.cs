using System;
using System.Collections.Generic;
using AnnoBibLibrary.Shared;
using AppKit;

namespace AnnoBibLibraryMac.DataSources
{
    public class DataSourceTableViewSourceFilter : NSTableViewDataSource
    {
        public DataSourceTableViewSourceFilter()
        {
            FilterInfo = new List<DataSourceSourceFilterInfo>
            {
                new DataSourceSourceFilterInfo { IsAddNewButton = true }
            };
        }

        public List<DataSourceSourceFilterInfo> FilterInfo;

        public override nint GetRowCount(NSTableView tableView)
        {
            return FilterInfo.Count;
        }
    }

    public class DataSourceSourceFilterInfo 
    {
        public bool IsAddNewButton;
        public string FilterChoice = GlobalResources.LibraryFilters[0].Item1;
        public string FilterParams = "";
    }
}
