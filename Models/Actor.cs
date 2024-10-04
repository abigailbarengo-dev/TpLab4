using System.ComponentModel.DataAnnotations;

namespace TpLab4.Models
{
    public class Actor
    {
        public int Id { get; set; }
        [MaxLength(50)]
        public string Nombre { get; set; }

        public byte[] Foto { get; set; }        // VER?

        public DateTime FechaNacimiento { get; set; }


    }
}
