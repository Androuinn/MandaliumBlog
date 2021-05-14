using Mandalium.Core.Helpers.Pagination;
using System;

namespace Mandalium.Core.Models
{
    public class BlogEntry: BaseEntityWithId
    {
        public string Headline { get; set; }
        public string SubHeadline { get; set; }
        public string InnerTextHtml { get; set; }
        public DateTime CreatedOn { get; set; }
        public int TimesRead { get; set; }
        public string PhotoUrl { get; set; }
        public bool WriterEntry { get; set; }
        public bool IsDeleted { get; set; }

        public virtual User User { get; set; }
        public virtual Topic Topic { get; set; }
        public virtual PagedList<Comment> Comments { get; set; }

    }
}