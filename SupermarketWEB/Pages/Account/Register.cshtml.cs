using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SupermarketWEB.Data;
using SupermarketWEB.Models;

namespace Autenticacion.Pages.Account
{
    public class RegisterModel : PageModel
    {
        [BindProperty]
        public User User { get; set; }

        private readonly SupermarketContext _context;

        public RegisterModel(SupermarketContext context)
        {
            _context = context;
        }

        public void OnGet() { }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            // Crear el hash de la contraseña ingresada por el usuario
            using (var sha256 = SHA256.Create())
            {
                var passwordHashBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(User.PasswordInput));
                var passwordHashString = Convert.ToBase64String(passwordHashBytes); // Convertir el hash a Base64
                User.Password = passwordHashString; // Asignar el hash a Password
            }

            // Agregar el usuario a la base de datos
            _context.Users.Add(User);
            await _context.SaveChangesAsync();

            return RedirectToPage("/Account/Login");
        }
    }
}
