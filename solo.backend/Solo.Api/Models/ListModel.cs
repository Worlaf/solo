using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Solo.Api.Models
{
    public class ListModel<TListItem>
    {
        public IReadOnlyCollection<TListItem> Items { get; set; }
    }
}
