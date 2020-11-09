using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Rauan.DbData
{
    public class Comment
    {
        public int Id { get; set; }

        public string UserName { get; set; }

        public DateTime DTime { get; set; }

        public string CommentText { get; set; }

        public string BadText { get; set; }

        public string GoodText { get; set; }

        public int ProductId { get; set; }
        public Product Product { get; set; }
    }
}
