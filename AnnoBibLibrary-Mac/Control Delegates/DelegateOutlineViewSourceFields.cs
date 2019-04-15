using System;
using System.Collections.Generic;
using AnnoBibLibrary.Shared.Fields;
using AnnoBibLibraryMac.DataSources;
using AppKit;
using Foundation;

namespace AnnoBibLibraryMac.ControlDelegates
{
    public class DelegateOutlineViewSourceFields : NSOutlineViewDelegate
    {
        private readonly Dictionary<Type, string> RowNames = new Dictionary<Type, string>
        {
            {typeof(WordField), "WordField"},
            {typeof(NameField), "NameField"},
            {typeof(NumberField), "NumberField"},
            {typeof(DateField), "DateField"}
        };

        private readonly DataSourceOutlineViewSourceFields DataSource;

        public DelegateOutlineViewSourceFields(DataSourceOutlineViewSourceFields dataSource)
        {
            DataSource = dataSource;
        }

        public override NSView GetView(NSOutlineView outlineView, NSTableColumn tableColumn, NSObject item)
        {
            var field = item as DataSourceOutlineViewSourceFieldsInfo;
            NSView view = null;

            if (field.CellType == DataSourceOutlineViewSourceFieldsCellType.AddNew)
            {
                view = new NSButton
                {
                    Title = "Add New",
                    ImagePosition = NSCellImagePosition.ImageLeft,
                    Alignment = NSTextAlignment.Left,
                    Bordered = true,
                };

                var button = view as NSButton;
                button.Activated += (sender, e) =>
                {
                    field.FieldGroupParent.AddEmptyFieldValue();
                    outlineView.ReloadData();
                };
            }

            else if (field.CellType == DataSourceOutlineViewSourceFieldsCellType.FieldGroup)
            {
                if (!field.IsExpandable)
                {
                    view = new NSSplitView
                    {
                        IsVertical = true,
                        DividerStyle = NSSplitViewDividerStyle.Thin,
                    };

                    var splitView = view as NSSplitView;

                    splitView.AddSubview(
                        new NSTextField
                        {
                            Selectable = false,
                            Editable = false,
                            Bordered = false,
                            StringValue = field.FieldInfo.Key
                        }
                    );

                    splitView.AddSubview(
                        new NSTextField
                        {
                            Selectable = true,
                            Editable = true,
                            Bordered = false,
                            StringValue = field.Fields[0].Value.ToString(),
                            PlaceholderString = $"New {field.FieldInfo.Key}"
                        }
                    );


                    if (field.FieldInfo.Value.Item1 == typeof(NumberField))
                        ((NSTextField)splitView.Subviews[1]).Formatter = new NSNumberFormatter();

                    splitView.SetHoldingPriority(500, 0);
                }
                else
                {
                    view = new NSTextField
                    {
                        Selectable = false,
                        Editable = false,
                        Bordered = false,
                        StringValue = field.FieldInfo.Key,
                    };
                }
            }

            else
            {
                Type fieldType = field.FieldInfo.Value.Item1;
                if (fieldType != typeof(DateField))
                {
                    view = new NSTextField()
                    {
                        Selectable = true,
                        Editable = true,
                        Bordered = false,
                        PlaceholderString = $"New {field.FieldInfo.Key}"
                    };

                    var textField = view as NSTextField;
                    if (fieldType == typeof(NumberField))
                        textField.Formatter = new NSNumberFormatter();

                    textField.StringValue = field.Value.ToString();
                }

                else
                {
                    view = new NSDatePicker();

                    var datePicker = view as NSDatePicker;
                    datePicker.DatePickerMode = NSDatePickerMode.Single;
                    datePicker.DatePickerElements = NSDatePickerElementFlags.YearMonthDateDay;
                }

            }
                
            view.Identifier = RowNames[field.FieldInfo.Value.Item1];

            return view;

        }
    }
}
