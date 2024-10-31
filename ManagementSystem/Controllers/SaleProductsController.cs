using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ManagementSystem.Data;
using ManagementSystem.Models;

namespace ManagementSystem.Controllers
{
    public class SaleProductsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public SaleProductsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: SaleProducts
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.SaleProduct.Include(s => s.Product).Include(s => s.Sale);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: SaleProducts/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var saleProduct = await _context.SaleProduct
                .Include(s => s.Product)
                .Include(s => s.Sale)
                .FirstOrDefaultAsync(m => m.SaleProductId == id);
            if (saleProduct == null)
            {
                return NotFound();
            }

            return View(saleProduct);
        }

        // GET: SaleProducts/Create
        public IActionResult Create()
        {
            ViewData["ProductId"] = new SelectList(_context.Product, "ProductId", "ProductDescription");
            ViewData["SaleId"] = new SelectList(_context.Sale, "SaleId", "SaleId");
            return View();
        }

        // POST: SaleProducts/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("SaleProductId,SaleId,ProductId,SaleProductQuantity")] SaleProduct saleProduct)
        {
            if (ModelState.IsValid)
            {
                saleProduct.SaleProductId = Guid.NewGuid();
                _context.Add(saleProduct);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ProductId"] = new SelectList(_context.Product, "ProductId", "ProductDescription", saleProduct.ProductId);
            ViewData["SaleId"] = new SelectList(_context.Sale, "SaleId", "SaleId", saleProduct.SaleId);
            return View(saleProduct);
        }

        // GET: SaleProducts/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var saleProduct = await _context.SaleProduct.FindAsync(id);
            if (saleProduct == null)
            {
                return NotFound();
            }
            ViewData["ProductId"] = new SelectList(_context.Product, "ProductId", "ProductDescription", saleProduct.ProductId);
            ViewData["SaleId"] = new SelectList(_context.Sale, "SaleId", "SaleId", saleProduct.SaleId);
            return View(saleProduct);
        }

        // POST: SaleProducts/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("SaleProductId,SaleId,ProductId,SaleProductQuantity")] SaleProduct saleProduct)
        {
            if (id != saleProduct.SaleProductId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(saleProduct);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SaleProductExists(saleProduct.SaleProductId))
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
            ViewData["ProductId"] = new SelectList(_context.Product, "ProductId", "ProductDescription", saleProduct.ProductId);
            ViewData["SaleId"] = new SelectList(_context.Sale, "SaleId", "SaleId", saleProduct.SaleId);
            return View(saleProduct);
        }

        // GET: SaleProducts/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var saleProduct = await _context.SaleProduct
                .Include(s => s.Product)
                .Include(s => s.Sale)
                .FirstOrDefaultAsync(m => m.SaleProductId == id);
            if (saleProduct == null)
            {
                return NotFound();
            }

            return View(saleProduct);
        }

        // POST: SaleProducts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var saleProduct = await _context.SaleProduct.FindAsync(id);
            if (saleProduct != null)
            {
                _context.SaleProduct.Remove(saleProduct);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool SaleProductExists(Guid id)
        {
            return _context.SaleProduct.Any(e => e.SaleProductId == id);
        }
    }
}
