using System;
using AppKit;

namespace AnnoBibLibraryMac.CustomControls
{
    public class EditableDatePicker : NSDatePicker
    {
        public bool IsBeingEdited { get; private set; }
    }
}
