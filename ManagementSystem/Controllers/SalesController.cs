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
    public class SalesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public SalesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Sales
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Sale.Include(s => s.Customer).Include(s => s.SaleProducts).ThenInclude(sp => sp.Product);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Sales/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var sale = await _context.Sale
                .Include(s => s.Customer)
                .Include(s => s.SaleProducts).ThenInclude(sp => sp.Product)
                .FirstOrDefaultAsync(m => m.SaleId == id);
            if (sale == null)
            {
                return NotFound();
            }

            return View(sale);
        }

        // GET: Sales/Create
        public IActionResult Create()
        {
            ViewData["CustomerId"] = new SelectList(_context.Customer, "CustomerId", "CustomerCpf");
            ViewData["ProductId"] = new SelectList(_context.Product, "ProductId", "ProductName");
            return View();
        }

        // POST: Sales/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("SaleId,CustomerId,IssueDate")] Sale sale, List<SaleProduct> saleProducts)
        {
            if (ModelState.IsValid)
            {
                // Validação de estoque
                foreach (var saleProduct in saleProducts)
                {
                    var product = await _context.Product.FindAsync(saleProduct.ProductId);
                    if (product != null && product.ProductStockQuantity < saleProduct.SaleProductQuantity)
                    {
                        ModelState.AddModelError("SaleProducts", $"Insufficient stock for product: {product.ProductName}");
                        return View(sale);
                    }
                }

                // Atualiza o estoque
                foreach (var saleProduct in saleProducts)
                {
                    var product = await _context.Product.FindAsync(saleProduct.ProductId);
                    if (product != null)
                    {
                        product.ProductStockQuantity -= saleProduct.SaleProductQuantity;
                        _context.Product.Update(product);
                    }
                }

                // Salva a venda e os produtos
                _context.Add(sale);
                _context.SaveChanges();

                foreach (var saleProduct in saleProducts)
                {
                    saleProduct.SaleId = sale.SaleId;
                    _context.SaleProduct.Add(saleProduct);
                }
                _context.SaveChanges();

                return RedirectToAction(nameof(Index));
            }
            ViewData["CustomerId"] = new SelectList(_context.Customer, "CustomerId", "CustomerCpf", sale.CustomerId);
            ViewData["ProductId"] = new SelectList(_context.Product, "ProductId", "ProductName");
            return View(sale);
        }

        // GET: Sales/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var sale = await _context.Sale.Include(s => s.SaleProducts).ThenInclude(sp => sp.Product)
                .FirstOrDefaultAsync(m => m.SaleId == id);
            if (sale == null)
            {
                return NotFound();
            }
            ViewData["CustomerId"] = new SelectList(_context.Customer, "CustomerId", "CustomerCpf", sale.CustomerId);
            ViewData["ProductId"] = new SelectList(_context.Product, "ProductId", "ProductName");
            return View(sale);
        }

        // POST: Sales/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("SaleId,CustomerId,IssueDate")] Sale sale, List<SaleProduct> saleProducts)
        {
            if (id != sale.SaleId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    // Remove SaleProducts existentes
                    var existingSaleProducts = _context.SaleProduct.Where(sp => sp.SaleId == sale.SaleId).ToList();
                    _context.SaleProduct.RemoveRange(existingSaleProducts);
                    _context.SaveChanges();

                    // Atualiza estoque dos produtos existentes
                    foreach (var existingSaleProduct in existingSaleProducts)
                    {
                        var product = await _context.Product.FindAsync(existingSaleProduct.ProductId);
                        if (product != null)
                        {
                            product.ProductStockQuantity += existingSaleProduct.SaleProductQuantity;
                            _context.Product.Update(product);
                        }
                    }

                    // Adiciona os novos SaleProducts
                    foreach (var saleProduct in saleProducts)
                    {
                        saleProduct.SaleId = sale.SaleId;
                        _context.SaleProduct.Add(saleProduct);
                    }

                    // Salva as alterações e atualiza o estoque novamente
                    _context.SaveChanges();
                    await AtualizaEstoque(saleProducts);

                    _context.Update(sale);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SaleExists(sale.SaleId))
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
            ViewData["CustomerId"] = new SelectList(_context.Customer, "CustomerId", "CustomerCpf", sale.CustomerId);
            ViewData["ProductId"] = new SelectList(_context.Product, "ProductId", "ProductName");
            return View(sale);
        }

        // GET: Sales/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var sale = await _context.Sale
                .Include(s => s.Customer)
                .FirstOrDefaultAsync(m => m.SaleId == id);
            if (sale == null)
            {
                return NotFound();
            }

            return View(sale);
        }

        // POST: Sales/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var sale = await _context.Sale.FindAsync(id);
            if (sale != null)
            {
                // Remove SaleProducts existentes
                var existingSaleProducts = _context.SaleProduct.Where(sp => sp.SaleId == sale.SaleId).ToList();
                _context.SaleProduct.RemoveRange(existingSaleProducts);
                _context.SaveChanges();

                // Atualiza estoque dos produtos existentes
                foreach (var existingSaleProduct in existingSaleProducts)
                {
                    var product = await _context.Product.FindAsync(existingSaleProduct.ProductId);
                    if (product != null)
                    {
                        product.ProductStockQuantity += existingSaleProduct.SaleProductQuantity;
                        _context.Product.Update(product);
                    }
                }

                _context.Sale.Remove(sale);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }

        private bool SaleExists(int id)
        {
            return _context.Sale.Any(e => e.SaleId == id);
        }

        // Função para atualizar o estoque
        private async Task AtualizaEstoque(List<SaleProduct> saleProducts)
        {
            foreach (var saleProduct in saleProducts)
            {
                var product = await _context.Product.FindAsync(saleProduct.ProductId);
                if (product != null)
                {
                    product.ProductStockQuantity -= saleProduct.SaleProductQuantity;
                    _context.Product.Update(product);
                }
            }
            await _context.SaveChangesAsync();
        }
    }
}