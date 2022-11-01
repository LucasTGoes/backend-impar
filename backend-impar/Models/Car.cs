using System.ComponentModel.DataAnnotations.Schema;

namespace backend_impar.Models
{
    public class Car
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Status { get; set; }

        [ForeignKey("Photo")]
        public int PhotoId { get; set; }
        public virtual Photo Photo { get; set; }

    }

}
