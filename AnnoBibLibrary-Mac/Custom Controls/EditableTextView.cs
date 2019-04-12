using System;
using AppKit;
using Foundation;

namespace AnnoBibLibraryMac.CustomControls
{
    // Child class of NSTextField, with check properties
    // that toggle a boolean if the text field is actively being
    // edited. Used for deletable fields - does not allow deletion
    // while being edited (by pressing Delete button)
    public class EditableTextView : NSTextField
    {
        public bool IsBeingEdited { get; private set; } = false;

        public override void DidEndEditing(NSNotification notification)
        {
            base.DidEndEditing(notification);
            IsBeingEdited = false;
        }

        public override void DidBeginEditing(NSNotification notification)
        {
            base.DidBeginEditing(notification);
            IsBeingEdited = true;
        }
    }
}
