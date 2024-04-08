using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace Opain.Jarvis.Dominio.Entidades
{
    [Table("Aeropuertos", Schema = "Opain")]
    public class Aeropuertos
    {
        [Key]
        [Required]
        [MaxLength(3)]
        public string CodigoIATA { get; set; }
        public bool CobroCausal64VuelosDom { get; set; }
        [MaxLength(45)]
        public string Ciudad { get; set; }
        [MaxLength(45)]
        public string Pais { get; set; }
    }
}
