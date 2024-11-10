using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using AtenasCalzado.Data;
using AtenasCalzado.Models;
using AtenasCalzado.ViewsModels;
using OfficeOpenXml;

namespace AtenasCalzado.Controllers
{
    public class ProductosController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment env;

        public ProductosController(ApplicationDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            this.env = env;
        }

        // GET: Productos
        public async Task<IActionResult> Index(string busquedaNombre, int? CategoriaId, int? MarcaId, int pagina = 1)
        {
            var ApplicationDbContext = _context.productos.Include(a => a.categoria).Include(a => a.marca).Select(a => a);

            


            if (!string.IsNullOrEmpty(busquedaNombre))
            {
                ApplicationDbContext = ApplicationDbContext.Where(a => a.Nombre.Contains(busquedaNombre));
            }

            if (CategoriaId.HasValue)
            {
                ApplicationDbContext = ApplicationDbContext.Where(a => a.CategoriaId == CategoriaId.Value);
            }

            if (MarcaId.HasValue)
            {
                ApplicationDbContext = ApplicationDbContext.Where(a => a.MarcaId == MarcaId.Value);
            }
            ViewData["CategoriaId"] = new SelectList(_context.categorias, "Id", "Descripcion", CategoriaId);
            ViewData["MarcaId"] = new SelectList(_context.marcas, "Id", "Descripcion", MarcaId);

            int RegistrosPorPagina = 3;
            var registroMostrar = ApplicationDbContext
                                .Skip((pagina - 1) * RegistrosPorPagina)
                                .Take(RegistrosPorPagina);

            ProductosViewModels modelo = new ProductosViewModels()
            {
                productos = ApplicationDbContext.ToList(),
                Categorias = new SelectList(_context.categorias, "Id", "descripcion", CategoriaId),
                Marcas = new SelectList(_context.marcas, "Id", "descripcion", MarcaId),
                Nombre = busquedaNombre
            };

            modelo.Paginador.PaginaActual = pagina;
            modelo.Paginador.RegistrosPorPagina = RegistrosPorPagina;
            modelo.Paginador.TotalRegistros = await ApplicationDbContext.CountAsync();

            return View(modelo);


            //return View(await applicationDbContext.ToListAsync());
        }

        // GET: Productos/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }


            //var archivos = HttpContext.Request.Form.Files;
            //if (archivos != null && archivos.Count > 0)
            //{
            //    var archivoImagen = archivos[0];
            //    var pathDestino = Path.Combine(env.WebRootPath, "imagenes\\productos");
            //    if (archivoImagen.Length > 0)
            //    {
            //        var archivoDestino = Guid.NewGuid().ToString().Replace("-", "") + Path.GetExtension(archivoImagen.FileName);
            //        using (var filestream = new FileStream(Path.Combine(pathDestino, archivoDestino), FileMode.Create))
            //        {
            //            archivoImagen.CopyTo(filestream);
            //            producto.Imagen = archivoDestino;
            //        }
            //    }
            //}

            var producto = await _context.productos
               .Include(p => p.categoria)
               .Include(p => p.marca)
               .FirstOrDefaultAsync(m => m.Id == id);

            if (producto == null)
            {
                return NotFound();
            }

