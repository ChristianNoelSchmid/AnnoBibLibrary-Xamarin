using System;
using AnnoBibLibraryMac.CustomControls;
using AnnoBibLibraryMac.DataSources;
using AppKit;

namespace AnnoBibLibraryMac.ControlDelegates
{
    public class DelegateTableViewKeywords : NSTableViewDelegate
    {
        private DataSourceTableViewKeywords _dataSource;
        public DelegateTableViewKeywords(DataSourceTableViewKeywords dataSource)
        {
            _dataSource = dataSource;
        }

        public override NSView GetViewForItem(NSTableView tableView, NSTableColumn tableColumn, nint row)
        {
            if(tableColumn.Title == "Keyword Group")
            {
                NSView view = tableView.MakeView("Group", this);
                if(view == null)
                {
                    view = new NSTextField
                    {
                        Editable = false,
                        Bordered = false,
                        Selectable = false,
                        StringValue = _dataSource.Keywords[(int)row].Item1
                    };
                }

                return view;
            }
            else
            {
                NSTextField textField = (NSTextField)tableView.MakeView("Values", this);
                if(textField == null)
                {
                    textField = new NSTextField
                    {
                        Editable = true,
                        Bordered = false,
                        Selectable = true,
                        StringValue = _dataSource.Keywords[(int)row].Item2
                    };
                }

                textField.EditingEnded += (sender, e) => _dataSource.Keywords[(int)row] =
                    new Tuple<string, string>(_dataSource.Keywords[(int)row].Item1, textField.StringValue);

                return textField;   
            }
        }
    }
}
