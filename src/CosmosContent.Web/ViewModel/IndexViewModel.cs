using CosmosContent.Data.Entities;
using System.Collections.Generic;

namespace CosmosContent.Web.ViewModel
{
    public class IndexViewModel
    {
        public IList<Content> Contents { get; set; }

        public IndexViewModel()
        {
        }
    }
}
