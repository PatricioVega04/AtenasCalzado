using Microsoft.AspNetCore.Mvc.Rendering;

namespace AtenasCalzado.Models
{
    public class Paginador
    {
        public int PaginaActual { get; set; }
        public int RegistrosPorPagina { get; set; }
        public int TotalRegistros { get; set; }
        public int TotalPagina => (int)Math.Ceiling((decimal)TotalRegistros / RegistrosPorPagina);
        
    }


}
   
   

