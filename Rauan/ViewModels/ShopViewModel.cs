using Rauan.DbData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Rauan.ViewModels
{
    public class ShopViewModel
    {
        public List<Product> Products { get; set; }
        public List<LateView> LateViews { get; set; }
        public List<Category> Categories { get; set; }

        public ShopViewModel()
        {
            Products = new List<Product>();
            Categories = new List<Category>();
        }
    }
}
