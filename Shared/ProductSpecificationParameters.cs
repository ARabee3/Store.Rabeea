using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared
{
    public class ProductSpecificationParameters
    {
        public int? BrandId { get; set; }
        public int? TypeId { get; set; }
        public string? Sort { get; set; }
        private int _pageIndex = 1;
        [DefaultValue(1)]
        public int PageIndex
        {
            get { return _pageIndex; }
            set { _pageIndex =  value; }
        }
        private int _pageSize = 5;
        [DefaultValue(5)]
        public int PageSize
        {
            get { return _pageSize; }
            set { _pageSize = value; }
        }

    }
}
