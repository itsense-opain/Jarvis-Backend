using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace Opain.Jarvis.Dominio.Entidades
{
    public class OperacionesVuelo
    {
        [Key]
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]
        [MaxLength(15)]
        public string MatriculaVuelo { get; set; }
        [DataType(DataType.Date)]
        [Required]
        public DateTime FechaVuelo { get; set; }
        [Required]
        [MaxLength(5)]
        public string HoraVuelo { get; set; }
        [Required]
        public int TotalEmbarcados { get; set; }
        [Required]
        public int INF { get; set; }
        [Required]
        public int TTL { get; set; }
        [Required]
        public int TTC { get; set; }
        [Required]
        public int EX { get; set; }
        [Required]
        public int TRIP { get; set; }
        [Required]
        public int PAX { get; set; }
        [Required]
        public int PagoCOP { get; set; }
        [Required]
        public int PagoUSD { get; set; }
        [Required]
        [MaxLength(5)]
        public string TipoVuelo { get; set; }
        [Required]
        [MaxLength(15)]
        public string NumeroVuelo { get; set; }
        [Required]
        [MaxLength(5)]
        public string Destino { get; set; }
        [Required]
        [ForeignKey("Aerolinea")]
        public int IdAerolinea { get; set; }
        public Aerolinea Aerolinea { get; set; }
        [Required]
        [ForeignKey("U_Item")]
        public int EstadoProceso { get; set; }
        public U_Item U_Item { get; set; }
        [Required]
        [MaxLength(15)]
        public string NumeroVueloLlegada { get; set; }
        [Required]
        [MaxLength(45)]
        public string OrigenDes { get; set; }
        [Required]
        [MaxLength(45)]
        public string Origen { get; set; }
        [DataType(DataType.Date)]
        [Required]
        public DateTime FechaLlegada { get; set; }
        [Required]
        [MaxLength(5)]
        public string HoraLlegada { get; set; }
        public int? TotalEmbarcadosAdd { get; set; }
        public int? TotalEmbarcados_LIQ { get; set; }
        public DateTime FechaCreacion { get; set; }
        public ICollection<RutaArchivos> Archivos { get; set; }
        public ICollection<Pasajero> Pasajeros { get; set; }
        public ICollection<PasajeroTransito> PasajerosTransitos { get; set; }
        public ICollection<NovedadProceso> NovedadesProceso { get; set; }
    }
}
