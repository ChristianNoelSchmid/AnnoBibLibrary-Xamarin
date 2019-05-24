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
		AnnoBibLibraryMac.TableViewKeywordGroups TableViewSourceFilters { get; set; }

		[Outlet]
		AppKit.NSTableView TableViewSourceGroups { get; set; }

		[Outlet]
		AppKit.NSTableView TableViewSources { get; set; }

		[Action ("OnSortByChanged:")]
		partial void OnSortByChanged (Foundation.NSObject sender);
		
		void ReleaseDesignerOutlets ()
		{
			if (TableViewSourceGroups != null) {
				TableViewSourceGroups.Dispose ();
				TableViewSourceGroups = null;
			}

			if (ComboBoxSortBy != null) {
				ComboBoxSortBy.Dispose ();
				ComboBoxSortBy = null;
			}

			if (TableViewSourceFilters != null) {
				TableViewSourceFilters.Dispose ();
				TableViewSourceFilters = null;
			}

			if (TableViewSources != null) {
				TableViewSources.Dispose ();
				TableViewSources = null;
			}
		}
	}
}
