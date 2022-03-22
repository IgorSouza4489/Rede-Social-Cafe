using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Core.Models
{
    public class Cafe
    {
        public int Id { get; set; }
        [Required]
        public string Produtor { get; set; }
        [Required]
        public string NomeCafe { get; set; }
        [Required]
        public int Nota { get; set; }
        [Required]
        public string Regiao { get; set; }
        [Required]
        public string Impressoes { get; set; }
        [Required]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime PublishedDate { get; set; } = DateTime.Now;



        public ICollection<CafeComment> CafesComments { get; set; }


    }
}
