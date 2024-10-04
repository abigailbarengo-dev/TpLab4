using System.ComponentModel.DataAnnotations;

namespace TpLab4.Models
{
    public class Genero
    {
        public int Id { get; set; }
        [MaxLength(150)]
        public string Descripcion { get; set; }

    }
}
