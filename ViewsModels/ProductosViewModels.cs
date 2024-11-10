using Microsoft.AspNetCore.Mvc.Rendering;
using AtenasCalzado.Models;

namespace AtenasCalzado.ViewsModels
{
    public class ProductosViewModels
    {
        public List<Producto>? productos { get; set; }
        public SelectList? Marcas { get; set; }
        public SelectList? Categorias { get; set; }
        public string? Nombre { get; set; }
        public Paginador Paginador { get; set; } = new Paginador();

    }
}
