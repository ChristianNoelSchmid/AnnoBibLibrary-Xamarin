using System;
using System.Collections.Generic;
using AnnoBibLibraryMac.CustomControls;
using AnnoBibLibraryMac.DataSources;
using AppKit;
using Foundation;

namespace AnnoBibLibraryMac.ViewDelegates
{
    public class DelegateTableViewKeywordGroups : NSTableViewDelegate
    {
        private const string CellIdentifier = "KeywordGroups";

        private DataSourceTableViewKeywordGroups _dataSource;

        public DelegateTableViewKeywordGroups(DataSourceTableViewKeywordGroups dataSource)
        {
            _dataSource = dataSource;
        }

        public override NSView GetViewForItem(NSTableView tableView, NSTableColumn tableColumn, nint row)
        {
            EditableTextView view = (EditableTextView)tableView.MakeView(CellIdentifier, this);
            if (view == null)
            {
                view = new EditableTextView
                {
                    Identifier = CellIdentifier,
                    Bordered = false,
                    BackgroundColor = NSColor.White,
                    Selectable = true,
                    Editable = true,
                    PlaceholderString = "New Keyword",
                };

                view.EditingEnded += (sender, e) =>
                {
                    AdjustLibraryKeywordGroup(view, (int)row);
                };
            }

            view.StringValue = string.Format("{0}{1}",
                _dataSource.Keywords[(int)row].IsDeleted ? "<DELETED>" : "",
                _dataSource.Keywords[(int)row].GroupName);

            return view;
        }

        private void AdjustLibraryKeywordGroup(NSTextField textField, int row)
        {
            if (!_dataSource.Keywords[row].IsNew)
            {
                if (textField.StringValue.ToLower().Trim() != _dataSource.Keywords[row].GroupName.ToLower().Trim())
                {
                    var alertDialog = new NSAlert
                    {
                        AlertStyle = NSAlertStyle.Warning,
                        MessageText = $"Are you sure you wish to overwrite keyword group \"{_dataSource.Keywords[row].GroupName}\" with \"{textField.StringValue}\"?",
                        InformativeText = "This will rename keyword group for all sources in library as well.",
                    };

                    alertDialog.AddButton("Yes");
                    alertDialog.AddButton("No");

                    var choice = alertDialog.RunModal();

                    if (choice == 1000) _dataSource.Keywords[row].GroupName = textField.StringValue;
                    else if (choice == 1001) textField.StringValue = _dataSource.Keywords[row].GroupName;
                }
            }
            else
                _dataSource.Keywords[row].GroupName = textField.StringValue;
        }

    }
}
