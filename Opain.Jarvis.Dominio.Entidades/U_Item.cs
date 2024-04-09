using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace Opain.Jarvis.Dominio.Entidades
{
    public class U_Item
    {
        [Key]
        [Required]
        public int Id { get; set; }
        [Required]
        [ForeignKey("U_Catalogo")]
        public int IdCatalogo { get; set; }
        public U_Catalogo U_Catalogo { get; set; }
        [MaxLength(255)]
        public string Nombre { get; set; }
        [MaxLength(255)]
        public string Descripcion { get; set; }
        public bool Activo { get; set; }
        public DateTime FechaCreacion { get; set; }
        [MaxLength(50)]
        public string Usuario  { get; set; }
    }
}
