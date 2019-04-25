using System;
using AppKit;
using Foundation;

namespace AnnoBibLibraryMac.CustomControls
{
    public class EditableLabelAndDatePicker : EditableLabelAndMultimedia, IEditableView
    {
        public EditableLabelAndDatePicker(string label) : base(label)
        {
            AddSubview(new EditableDatePicker
            {
                DatePickerStyle = NSDatePickerStyle.TextField,
                DateValue = (NSDate)DateTime.Now,
                DatePickerElements = NSDatePickerElementFlags.YearMonthDateDay
            });
        }
    }
}
