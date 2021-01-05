using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Rauan.DbData
{
    public class BrandPodCategory
    {
        public int Id { get; set; }


        [ForeignKey("Pod_Category")]
        public int Pod_CategoryId { get; set; }
        public Pod_Category Pod_Category { get; set; }


        [ForeignKey("Brand")]
        public int BrandId { get; set; }
        public Brand Brand { get; set; }
    }
}
