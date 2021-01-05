using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Rauan.DbData
{
    public class Product
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string FirstInfo { get; set; }

        public int? Price { get; set; }

        public int? MinPay { get; set; }

        public string PublicImage { get; set; }

        public string Info1 { get; set; }
        public string Info2 { get; set; }
        public string Info3 { get; set; }
        public string Info4 { get; set; }
        public string Info5 { get; set; }
        public string Info6 { get; set; }
        public string Info7 { get; set; }
        public string Info8 { get; set; }

        public int? CategoryId { get; set; }

        public int Pod_CategoryId { get; set; }
        public Pod_Category Pod_Category { get; set; }



        [ForeignKey("Brand")]
        public int? BrandId { get; set; }
        public Brand Brand { get; set; }
    }
}
