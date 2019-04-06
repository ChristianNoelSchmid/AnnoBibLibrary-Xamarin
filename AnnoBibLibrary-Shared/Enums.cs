using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnnoBibLibrary.Shared
{  
    public enum SourceFilterLogic
    {
        And = 0,
        Or
    }

    public struct SourceFilterSingle
    {
        public SourceFilterLogic filterLogic;
        public KeyValuePair<string, string> fieldAndValue;
    }

    public struct SourceFilterRange
    {
        public SourceFilterLogic filterLogic;
        public KeyValuePair<string, string> fieldAndValue;
    }
}
