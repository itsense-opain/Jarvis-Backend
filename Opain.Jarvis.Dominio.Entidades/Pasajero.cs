using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace Opain.Jarvis.Dominio.Entidades
{
    public class Pasajero
    {
        [Key]
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]
        [ForeignKey("OperacionesVuelo")]
        public int IdOperacionVuelo { get; set; }
        public OperacionesVuelo OperacionesVuelo { get; set; }
        [Required]
        [MaxLength(250)]
        public string NombrePasajero { get; set; }
        [Required]
        [MaxLength(5)]
        public string IdCategoriaPasajero { get; set; }
        [Required]
        public DateTime FechaVuelo { get; set; }
        [Required]
        public string NumeroVuelo { get; set; }
        [Required]
        [MaxLength(15)]
        public string MatriculaVuelo { get; set; }
        [MaxLength(45)]
        public string Realiza_viaje { get; set; }
        [MaxLength(45)]
        public string Motivo_exencion { get; set; }
        public int IdCargue { get; set; }
    }
}
