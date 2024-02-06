
using SQLite;

namespace Tarea_1._2.Models
{
    [Table("Personas")]
    public class Personas
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        [MaxLength(100)]
        public string Nombre { get; set; }

        [MaxLength(100)]
        public string Apellido { get; set; }

        public int Edad { get; set; }

        public string Correo { get; set; }

        public string Direccion { get; set; }
    }
}
