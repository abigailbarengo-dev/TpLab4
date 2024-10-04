using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using CsvHelper.Configuration;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using OfficeOpenXml;
using TpLab4.Models;
using TpLab4.Servicios;

namespace TpLab4.Controllers
{
    public class GeneroController : Controller
    {
        private readonly ApplicationDbContext context;

        public GeneroController(ApplicationDbContext context)
        {
            this.context = context;
        }

        public IActionResult Index()
        {
            var generos = context.genero.ToList();
            return View(generos);
        }


        // GET: Generoes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var genero = await context.genero
                .FirstOrDefaultAsync(m => m.Id == id);
            if (genero == null)
            {
                return NotFound();
            }

            return View(genero);
        }

        // GET: Generoes/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Generoes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Descripcion")] Genero genero)
        {
            if (ModelState.IsValid)
            {
                context.Add(genero);
                await context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(genero);
        }

        // GET: Generoes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var genero = await context.genero.FindAsync(id);
            if (genero == null)
            {
                return NotFound();
            }
            return View(genero);
        }

        // POST: Generoes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Descripcion")] Genero genero)
        {
            if (id != genero.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    context.Update(genero);
                    await context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!GeneroExists(genero.Id))
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
            return View(genero);
        }

        // GET: Generoes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var genero = await context.genero
                .FirstOrDefaultAsync(m => m.Id == id);
            if (genero == null)
            {
                return NotFound();
            }

            return View(genero);
        }

        // POST: Generoes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var genero = await context.genero.FindAsync(id);
            if (genero != null)
            {
                context.genero.Remove(genero);
            }

            await context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public IActionResult ImportGenres()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ImportGenres(IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest("No se ha seleccionado ningún archivo.");
            }

            var genres = new List<Genero>();

            if (Path.GetExtension(file.FileName).Equals(".xlsx", StringComparison.OrdinalIgnoreCase))
            {
                // Importar desde Excel
                using (var stream = new MemoryStream())
                {
                    await file.CopyToAsync(stream);
                    using (var package = new ExcelPackage(stream))
                    {
                        var worksheet = package.Workbook.Worksheets[0];
                        // Asumir la primera hoja
                        var rowCount = worksheet.Dimension.Rows;

                        for (var row = 2; row <= rowCount; row++) // Saltar la cabecera
                        {
                            var description = worksheet.Cells[row, 2].Value?.ToString();

                            if (!string.IsNullOrEmpty(description))
                            {
                                genres.Add(new Genero
                                {
                                    Descripcion = description
                                });
                            }
                        }
                    }
                }
            }
            else if (Path.GetExtension(file.FileName).Equals(".csv", StringComparison.OrdinalIgnoreCase))
            {
                // Importar desde CSV
                using (var reader = new StreamReader(file.OpenReadStream()))
                {
                    var config = new CsvConfiguration(CultureInfo.InvariantCulture)
                    {
                        HeaderValidated = null
                    };

                    using (var csv = new CsvHelper.CsvReader(reader, config))
                    {
                        var records = csv.GetRecords<Genero>();
                        genres.AddRange(records);
                    }
                }
            }
            else
            {
                return BadRequest("Formato de archivo no válido.");
            }

            context.genero.AddRange(genres);
            await context.SaveChangesAsync();

            return Ok("Géneros importados correctamente.");
        }

        private bool GeneroExists(int id)
        {
            return context.genero.Any(e => e.Id == id);
        }
    }
}
