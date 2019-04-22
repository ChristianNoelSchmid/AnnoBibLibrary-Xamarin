using System;
using System.Collections.Generic;
using AnnoBibLibrary.Shared.Fields;
using AnnoBibLibraryMac.CustomControls;
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
                view = CreateAddNewButtonView(field, outlineView);
            }

            else if (field.CellType == DataSourceOutlineViewSourceFieldsCellType.FieldGroup)
            {
                view = CreateFieldGroupView(field);
            }

            else
            {
                view = CreateValueView(field);
            }
                
            view.Identifier = RowNames[field.FieldInfo.Value.Item1];

            return view;

        }

        private NSView CreateAddNewButtonView(DataSourceOutlineViewSourceFieldsInfo field, NSOutlineView outlineView)
        {
            NSButton button = new NSButton
            {
                Title = "Add New",
                ImagePosition = NSCellImagePosition.ImageLeft,
                Alignment = NSTextAlignment.Left,
                Bordered = false,
                Image = NSImage.ImageNamed("NSAddTemplate"),
                ContentTintColor = NSColor.Black
            };

            button.Activated += (sender, e) =>
            {
                field.FieldGroupParent.AddEmptyFieldValue();
                outlineView.ReloadData();
            };

            return button;
        }

        private NSView CreateFieldGroupView(DataSourceOutlineViewSourceFieldsInfo field)
        {
            NSView view = null;

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
                        StringValue = field.FieldInfo.Key,
                    }
                );

                if (field.FieldInfo.Value.Item1 == typeof(DateField))
                {
                    splitView.AddSubview(
                        new NSDatePicker
                        {
                            DatePickerStyle = NSDatePickerStyle.TextField,
                            DateValue = (NSDate)(DateTime.TryParse(field.Fields[0].Value, out DateTime parsedDate) ? parsedDate : DateTime.Now),
                            DatePickerElements = NSDatePickerElementFlags.YearMonthDateDay
                        }
                    );

                    var dateField = splitView.Subviews[1] as NSDatePicker;
                    dateField.ValidateProposedDateValue += (sender, e) => field.Fields[0].Value = ((DateTime)dateField.DateValue).ToLocalTime().ToString();
                }

                else
                {
                    splitView.AddSubview(
                        new EditableTextView
                        {
                            Selectable = true,
                            Editable = true,
                            Bordered = false,
                            StringValue = field.Fields[0].Value,
                            PlaceholderString = $"New {field.FieldInfo.Key}",
                        }
                    );

                    var textField = splitView.Subviews[1] as NSTextField;

                    textField.EditingEnded += (sender, e) => field.Fields[0].Value = textField.StringValue;
                    if (field.FieldInfo.Value.Item1 == typeof(NumberField))
                        textField.Formatter = new NSNumberFormatter();
                }

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

            return view;
        }

        private NSView CreateValueView(DataSourceOutlineViewSourceFieldsInfo field)
        {
            NSView view = null;

            Type fieldType = field.FieldInfo.Value.Item1;
            if (field.FieldInfo.Value.Item1 == typeof(DateField))
            {
                view = new NSDatePicker
                {
                    DatePickerStyle = NSDatePickerStyle.TextField,
                    DateValue = (NSDate)(DateTime.TryParse(field.Value, out DateTime parsedDate) ? parsedDate : DateTime.Now),
                    DatePickerElements = NSDatePickerElementFlags.YearMonthDateDay
                };

                var dateField = view as NSDatePicker;
                dateField.ValidateProposedDateValue += (sender, e) => field.Fields[0].Value = ((DateTime)dateField.DateValue).ToLocalTime().ToString();
            }
            else
            {
                view = new EditableTextView()
                {
                    Selectable = true,
                    Editable = true,
                    Bordered = false,
                    PlaceholderString = $"New {field.FieldInfo.Key}"
                };

                var textField = view as NSTextField;
                if (fieldType == typeof(NumberField))
                    textField.Formatter = new NSNumberFormatter();

                textField.StringValue = field.Value;

                textField.EditingEnded += (sender, e) =>
                    field.Value = textField.StringValue;
            }

            return view;
        }
    }
}
