using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Models
{
   public class Produtor
    {
        public int ProdutorId { get; set; }
        public string ProdutorName { get; set; }
        public virtual ICollection<Cafe> Cafe { get; set; }
    }
}
