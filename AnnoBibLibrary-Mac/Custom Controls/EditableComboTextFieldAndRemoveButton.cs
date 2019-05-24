using System;
using AppKit;
using Foundation;

namespace AnnoBibLibraryMac.CustomControls
{
    public class EditableComboTextFieldAndRemoveButton : NSSplitView, IEditableView
    {
        public bool IsBeingEdited => ((EditableTextView)Subviews[1]).IsBeingEdited;
        public bool CanBeDeleted => false;

        public EditableComboTextFieldAndRemoveButton(params NSString[] comboBoxValues)         
        {
            DividerStyle = NSSplitViewDividerStyle.Thin;
            IsVertical = true;

            NSComboBox comboBox = new NSComboBox
            {
                Editable = false,
                Selectable = false,
                Bordered = false,
            };
            comboBox.SetFrameSize(new CoreGraphics.CGSize(200f, 17f));
            comboBox.Add(comboBoxValues);

            AddSubview(comboBox);
            AddSubview(new EditableTextView
            {
                Bordered = false,
                StringValue = "",
                PlaceholderString = "New Filter",
            });
            AddSubview(new NSButton
            {
                Bordered = false,
                Image = NSImage.ImageNamed("NSRemoveTemplate"),
            });

            SetHoldingPriority(0, 490);
            SetHoldingPriority(1, 490);
            SetHoldingPriority(2, 490);
        }
    }
}
