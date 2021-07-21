using System.Collections.Generic;

namespace EBookPresenter.Models
{
    public class EBookViewModel
    {
        public IEnumerable<EBook> EBooks { get; init; }
        public string SortOrder { get; init; }
        public int TotalItems { get; init; }
        public string NextPageUrl { get; set; }
        public string PreviousPageUrl { get; set; }
    }
}