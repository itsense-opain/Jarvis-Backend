using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Opain.Jarvis.Dominio.Entidades
{
    public class Pais
    {
        [Key]
        [Required]
        [MaxLength(2)]
        public string Id { get; set; }
        [Required]
        [MaxLength(100)]
        public string Nombre { get; set; }
        [Required]
        public bool IdEstado { get; set; }
        public ICollection<Ciudad> Ciudades { get; set; }
    }
}
