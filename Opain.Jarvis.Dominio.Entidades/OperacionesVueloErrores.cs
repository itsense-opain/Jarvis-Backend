using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace Opain.Jarvis.Dominio.Entidades
{
    public class OperacionesVueloErrores
    {
        [Key]
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]
        [ForeignKey("OperacionesVuelo")]
        public int IdVuelo { get; set; }
        public OperacionesVuelo OperacionesVuelo { get; set; }
        [Required]
        [MaxLength(45)]
        public string TipoError { get; set; }
        [Required]
        [MaxLength(500)]
        public string Error { get; set; }
        [DataType(DataType.Date)]
        [Required]
        public DateTime Fecha { get; set; }
        [MaxLength(100)]
        public string Valores { get; set; }
        [MaxLength(10)]
        public int ValoresNuevos { get; set; }
        public int TipoValidacion { get; set; }
        public int TipoValidacion2 { get; set; }
    }
}
