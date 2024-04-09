using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace Opain.Jarvis.Dominio.Entidades
{
    public class CategoriaPasajeros
    {
        [Key]
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]
        [MaxLength(5)]
        public string CodPasajero { get; set; }
        [Required]
        [MaxLength(50)]
        public string Descripcion { get; set; }
        [Required]
        public int Posicion { get; set; }
        [Required]
        [MaxLength(45)]
        public string Deja { get; set; }// Preguntar
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
