using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mandalium.Core.Models
{
    public class MostReadEntries
    {
        public int BlogEntryId { get; set; }

        public bool IsWriterEntry { get; set; }

        public DateTime CreatedOn { get; set; }

       
    }
}
