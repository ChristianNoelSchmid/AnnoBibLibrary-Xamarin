using System;
using AppKit;

namespace AnnoBibLibraryMac.CustomControls
{
    public class EditableDatePicker : NSDatePicker, IEditableView
    {
        public bool IsBeingEdited { get; private set; } = false;

        public bool CanBeDeleted => true;
    }
}
