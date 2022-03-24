using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Models
{
    public class Regiao
    {
        public int RegiaoId { get; set; }
        public string RegiaoName { get; set; }

        public virtual ICollection<Cafe> Cafe { get; set; }


    }
}
