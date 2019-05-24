// WARNING
//
// This file has been generated automatically by Visual Studio to store outlets and
// actions made in the UI designer. If it is removed, they will be lost.
// Manual changes to this file may not be handled correctly.
//
using Foundation;
using System.CodeDom.Compiler;

namespace AnnoBibLibraryMac
{
	[Register ("NewSourceViewController")]
	partial class NewSourceViewController
	{
		[Outlet]
		AppKit.NSButton CheckBoxTitleUnknown { get; set; }

		[Outlet]
		AppKit.NSComboBox ComboBoxFormats { get; set; }

		[Outlet]
		AppKit.NSView MainView { get; set; }

		[Outlet]
		AnnoBibLibraryMac.EditableOutlineView OutlineViewSourceFields { get; set; }

		[Outlet]
		AppKit.NSTableView TableViewKeywords { get; set; }

		[Outlet]
		AppKit.NSTextField TextFieldTitle { get; set; }

		[Action ("OnAccept:")]
		partial void OnAccept (Foundation.NSObject sender);

		[Action ("OnCancel:")]
		partial void OnCancel (Foundation.NSObject sender);

		[Action ("OnFormatChange:")]
		partial void OnFormatChange (Foundation.NSObject sender);

		[Action ("OnTitleUnknownCheck:")]
		partial void OnTitleUnknownCheck (Foundation.NSObject sender);
		
		void ReleaseDesignerOutlets ()
		{
			if (TableViewKeywords != null) {
				TableViewKeywords.Dispose ();
				TableViewKeywords = null;
			}

			if (CheckBoxTitleUnknown != null) {
				CheckBoxTitleUnknown.Dispose ();
				CheckBoxTitleUnknown = null;
			}

			if (ComboBoxFormats != null) {
				ComboBoxFormats.Dispose ();
				ComboBoxFormats = null;
			}

			if (MainView != null) {
				MainView.Dispose ();
				MainView = null;
			}

			if (OutlineViewSourceFields != null) {
				OutlineViewSourceFields.Dispose ();
				OutlineViewSourceFields = null;
			}

			if (TextFieldTitle != null) {
				TextFieldTitle.Dispose ();
				TextFieldTitle = null;
			}
		}
	}
}
