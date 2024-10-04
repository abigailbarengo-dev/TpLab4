using System.ComponentModel.DataAnnotations;

namespace TpLab4.Models
{
    public class Pelicula
    {
        public int Id { get; set; }
        public int GeneroId { get; set; }

        [MaxLength(50)]
        public string Titulo { get; set; }

        public string Portada { get; set; }        

        public DateTime FechaEstreno { get; set; }

        public string Trailer { get; set; } // link

        public string Resumen {  get; set; }


    }
}
