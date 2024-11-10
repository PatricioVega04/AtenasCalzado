using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AtenasCalzado.Models
{
    public class ProveedorProducto
    {
        public int Id { get; set; }
        public int ProductoId { get; set; }
        public int ProveedorId { get; set; }
        public Producto producto { get; set; }
        public Proveedor proveedores { get; set; }
    }
}
