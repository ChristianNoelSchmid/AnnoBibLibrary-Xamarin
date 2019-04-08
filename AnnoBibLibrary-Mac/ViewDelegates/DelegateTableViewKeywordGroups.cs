using System;
using System.Collections.Generic;
using AnnoBibLibraryMac.DataSources;
using AppKit;


namespace AnnoBibLibraryMac.ViewDelegates
{
    public class DelegateTableViewKeywordGroups : NSTableViewDelegate
    {
        #region Constants
        private const string CellIdentifier = "KeywordGroups";
        #endregion

        #region Private Variables
        private readonly DataSourceTableViewKeywordGroups _dataSource;
        #endregion

        #region Constructors
        public DelegateTableViewKeywordGroups(DataSourceTableViewKeywordGroups dataSource)
        {
            _dataSource = dataSource;       
        }
        #endregion

        #region Override Methods
        public override NSView GetViewForItem(NSTableView tableView, NSTableColumn tableColumn, nint row)
        {
            NSTextField view = (NSTextField)tableView.MakeView(CellIdentifier, this);
            if(view == null)
            {
                view = new NSTextField
                {
                    Identifier = CellIdentifier,
                    Bordered = false,
                    BackgroundColor = NSColor.White,
                    Selectable = true,
                    Editable = true
                };
            }

            view.StringValue = _dataSource.Keywords[(int)row];

            return view;
        }
        #endregion
    }
}
