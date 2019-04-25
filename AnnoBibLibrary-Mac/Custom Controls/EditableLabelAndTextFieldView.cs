using System;
using AppKit;

namespace AnnoBibLibraryMac.CustomControls
{
    public class EditableLabelAndTextFieldView : EditableLabelAndMultimedia, IEditableView
    {
        public EditableLabelAndTextFieldView(string label, string text) : base(label)
        { 
            AddSubview(new EditableTextView
            {
                Selectable = true,
                Editable = true,
                Bordered = false,
                StringValue = text
            });
        }
    }
}
