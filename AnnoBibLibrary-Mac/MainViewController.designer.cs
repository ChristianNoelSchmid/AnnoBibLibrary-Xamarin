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
	[Register ("MainViewController")]
	partial class MainViewController
	{
		[Outlet]
		AppKit.NSComboBox ComboBoxSortBy { get; set; }

		[Outlet]
		AppKit.NSTableView TableViewTest { get; set; }

		[Outlet]
		AppKit.NSScrollView TextViewNotes { get; set; }
		
		void ReleaseDesignerOutlets ()
		{
			if (ComboBoxSortBy != null) {
				ComboBoxSortBy.Dispose ();
				ComboBoxSortBy = null;
			}

			if (TextViewNotes != null) {
				TextViewNotes.Dispose ();
				TextViewNotes = null;
			}

			if (TableViewTest != null) {
				TableViewTest.Dispose ();
				TableViewTest = null;
			}
		}
	}
}
