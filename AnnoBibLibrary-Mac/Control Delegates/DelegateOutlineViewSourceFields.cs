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
                view = CreateFieldGroupView(field, outlineView);
            }

            else
            {
                view = CreateValueView(field);
            }
                
            view.Identifier = RowNames[field.FieldInfo.FieldType];

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

        private NSView CreateFieldGroupView(DataSourceOutlineViewSourceFieldsInfo field, NSOutlineView outlineView)
        {
            NSView view;

            if (!field.IsExpandable)
            {
                if (field.FieldInfo.FieldType == typeof(DateField))
                {
                    view = new EditableLabelAndDatePicker(field.FieldInfo.Name);
                    var datePicker = (EditableDatePicker)view.Subviews[1];

                    datePicker.ValidateProposedDateValue += (sender, e) => field.Value = ((DateTime)datePicker.DateValue).ToLocalTime();
                }

                else
                {
                    view = new EditableLabelAndTextFieldView(field.FieldInfo.Name, field.Fields[0].Value.ToString());
                    var textField = view.Subviews[1] as NSTextField;

                    if (field.FieldInfo.FieldType == typeof(NumberField))
                    {
                        textField.Formatter = new NSNumberFormatter();
                        textField.EditingEnded += (sender, e) => field.Value = int.Parse(textField.StringValue);
                    }
                    else
                        textField.EditingEnded += (sender, e) => field.Value = textField.StringValue;
                }

                (view as NSSplitView).SetHoldingPriority(500, 0);
            }
            else
            {
                view = new NSTextField
                {
                    Selectable = false,
                    Editable = false,
                    Bordered = false,
                    StringValue = field.FieldInfo.Name,
                };
            }

            return view;
        }

        private NSView CreateValueView(DataSourceOutlineViewSourceFieldsInfo field)
        {
            NSView view = null;

            Type fieldType = field.FieldInfo.FieldType;
            if (fieldType == typeof(DateField))
            {
                view = new NSDatePicker
                {
                    DatePickerStyle = NSDatePickerStyle.TextField,
                    DatePickerElements = NSDatePickerElementFlags.YearMonthDateDay
                };

                var dateField = view as NSDatePicker;
                dateField.ValidateProposedDateValue += (sender, e) => field.Fields[0].Value = ((DateTime)dateField.DateValue).ToLocalTime();
            }
            else
            {
                view = new EditableTextView()
                {
                    Selectable = true,
                    Editable = true,
                    Bordered = false,
                    PlaceholderString = $"New {field.FieldInfo.Name}"
                };

                var textField = view as NSTextField;
                if (fieldType == typeof(NumberField))
                    textField.Formatter = new NSNumberFormatter();

                textField.StringValue = field.Value.ToString();

                textField.EditingEnded += (sender, e) =>
                    field.Value = textField.StringValue;
            }

            return view;
        }
    }
}
