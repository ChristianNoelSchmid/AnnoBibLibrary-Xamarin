using System;
using AppKit;
using Foundation;

namespace AnnoBibLibraryMac.CustomControls
{
    // Child class of NSTextField, with check properties
    // that toggle a boolean if the text field is actively being
    // edited. Used for deletable fields - does not allow deletion
    // while being edited (by pressing Delete button)
    public class EditableTextView : NSTextField, IEditableView
    {
        public bool IsBeingEdited { get; private set; } = false;

        public EventHandler OnDeletePressed { get; set; }

        public bool CanBeDeleted => true;

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

        public override void KeyDown(NSEvent theEvent)
        {
            base.KeyDown(theEvent);

            if (theEvent.KeyCode == (int)NSKey.Delete)
            {
                if (IsBeingEdited) return;
                OnDeletePressed(this, null);
            }
        }
    }
}
