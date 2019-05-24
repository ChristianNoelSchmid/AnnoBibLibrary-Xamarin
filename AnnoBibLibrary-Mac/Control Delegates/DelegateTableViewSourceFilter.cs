using System;
using System.Collections.Generic;
using AnnoBibLibrary.Shared;
using AnnoBibLibraryMac.CustomControls;
using AnnoBibLibraryMac.DataSources;
using AppKit;
using Foundation;

namespace AnnoBibLibraryMac.ControlDelegates
{
    public class DelegateTableViewSourceFilter : NSTableViewDelegate
    {
        private DataSourceTableViewSourceFilter _dataSource;
        private SourceSorter _sourceSorter;

        public DelegateTableViewSourceFilter(DataSourceTableViewSourceFilter dataSource, SourceSorter sorter)
        {
            _dataSource = dataSource;
            _sourceSorter = sorter;
        }

        public override NSView GetViewForItem(NSTableView tableView, NSTableColumn tableColumn, nint row)
        {
            NSView view = tableView.MakeView("Filter", this);
            if(view == null)
            {
                if(_dataSource.FilterInfo[(int)row].IsAddNewButton)
                {
                    view = new NSButton
                    {
                        Title = "New Filter",
                        ImagePosition = NSCellImagePosition.ImageLeft,
                        Alignment = NSTextAlignment.Left,
                        Bordered = false,
                        Image = NSImage.ImageNamed("NSAddTemplate"),
                    };

                    NSButton button = view as NSButton;
                    button.Activated += (sender, e) =>
                    {
                        _dataSource.FilterInfo.Insert(_dataSource.FilterInfo.Count - 1, new DataSourceSourceFilterInfo
                        {
                            IsAddNewButton = false,
                        });
                        tableView.ReloadData();
                    };
                }
                else
                {
                    Tuple<string, Type>[] filters = GlobalResources.LibraryFilters;
                    NSString[] filterStrings = new NSString[filters.Length];

                    for (int i = 0; i < filters.Length; ++i)
                        filterStrings[i] = new NSString(filters[i].Item1);

                    view = new EditableComboTextFieldAndRemoveButton(filterStrings);

                    var comboBox = (NSComboBox)view.Subviews[0];
                    comboBox.StringValue = _dataSource.FilterInfo[(int)row].FilterChoice;

                    comboBox.SelectionChanged += (sender, e) =>
                    {
                        _dataSource.FilterInfo[(int)row].FilterChoice = comboBox.StringValue;
                        FilterSources();
                    };

                    var textField = (NSTextField)view.Subviews[1];
                    textField.StringValue = _dataSource.FilterInfo[(int)row].FilterParams;
                    textField.EditingEnded += (sender, e) =>
                    {
                        _dataSource.FilterInfo[(int)row].FilterParams = textField.StringValue;
                        var filterInfo = FilterSources();

                        _sourceSorter.ImportFilterInfo(filterInfo);
                    };

                    NSButton button = (NSButton)view.Subviews[2];
                    button.Activated += (sender, e) =>
                    {
                        _dataSource.FilterInfo.RemoveAt((int)row);
                        tableView.ReloadData();
                        var filterInfo = FilterSources();

                        _sourceSorter.ImportFilterInfo(filterInfo);
                    };

                }
            }

            return view;
        }
    
        private SourceSorterFilterInfo[] FilterSources()
        {
            var filterInfo = new List<SourceSorterFilterInfo>();
            foreach(var filters in _dataSource.FilterInfo)
            {
                if (filters.IsAddNewButton) continue;
                filterInfo.Add(new SourceSorterFilterInfo
                {
                    FieldName = filters.FilterChoice,
                    Parameters = filters.FilterParams
                });
            }

            return filterInfo.ToArray();
        } 
    }
}
