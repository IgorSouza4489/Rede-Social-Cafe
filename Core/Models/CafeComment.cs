using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Models
{
    public class CafeComment
    {
        public int Id { get; set; }
        public string Comments { get; set; }

        public DateTime PublishedDate { get; set; }
        public int CafesId { get; set; }
        public Cafe Cafes { get; set; }
        public int Rating { get; set; }
    }
}
