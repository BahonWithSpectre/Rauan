using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Rauan.DbData
{
    public class ProductInfo
    {
        public int Id { get; set; }

        public string InfoName { get; set; }

        public int ProductId { get; set; }
        public Product Product { get; set; }

        public List<InProductInfo> InProductInfos { get; set; }
    }
}
