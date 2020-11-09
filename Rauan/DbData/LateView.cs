using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Rauan.DbData
{
    public class LateView
    {
        public int Id { get; set; }

        public string UserId { get; set; }
        public User User { get; set; }

        public int ProductId { get; set; }
        public Product Product { get; set; }

        public DateTime DTime { get; set; }
    }
}
