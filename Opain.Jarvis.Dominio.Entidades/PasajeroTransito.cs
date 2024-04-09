using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Opain.Jarvis.Dominio.Entidades
{
    public class PasajeroTransito
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
        public int IdVueloLlegada { get; set; }
        [Required]
        public int IdVueloSalida { get; set; }
        [Required]
        public string Categoria { get; set; }
        [Required]
        [DataType(DataType.Date)]
        public DateTime FechaLlegada { get; set; }
        [Required]
        [MaxLength(5)]
        public string HoraLlegada { get; set; }
        [Required]
        [DataType(DataType.Date)]
        public DateTime FechaSalida { get; set; }
        [Required]
        [MaxLength(5)]
        public string HoraSalida { get; set; }
        public DateTime FechaHoraCargue { get; set; }
        public int Firmado { get; set; }
        public DateTime FechaHoraFirma { get; set; }
        [Required]
        [MaxLength(15)]
        public string NumeroVueloSalida { get; set; }
        [Required]
        [MaxLength(5)]
        public string Destino { get; set; }
        [Required]
        [MaxLength(50)]
        public string AerolineaSalida { get; set; }
        [Required]
        [MaxLength(15)]
        public string NumeroVueloLlegada { get; set; }
        [Required]
        [MaxLength(5)]
        public string Origen { get; set; }   
        [Required]
        [MaxLength(50)]
        public string AerolineaLlegada { get; set; }
        [MaxLength(100)]
        public string Observaciones { get; set; }
        public bool TasaCobrada { get; set; }        
        public int IdCargue { get; set; }        
    }
}
