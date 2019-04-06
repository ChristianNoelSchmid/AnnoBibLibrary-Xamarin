using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnnoBibLibrary.Shared
{
    class CitationFormatNotFoundException : Exception
    {
        public CitationFormatNotFoundException(string msg) : base(msg) { }
    }

    class CitationFormatFieldMismatch : Exception
    {
        public CitationFormatFieldMismatch(string msg) : base(msg) { }
    }

    class SerializerCannotParseFieldTypeException : Exception
    {
        public SerializerCannotParseFieldTypeException(string msg) : base(msg) { }
    }

    class MultipleValuesInFieldNotAllowedException : Exception
    {
        public MultipleValuesInFieldNotAllowedException(string msg) : base(msg) { }
    }

    class FieldNotFoundException : Exception
    {
        public FieldNotFoundException(string msg) : base(msg) { }
    }

    class FieldValueTypeMismatch : Exception
    {
        public FieldValueTypeMismatch(string msg) : base(msg) { }
    }
}
