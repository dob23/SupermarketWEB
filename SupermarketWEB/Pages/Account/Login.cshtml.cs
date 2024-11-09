using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using SupermarketWEB.Data;
using SupermarketWEB.Models;
using System.Collections.Generic;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Autenticacion.Pages.Account
{
    public class LoginModel : PageModel
    {
        [BindProperty]
        public User User { get; set; } = new User();

        private readonly SupermarketContext _context;

        public LoginModel(SupermarketContext context)
        {
            _context = context;
        }

        public void OnGet() { }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid) return Page();

            // Buscar el usuario en la base de datos
            var storedUser = await _context.Users.FirstOrDefaultAsync(u => u.Username == User.Username);
            if (storedUser == null)
            {
                ModelState.AddModelError(string.Empty, "Nombre de usuario o contraseña incorrectos.");
                return Page();
            }

            // Hash de la contraseña ingresada por el usuario
            using (var sha256 = SHA256.Create())
            {
                var passwordHashBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(User.PasswordInput));
                var passwordHashString = Convert.ToBase64String(passwordHashBytes);

                // Comparar el hash de la contraseña ingresada con el almacenado
                if (passwordHashString != storedUser.Password)
                {
                    ModelState.AddModelError(string.Empty, "Nombre de usuario o contraseña incorrectos.");
                    return Page();
                }
            }

            // Autenticación exitosa
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, storedUser.Username),
            };
            var identity = new ClaimsIdentity(claims, "MyCookieAuth");
            ClaimsPrincipal claimsPrincipal = new ClaimsPrincipal(identity);
            await HttpContext.SignInAsync("MyCookieAuth", claimsPrincipal);

            return RedirectToPage("/Index"); // Redirigir a la página principal
        }

        public IActionResult OnPostRegister()
        {
            return RedirectToPage("/Account/Register"); // Redirige a la página de registro
        }
    }
}
