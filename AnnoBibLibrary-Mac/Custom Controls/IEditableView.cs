using System;
using AppKit;

namespace AnnoBibLibraryMac.CustomControls
{
    public interface IEditableView
    {
        bool IsBeingEdited { get; }
        bool CanBeDeleted { get; }
    }
}
