using System;
using System.Linq;
using AnnoBibLibrary.Shared.Fields;
using AnnoBibLibraryMac.DataSources;
using AppKit;

namespace AnnoBibLibraryMac.ControlDelegates
{
    public class DelegateTableViewSources : NSTableViewDelegate
    {
        private DataSourceTableViewSources _dataSource;

        public DelegateTableViewSources(DataSourceTableViewSources dataSource)
        {
            _dataSource = dataSource;
        }

        public override NSView GetViewForItem(NSTableView tableView, NSTableColumn tableColumn, nint row)
        {
            var rowView = tableView.MakeView("Source", this);
            if(rowView == null)
            {
                rowView = new NSTextField
                {
                    Bordered = false,
                    Editable = false,
                    Selectable = false,
                };

                var source = _dataSource.DisplayedSources[(int)row];
                var textField = rowView as NSTextField;
                Field field = source.GetField(tableColumn.Title);

                if (field != null && field.FormattedValues.Length > 0)
                    textField.StringValue = field.FormattedValues.Aggregate(
                        (formatted, next) => $"{formatted}, {next}"
                    );

                else textField.StringValue = "N/A";
            }

            return rowView;
        }
    }
}
