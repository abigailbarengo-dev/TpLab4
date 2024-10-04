using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TpLab4.Models;
using TpLab4.Servicios;

namespace TpLab4.Controllers
{
    public class PeliculaActorController : Controller
    {
        private readonly ApplicationDbContext context;

        public PeliculaActorController(ApplicationDbContext context)
        {
            this.context = context;
        }

        public IActionResult Index()
        {
            var peliculasActores = context.peliculaActores.ToList();
            return View(peliculasActores);
        }

        // GET: PeliculaActores
        public async Task<IActionResult> Index(ApplicationDbContext context)
        {
            return View(await context.peliculaActores.ToListAsync());
        }

        // GET: PeliculaActores/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var peliculaActores = await context.peliculaActores
                .FirstOrDefaultAsync(m => m.Id == id);
            if (peliculaActores == null)
            {
                return NotFound();
            }

            return View(peliculaActores);
        }

        // GET: PeliculaActores/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: PeliculaActores/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,PeliculaId,PersonaId")] PeliculaActores peliculaActores)
        {
            if (ModelState.IsValid)
            {
                context.Add(peliculaActores);
                await context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(peliculaActores);
        }

        // GET: PeliculaActores/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var peliculaActores = await context.peliculaActores.FindAsync(id);
            if (peliculaActores == null)
            {
                return NotFound();
            }
            return View(peliculaActores);
        }

        // POST: PeliculaActores/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,PeliculaId,PersonaId")] PeliculaActores peliculaActores)
        {
            if (id != peliculaActores.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    context.Update(peliculaActores);
                    await context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PeliculaActoresExists(peliculaActores.Id))
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
            return View(peliculaActores);
        }

        // GET: PeliculaActores/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var peliculaActores = await context.peliculaActores
                .FirstOrDefaultAsync(m => m.Id == id);
            if (peliculaActores == null)
            {
                return NotFound();
            }

            return View(peliculaActores);
        }

        // POST: PeliculaActores/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var peliculaActores = await context.peliculaActores.FindAsync(id);
            if (peliculaActores != null)
            {
                context.peliculaActores.Remove(peliculaActores);
            }

            await context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PeliculaActoresExists(int id)
        {
            return context.peliculaActores.Any(e => e.Id == id);
        }
    }
}
