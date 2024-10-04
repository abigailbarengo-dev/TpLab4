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
    public class PeliculaController : Controller
    {
        private readonly ApplicationDbContext context;

        public PeliculaController(ApplicationDbContext context)
        {
            this.context = context;
        }

        public IActionResult Index()
        {
            var peliculas = context.pelicula.ToList();
            return View(peliculas);
        }


        // GET: Peliculas
        
        // GET: Peliculas/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var pelicula = await context.pelicula
                .FirstOrDefaultAsync(m => m.Id == id);
            if (pelicula == null)
            {
                return NotFound();
            }

            return View(pelicula);
        }

        // GET: Peliculas/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Peliculas/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Pelicula pelicula, IFormFile portada)
        {
            if (ModelState.IsValid)
            {
                // Verificar si se subió una imagen
                if (portada != null && portada.Length > 0)
                {
                    // Definir la carpeta donde se guardarán las imágenes
                    var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images");

                    // Crear el nombre único del archivo
                    var uniqueFileName = Guid.NewGuid().ToString() + "_" + Path.GetFileName(portada.FileName);

                    // Ruta completa donde se guardará la imagen
                    var filePath = Path.Combine(uploadsFolder, uniqueFileName);

                    // Guardar la imagen en el servidor
                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        await portada.CopyToAsync(fileStream);
                    }

                    // Guardar la ruta o el nombre del archivo en la propiedad Portada
                    pelicula.Portada = "/images/" + uniqueFileName;
                }

                context.Add(pelicula);
                await context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(pelicula);
        }

        // GET: Peliculas/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var pelicula = await context.pelicula.FindAsync(id);
            if (pelicula == null)
            {
                return NotFound();
            }
            return View(pelicula);
        }

        // POST: Peliculas/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,GeneroId,Titulo,Portada,FechaEstreno,Trailer,Resumen")] Pelicula pelicula)
        {
            if (id != pelicula.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    context.Update(pelicula);
                    await context.SaveChangesAsync();
                }   
                catch (DbUpdateConcurrencyException)
                {
                    if (!PeliculaExists(pelicula.Id))
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
            return View(pelicula);
        }

        // GET: Peliculas/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var pelicula = await context.pelicula
                .FirstOrDefaultAsync(m => m.Id == id);
            if (pelicula == null)
            {
                return NotFound();
            }

            return View(pelicula);
        }

        // POST: Peliculas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var pelicula = await context.pelicula.FindAsync(id);
            if (pelicula != null)
            {
                context.pelicula.Remove(pelicula);
            }

            await context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PeliculaExists(int id)
        {
            return context.pelicula.Any(e => e.Id == id);
        }
    }
}
