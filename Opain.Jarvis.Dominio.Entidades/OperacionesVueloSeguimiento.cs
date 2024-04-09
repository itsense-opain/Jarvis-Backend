using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace Opain.Jarvis.Dominio.Entidades
{
    public class OperacionesVueloSeguimiento
    {
        [Key]
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]
        [ForeignKey("OperacionesVuelo")]
        public int IdOperacionVuelo { get; set; }
        public OperacionesVuelo OperacionesVuelo { get; set; }
        [MaxLength(200)]
        public string Observacion { get; set; }
        [Required]
        [ForeignKey("U_Item")]
        public int Estado { get; set; }
        public U_Item U_Item { get; set; }
        [Required]
        [ForeignKey("Usuario")]
        public string IdUsuario { get; set; }
        public Usuario Usuario { get; set; }
        [Required]
        public DateTime FechaCreacion { get; set; }
    }
}
