using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AtenasCalzado.Models
{
    public class Categoria
    {
        public int Id { get; set; }
        public string? Descripcion { get; set; }
        public List<Producto>? productos { get; set; }
    }
}
