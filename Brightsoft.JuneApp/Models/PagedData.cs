using System.Collections.Generic;

namespace JuneApp.Models
{
    public class PagedData<T>
    {
        public int Page { get; set; }

        public int PageSize { get; set; }

        public long TotalCount { get; set; }

        public IEnumerable<T> Items { get; set; }
    }
}
