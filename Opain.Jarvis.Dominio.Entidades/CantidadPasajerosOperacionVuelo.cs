using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace Opain.Jarvis.Dominio.Entidades
{
    public class CantidadPasajerosOperacionVuelo
    {
        [Key]
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]
        [ForeignKey("CategoriaPasajeros")]
        public string CodPasajero { get; set; }
        public CategoriaPasajeros CategoriaPasajeros { get; set; }
        [Required]
        public int Cantidad { get; set; }
        [Required]
        public DateTime FechaCreacion { get; set; }
        [Required]
        public DateTime FechaActualizacion { get; set; }
        [Required]
        [ForeignKey("Usuario")]
        public string IdUsuario { get; set; }
        public Usuario Usuario { get; set; }
    }
}
