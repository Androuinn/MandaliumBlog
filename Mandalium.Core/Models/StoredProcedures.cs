using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mandalium.Core.Models
{
    public struct StoredProcedures
    {
        public const string InsertMostRead = "EXEC InsertMostRead @BlogId, @WriterEntry";
        public const string GetMostReadEntries = "EXEC GetMostReadEntries";
    }
}
