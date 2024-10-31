using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using SupermarketWEB.Data;
using SupermarketWEB.Models;

namespace SupermarketWEB.Pages.PayModels
{
    public class DeleteModel : PageModel
    {
        private readonly SupermarketContext _context;

        public DeleteModel(SupermarketContext context)
        {
            _context = context;
        }

        [BindProperty]
        public PayMode PayModel { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null || _context.PayModel == null)
            {
                return NotFound();
            }
            var payModel = await _context.PayModel.FirstOrDefaultAsync(m => m.Id == id);

            if (payModel == null)
            {
                return NotFound();
            }
            else
            {
                PayModel = payModel;
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null || _context.PayModel == null)
            {
                return NotFound();
            }
            var payModel = await _context.PayModel.FindAsync(id);
            if (payModel != null)
            {
                PayModel = payModel;
                _context.PayModel.Remove(payModel);
                await _context.SaveChangesAsync();
            }
            return RedirectToPage("./Index");
        }
    }
}
