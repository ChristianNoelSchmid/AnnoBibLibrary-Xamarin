using System;
using AnnoBibLibraryMac.CustomControls;
using AppKit;

namespace AnnoBibLibraryMac
{
    public partial class OutlineViewSourceFields : NSOutlineView
    {
        public OutlineViewSourceFields(IntPtr handle) : base(handle) { }

        public EventHandler<DeletePressedEventArgs> OnDeletePressed;
        public override void KeyUp(NSEvent theEvent)
        {
            base.KeyUp(theEvent);

            if (theEvent.KeyCode == (int)NSKey.Delete)
            {
                var textView = (EditableTextView)GetView(0, SelectedRow, false);
                if (textView != null && textView.IsBeingEdited)
                    return;

                OnDeletePressed(this, new DeletePressedEventArgs((int)this.SelectedRow));

            }
        }
    }
}
