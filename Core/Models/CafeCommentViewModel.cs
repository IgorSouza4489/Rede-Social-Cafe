using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Models
{
    public class CafeCommentViewModel
    {
        public string Title { get; set; }

        public string NomeCafe { get; set; }
        public List<CafeComment> ListOfComments { get; set; }
        public string Comment { get; set; }
        public int CafesId { get; set; }
        public int Rating { get; set; }
    }
}