            return View(producto);
        }

        // GET: Productos/Create
        public IActionResult Create()
        {
            ViewData["CategoriaId"] = new SelectList(_context.categorias, "Id", "Descripcion");
            ViewData["MarcaId"] = new SelectList(_context.marcas, "Id", "Descripcion");
            return View();
        }

        // POST: Productos/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Nombre,Precio,Descripcion,Imagen,Stock,CategoriaId,MarcaId")] Producto producto)
        {
            if (ModelState.IsValid)
            {
                var archivos = HttpContext.Request.Form.Files;
                if (archivos != null && archivos.Count > 0)
                {
                    var archivoImagen = archivos[0];
                    var pathDestino = Path.Combine(env.WebRootPath, "imagenes\\productos");
                    if (archivoImagen.Length > 0)
                    {
                        var archivoDestino = Guid.NewGuid().ToString().Replace("-", "") + Path.GetExtension(archivoImagen.FileName);
                        using (var filestream = new FileStream(Path.Combine(pathDestino, archivoDestino), FileMode.Create))
                        {
                            archivoImagen.CopyTo(filestream);
                            producto.Imagen = archivoDestino;
                        }
                    }
                }
                _context.Add(producto);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CategoriaId"] = new SelectList(_context.categorias, "Id", "Descripcion", producto.CategoriaId);
            ViewData["MarcaId"] = new SelectList(_context.marcas, "Id", "Descripcion", producto.MarcaId);
            return View(producto);
        }

        // GET: Productos/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
           
            if (id == null)
            {
                return NotFound();
            }

            var producto = await _context.productos.FindAsync(id);
            if (producto == null)
            {
                return NotFound();
            }
            ViewData["CategoriaId"] = new SelectList(_context.categorias, "Id", "Descripcion", producto.CategoriaId);
            ViewData["MarcaId"] = new SelectList(_context.marcas, "Id", "Descripcion", producto.MarcaId);
            return View(producto);
        }

        // POST: Productos/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Nombre,Precio,Descripcion,Imagen,Stock,CategoriaId,MarcaId")] Producto producto)
        {
            if (id != producto.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var archivos = HttpContext.Request.Form.Files;
                if (archivos != null && archivos.Count > 0)
                {
                    var archivoImagen = archivos[0];
                    var pathDestino = Path.Combine(env.WebRootPath, "imagenes\\productos");
                    if (archivoImagen.Length > 0)
                    {
                        var archivoDestino = Guid.NewGuid().ToString().Replace("-", "") + Path.GetExtension(archivoImagen.FileName);
                        using (var filestream = new FileStream(Path.Combine(pathDestino, archivoDestino), FileMode.Create))
                        {
                            archivoImagen.CopyTo(filestream);
                            producto.Imagen = archivoDestino;
                        }
                    }
                }
                try
                {
                    _context.Update(producto);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductoExists(producto.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["CategoriaId"] = new SelectList(_context.categorias, "Id", "Descripcion", producto.CategoriaId);
            ViewData["MarcaId"] = new SelectList(_context.marcas, "Id", "Descipcion", producto.MarcaId);
            return View(producto);
        }

        // GET: Productos/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var producto = await _context.productos
                .Include(p => p.categoria)
                .Include(p => p.marca)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (producto == null)
            {
                return NotFound();
            }

            return View(producto);
        }

        // POST: Productos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var producto = await _context.productos.FindAsync(id);
            if (producto != null)
            {
                _context.productos.Remove(producto);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProductoExists(int id)
        {
            return _context.productos.Any(e => e.Id == id);
        }
        public async Task<IActionResult> ExportToExcel()
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            var productos = await _context.productos.ToListAsync();
            using (var package = new ExcelPackage())
            {
                var worksheet = package.Workbook.Worksheets.Add("Productos");

                worksheet.Cells[1, 1].Value = "NOMBRE";
                worksheet.Cells[1, 2].Value = "DESCRIPCION";
                worksheet.Cells[1, 3].Value = "STOCK";

                int row = 2;
                foreach (var producto in productos)
                {
                    worksheet.Cells[row, 1].Value = producto.Nombre;
                    worksheet.Cells[row, 2].Value = producto.Descripcion;
                    worksheet.Cells[row, 3].Value = producto.Stock;
                    row++;
                }
                worksheet.Cells[worksheet.Dimension.Address].AutoFitColumns();

                var stream = new MemoryStream();
                package.SaveAs(stream);
                stream.Position = 0;
                string excelName = $"Productos_{DateTime.Now:yyyyMMddHHmmss}.xlsx";
                return File(stream, "aplication/vmd.openxmlformats-officedocument.spreadsheetml.sheet", excelName);
            }

        }
    }
}
