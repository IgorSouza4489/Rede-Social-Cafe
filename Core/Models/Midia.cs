using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Core.Models
{
   public class Midia
    {
        [Key]
        public int MidiasId { get; set; }
        [NotMapped]
        public IFormFile Imagem { get; set; }
        public string Foto { get; set; }
    }
}
