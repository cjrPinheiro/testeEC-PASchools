using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PASchools.Application.DTOs
{
    public class PagedList<T> where T : class
    {
        public List<T> Items { get; set; }
        public int TotalItems { get; set; }
        public int Index { get; set; }
        public int Size { get; set; }
    }
}
