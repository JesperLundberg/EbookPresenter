using System.Collections.Generic;

namespace EBookPresenter.Models
{
    public class EBookViewModel
    {
        public IEnumerable<EBook> EBooks { get; set; }
        public string SortOrder { get; set; }
        public int TotalItems { get; set; }
    }
}