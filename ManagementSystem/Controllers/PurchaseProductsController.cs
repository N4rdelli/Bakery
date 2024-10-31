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
    public class PurchaseProductsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public PurchaseProductsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: PurchaseProducts
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.PurchaseProduct.Include(p => p.Product).Include(p => p.Purchase);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: PurchaseProducts/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var purchaseProduct = await _context.PurchaseProduct
                .Include(p => p.Product)
                .Include(p => p.Purchase)
                .FirstOrDefaultAsync(m => m.PurchaseProductId == id);
            if (purchaseProduct == null)
            {
                return NotFound();
            }

            return View(purchaseProduct);
        }

        // GET: PurchaseProducts/Create
        public IActionResult Create()
        {
            ViewData["ProductId"] = new SelectList(_context.Product, "ProductId", "ProductDescription");
            ViewData["PurchaseId"] = new SelectList(_context.Purchase, "PurchaseId", "PurchaseId");
            return View();
        }

        // POST: PurchaseProducts/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("PurchaseProductId,PurchaseId,ProductId,PurchaseProductQuantity,PurchaseProductPrice")] PurchaseProduct purchaseProduct)
        {
            if (ModelState.IsValid)
            {
                purchaseProduct.PurchaseProductId = Guid.NewGuid();
                _context.Add(purchaseProduct);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ProductId"] = new SelectList(_context.Product, "ProductId", "ProductDescription", purchaseProduct.ProductId);
            ViewData["PurchaseId"] = new SelectList(_context.Purchase, "PurchaseId", "PurchaseId", purchaseProduct.PurchaseId);
            return View(purchaseProduct);
        }

        // GET: PurchaseProducts/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var purchaseProduct = await _context.PurchaseProduct.FindAsync(id);
            if (purchaseProduct == null)
            {
                return NotFound();
            }
            ViewData["ProductId"] = new SelectList(_context.Product, "ProductId", "ProductDescription", purchaseProduct.ProductId);
            ViewData["PurchaseId"] = new SelectList(_context.Purchase, "PurchaseId", "PurchaseId", purchaseProduct.PurchaseId);
            return View(purchaseProduct);
        }

        // POST: PurchaseProducts/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("PurchaseProductId,PurchaseId,ProductId,PurchaseProductQuantity,PurchaseProductPrice")] PurchaseProduct purchaseProduct)
        {
            if (id != purchaseProduct.PurchaseProductId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(purchaseProduct);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PurchaseProductExists(purchaseProduct.PurchaseProductId))
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
            ViewData["ProductId"] = new SelectList(_context.Product, "ProductId", "ProductDescription", purchaseProduct.ProductId);
            ViewData["PurchaseId"] = new SelectList(_context.Purchase, "PurchaseId", "PurchaseId", purchaseProduct.PurchaseId);
            return View(purchaseProduct);
        }

        // GET: PurchaseProducts/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var purchaseProduct = await _context.PurchaseProduct
                .Include(p => p.Product)
                .Include(p => p.Purchase)
                .FirstOrDefaultAsync(m => m.PurchaseProductId == id);
            if (purchaseProduct == null)
            {
                return NotFound();
            }

            return View(purchaseProduct);
        }

        // POST: PurchaseProducts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var purchaseProduct = await _context.PurchaseProduct.FindAsync(id);
            if (purchaseProduct != null)
            {
                _context.PurchaseProduct.Remove(purchaseProduct);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PurchaseProductExists(Guid id)
        {
            return _context.PurchaseProduct.Any(e => e.PurchaseProductId == id);
        }
    }
}
