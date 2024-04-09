using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace Opain.Jarvis.Dominio.Entidades
{
    public class PoliticasDeTratamientoDeDatos
    {
        [Key]
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [MaxLength(150)]
        public string NombreUsuario { get; set; }
        [Column(TypeName = "BIT")]
        public bool AceptarPoliticas { get; set; }
        public DateTime Fecha { get; set; }
        [MaxLength(5)]
        public string Hora { get; set; }
        [MaxLength(256)]
        public string Email { get; set; }
        [MaxLength(45)]
        public string PhoneNumber { get; set; }        
        [MaxLength(20)]
        public string NumeroDocumento { get; set; }
        [MaxLength(50)]
        public string Cargo { get; set; }
        [MaxLength(150)]
        public string Aerolinea { get; set; }        
    }
}
