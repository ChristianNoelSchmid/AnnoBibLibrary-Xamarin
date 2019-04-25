using System;
using AppKit;

namespace AnnoBibLibraryMac.CustomControls
{
    public abstract class EditableLabelAndMultimedia : NSSplitView, IEditableView
    {
        public bool IsBeingEdited => ((IEditableView)Subviews[1]).IsBeingEdited;

        public bool CanBeDeleted => false;

        protected EditableLabelAndMultimedia(string label) : base()
        {
            IsVertical = true;
            DividerStyle = NSSplitViewDividerStyle.Thin;

            AddSubview(new NSTextField
            {
                Selectable = false,
                Editable = false,
                Bordered = false,
                StringValue = label
            });
        }
    }
}
