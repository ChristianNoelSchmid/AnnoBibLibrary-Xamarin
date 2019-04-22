using System;
using System.Collections.Generic;
using AnnoBibLibrary.Shared.Bibliography;
using AnnoBibLibrary.Shared.Fields;
using AppKit;
using Foundation;

namespace AnnoBibLibraryMac.DataSources
{
    // The DataSource for the SourceFields OutlineView
    // Overwrites OutlineView methods, supplying information related to a created/edited
    // Source's Fields
    public class DataSourceOutlineViewSourceFields : NSOutlineViewDataSource
    { 
        public List<DataSourceOutlineViewSourceFieldsInfo> FieldInfo { get; set; } = new List<DataSourceOutlineViewSourceFieldsInfo>();

        public override nint GetChildrenCount(NSOutlineView outlineView, NSObject item)
        {
            if (item == null)
                return FieldInfo.Count;

            return ((DataSourceOutlineViewSourceFieldsInfo)item).Fields.Count;
        }

        public override NSObject GetChild(NSOutlineView outlineView, nint childIndex, NSObject item)
        {
            if (item == null)
                return FieldInfo[(int)childIndex];

            return ((DataSourceOutlineViewSourceFieldsInfo)item).Fields[(int)childIndex];
        }

        public override bool ItemExpandable(NSOutlineView outlineView, NSObject item)
        {
            if (item == null)
                return FieldInfo[0].IsExpandable;

            return ((DataSourceOutlineViewSourceFieldsInfo)item).IsExpandable;
        }
    }

    // The DataSource collection type for the Source Field outline view
    // Contains all information related to a particular Field, as well as
    // whether the specified NSObject is a FieldGroup or not. FieldGroups will be
    // the main dividers for the OutlineView, and will simply list the FieldName. 
    public class DataSourceOutlineViewSourceFieldsInfo : NSObject
    {
        public DataSourceOutlineViewSourceFieldsInfo(KeyValuePair<string, Tuple<Type, bool>> fieldInfo)
        {
            FieldGroupParent = null;
            FieldInfo = fieldInfo;

            Value = null;
            Fields = new List<DataSourceOutlineViewSourceFieldsInfo>();

            // If the Field does not allow multiple, it will require a default value to be able
            // to be retrieved by the OutlineView, by the way in which it is drawn
            if (!FieldInfo.Value.Item2)
                AddEmptyFieldValue();

            else
                CreateAddNewButton();
        }

        private DataSourceOutlineViewSourceFieldsInfo(DataSourceOutlineViewSourceFieldsInfo parent)
        {
            FieldGroupParent = parent;
            FieldInfo = parent.FieldInfo;
            CellType = DataSourceOutlineViewSourceFieldsCellType.Value;

            Value = null;
            Fields = null;
        }

        public void AddEmptyFieldValue()
        {
            FieldGroupParent = this;

            Fields.Insert(0, new DataSourceOutlineViewSourceFieldsInfo(this));
            Fields[0].Value = "";

            Fields[0].Fields = null;
        }

        private void CreateAddNewButton()
        {
            Fields.Add(new DataSourceOutlineViewSourceFieldsInfo(this));
            Fields[Fields.Count - 1].CellType = DataSourceOutlineViewSourceFieldsCellType.AddNew;
        }

        public KeyValuePair<string, Tuple<Type, bool>> FieldInfo;
        public DataSourceOutlineViewSourceFieldsInfo FieldGroupParent { get; set; }
        public DataSourceOutlineViewSourceFieldsCellType CellType { get; set; }

        public string Value { get; set; }
        public List<DataSourceOutlineViewSourceFieldsInfo> Fields { get; set; }

        public bool IsExpandable => CellType == DataSourceOutlineViewSourceFieldsCellType.FieldGroup &&
            FieldInfo.Value.Item2;
    }

    public enum DataSourceOutlineViewSourceFieldsCellType
    {
        FieldGroup,
        Value,
        AddNew
    }

  
}

