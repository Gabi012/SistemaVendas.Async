using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SistemaVendas.Web.Data;
using SistemaVendas.Web.Models;


namespace SistemaVendas.Web.Controllers;


public class VendasController : Controller
{

    private readonly AppDbContext _context;


    public VendasController(
        AppDbContext context)
    {
        _context = context;
    }

    // GET: Vendas

    public async Task<IActionResult> Index()
    {
        var vendas = await _context.Vendas
            .OrderByDescending(x => x.DataVenda)
            .ToListAsync();


        return View(vendas);
    }

    // GET: Criar

    public IActionResult Create()
    {
        return View();
    }


    // POST: Criar

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(Venda venda)
    {

        if (ModelState.IsValid)
        {

            venda.Id = Guid.NewGuid();

            venda.DataVenda = DateTime.Now;

            venda.Status = "Finalizada";


            _context.Add(venda);


            await _context.SaveChangesAsync();


            return RedirectToAction(nameof(Index));

        }


        return View(venda);
    }

    // GET: Editar

    public async Task<IActionResult> Edit(Guid id)
    {

        var venda = await _context.Vendas
            .FirstOrDefaultAsync(x => x.Id == id);


        if (venda == null)
            return NotFound();


        return View(venda);

    }


    // POST: Editar


    [HttpPost]
    [ValidateAntiForgeryToken]

    public async Task<IActionResult> Edit(Guid id,Venda venda)
    {

        if (id != venda.Id)
            return NotFound();

        if (ModelState.IsValid)
        {
            _context.Update(venda);

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));

        }


        return View(venda);

    }


    // GET Details
    public async Task<IActionResult> Details(Guid id)
    {

        var venda = await _context.Vendas
            .FirstOrDefaultAsync(x => x.Id == id);


        if (venda == null)
            return NotFound();

        return View(venda);

    }

    // GET Delete
    public async Task<IActionResult> Delete(Guid id)
    {

        var venda = await _context.Vendas
            .FirstOrDefaultAsync(x => x.Id == id);

        if (venda == null)
            return NotFound();

        return View(venda);

    }

    // POST Delete
    [HttpPost, ActionName("Delete")]
    public async Task<IActionResult> DeleteConfirmado(Guid id)
    {
        var venda = await _context.Vendas
            .FindAsync(id);


        if (venda != null)
        {

            _context.Vendas.Remove(venda);

            await _context.SaveChangesAsync();
        }

        return RedirectToAction(nameof(Index));

    }

}