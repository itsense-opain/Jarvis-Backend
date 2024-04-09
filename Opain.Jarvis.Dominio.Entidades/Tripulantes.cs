using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace Opain.Jarvis.Dominio.Entidades
{
    public class Tripulantes
    {
        [Key]
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int IdTripulantes { get; set; }
        [MaxLength(150)]
        public string NombreTripulante { get; set; }
        [MaxLength(45)]
        public string LicenciaTripulante { get; set; }
        [MaxLength(45)]
        public string FuncionTripulante { get; set; }
        [Required]
        [ForeignKey("Aerolinea")]
        public int codigoAreolinea { get; set; }
        public Aerolinea Aerolinea { get; set; }
    }
}
