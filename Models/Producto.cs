using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AtenasCalzado.Models
{
    public class Producto
    {
        public int Id { get; set; }
        public string? Nombre { get; set; }
        public decimal? Precio { get; set; }
        public string? Descripcion { get; set; }
        public string? Imagen { get; set; }
        public int? Stock { get; set; }
        public int? CategoriaId { get; set; }
        public int? MarcaId { get; set; }
        public Categoria? categoria { get; set; }
        public Marca? marca { get; set; }
    }
    
}
