using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ShopApp.Models;

namespace ShopApp.Controllers
{
    public class NotebooksController : Controller
    {
        private readonly ShopAppContext _context;

        public NotebooksController(ShopAppContext context)
        {
            _context = context;
        }

        // GET: Notebooks
        // Requires using Microsoft.AspNetCore.Mvc.Rendering;
        public async Task<IActionResult> Index(string notebookGPU, string searchString)
        {
            // Use LINQ to get list of GPUs.
            IQueryable<string> GPUQuery = from n in _context.Notebook
                                            orderby n.GPU
                                            select n.GPU;
            // select notebooks from database
            var notebooks = from n in _context.Notebook
                         select n;

            if (!String.IsNullOrEmpty(searchString))
            {   // return notebooks with given name
                notebooks = notebooks.Where(s => s.Name.Contains(searchString)); 
            }

            if (!String.IsNullOrEmpty(notebookGPU))
            {   // returns notebooks with given GPU
                notebooks = notebooks.Where(x => x.GPU == notebookGPU); 
            }

            // Create list with above conditions
            var notebookGPUType = new NotebookGPUViewModel();
            notebookGPUType.GPUs = new SelectList(await GPUQuery.Distinct().ToListAsync());
            notebookGPUType.notebooksList = await notebooks.ToListAsync();
            // Send summed up view with  aboveconditions to the view
            return View(notebookGPUType);
        }

        [HttpPost]
        public string Index(string searchString, bool notUsed)
        {
            return "From [HttpPost]Index: filter on " + searchString;
        }

        // GET: Notebooks/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var notebook = await _context.Notebook
                .SingleOrDefaultAsync(m => m.ID == id);
            if (notebook == null)
            {
                return NotFound();
            }

            return View(notebook);
        }

        // GET: Notebooks/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Notebooks/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,Name,GPU,RAM,Processor,ScreenSizeInch,RouteToImage,ImageBytes")] Notebook notebook)
        {
            if (ModelState.IsValid)
            {
                _context.Add(notebook);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(notebook);
        }

        // GET: Notebooks/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var notebook = await _context.Notebook.SingleOrDefaultAsync(m => m.ID == id);
            if (notebook == null)
            {
                return NotFound();
            }
            return View(notebook);
        }


        // GET: Notebooks/Edit/5
        public void DeleteDatabase(List<Notebook> NotebookList)
        {
            foreach (var ntb in NotebookList)
            {
                _context.Remove(ntb);
            }
            _context.SaveChanges();
        }


        // GET: Notebooks/Edit/5
        public async Task<IActionResult> DownloadData()
        {
            DownloadedNotebook dwnlNotebook = new DownloadedNotebook();
            dwnlNotebook.Start();
            dwnlNotebook.Init();

            foreach (var notebook in dwnlNotebook.notebookList)
                await Create(notebook);

            return RedirectToAction(nameof(Index));
        }

        // POST: Notebooks/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,Name,GPU,RAM,Processor,ScreenSizeInch,RouteToImage,ImageBytes")] Notebook notebook)
        {
            if (id != notebook.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(notebook);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!NotebookExists(notebook.ID))
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
            return View(notebook);
        }

        // GET: Notebooks/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var notebook = await _context.Notebook
                .SingleOrDefaultAsync(m => m.ID == id);
            if (notebook == null)
            {
                return NotFound();
            }

            return View(notebook);
        }

        // POST: Notebooks/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var notebook = await _context.Notebook.SingleOrDefaultAsync(m => m.ID == id);
            _context.Notebook.Remove(notebook);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool NotebookExists(int id)
        {
            return _context.Notebook.Any(e => e.ID == id);
        }
    }
}
