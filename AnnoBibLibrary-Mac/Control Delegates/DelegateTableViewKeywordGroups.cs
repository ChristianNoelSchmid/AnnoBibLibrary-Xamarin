using System;
using System.Collections.Generic;
using AnnoBibLibraryMac.DataSources;
using AppKit;
using Foundation;

namespace AnnoBibLibraryMac.ViewDelegates
{
    public class DelegateTableViewKeywordGroups : NSTableViewDelegate
    {
        private const string CellIdentifier = "KeywordGroups";

        private readonly DataSourceTableViewKeywordGroups _dataSource;

        public DelegateTableViewKeywordGroups(DataSourceTableViewKeywordGroups dataSource)
        {
            _dataSource = dataSource;
        }

        public override NSView GetViewForItem(NSTableView tableView, NSTableColumn tableColumn, nint row)
        {
            NSTextField view = (NSTextField)tableView.MakeView(CellIdentifier, this);
            if (view == null)
            {
                view = new NSTextField
                {
                    Identifier = CellIdentifier,
                    Bordered = false,
                    BackgroundColor = NSColor.White,
                    Selectable = true,
                    Editable = true,
                    PlaceholderString = "New Keyword",
                };

                view.Activated += (sender, e) =>
                {
                    AdjustLibraryKeywordGroup(view, (int)row);
                };
            }

            view.StringValue = _dataSource.Keywords[(int)row].Item1;

            return view;
        }

        private void AdjustLibraryKeywordGroup(NSTextField textField, int row)
        {
            if (_dataSource.Keywords[row].Item2 != true)
            {
                if (textField.StringValue.ToLower().Trim() != _dataSource.Keywords[row].Item1)
                {
                    var alertDialog = new NSAlert
                    {
                        AlertStyle = NSAlertStyle.Warning,
                        MessageText = $"Are you sure you wish to overwrite keyword group \"{_dataSource.Keywords[row].Item1}\" with \"{textField.StringValue}\"?",
                        InformativeText = "This will rename keyword group for all sources in library as well.",
                    };

                    alertDialog.AddButton("Yes");
                    alertDialog.AddButton("No");

                    var choice = alertDialog.RunModal();

                    if (choice == 1000) _dataSource.Keywords[row] = new Tuple<string, bool>(textField.StringValue, false);
                    else if (choice == 1001) textField.StringValue = _dataSource.Keywords[row].Item1;
                }
            }
            else
                _dataSource.Keywords[row] = new Tuple<string, bool>(textField.StringValue, false);
        }

    }
}
