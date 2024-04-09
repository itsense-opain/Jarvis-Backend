using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace Opain.Jarvis.Dominio.Entidades
{
    public class TasaAeroportuaria
    {
        [Key]
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]
        public decimal ValorCOP { get; set; }
        [Required]
        public decimal ValorUSD { get; set; }
        [DataType(DataType.Date)]
        [Required]
        public DateTime Fecha { get; set; }
    }
}
