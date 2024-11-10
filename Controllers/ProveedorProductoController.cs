using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using AtenasCalzado.Data;
using AtenasCalzado.Models;

namespace AtenasCalzado.Controllers
{
    public class ProveedorProductoController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ProveedorProductoController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: ProveedorProducto
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.proveedorProductos.Include(p => p.producto).Include(p => p.proveedores);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: ProveedorProducto/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var proveedorProducto = await _context.proveedorProductos
                .Include(p => p.producto)
                .Include(p => p.proveedores)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (proveedorProducto == null)
            {
                return NotFound();
            }

            return View(proveedorProducto);
        }

        // GET: ProveedorProducto/Create
        public IActionResult Create()
        {
            ViewData["ProductoId"] = new SelectList(_context.productos, "Id", "Nombre");
            ViewData["ProveedorId"] = new SelectList(_context.proveedores, "Id", "Nombre");
            return View();
        }

        // POST: ProveedorProducto/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,ProductoId,ProveedorId")] ProveedorProducto proveedorProducto)
        {
            if (ModelState.IsValid)
            {
                _context.Add(proveedorProducto);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ProductoId"] = new SelectList(_context.productos, "Id", "Nombre", proveedorProducto.ProductoId);
            ViewData["ProveedorId"] = new SelectList(_context.proveedores, "Id", "Nombre", proveedorProducto.ProveedorId);

            return View(proveedorProducto);
        }

        // GET: ProveedorProducto/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var proveedorProducto = await _context.proveedorProductos.FindAsync(id);
            if (proveedorProducto == null)
            {
                return NotFound();
            }
            ViewData["ProductoId"] = new SelectList(_context.productos, "Id", "Nombre", proveedorProducto.ProductoId);
            ViewData["ProveedorId"] = new SelectList(_context.proveedores, "Id", "Nombre", proveedorProducto.ProveedorId);
            return View(proveedorProducto);
        }

        // POST: ProveedorProducto/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,ProductoId,ProveedorId")] ProveedorProducto proveedorProducto)
        {
            if (id != proveedorProducto.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(proveedorProducto);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProveedorProductoExists(proveedorProducto.Id))
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
            ViewData["ProductoId"] = new SelectList(_context.productos, "Id", "Nombre", proveedorProducto.ProductoId);
            ViewData["ProveedorId"] = new SelectList(_context.proveedores, "Id", "Nombre", proveedorProducto.ProveedorId);
            return View(proveedorProducto);
        }

        // GET: ProveedorProducto/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var proveedorProducto = await _context.proveedorProductos
                .Include(p => p.producto)
                .Include(p => p.proveedores)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (proveedorProducto == null)
            {
                return NotFound();
            }

            return View(proveedorProducto);
        }

        // POST: ProveedorProducto/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var proveedorProducto = await _context.proveedorProductos.FindAsync(id);
            if (proveedorProducto != null)
            {
                _context.proveedorProductos.Remove(proveedorProducto);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProveedorProductoExists(int id)
        {
            return _context.proveedorProductos.Any(e => e.Id == id);
        }
    }
}
