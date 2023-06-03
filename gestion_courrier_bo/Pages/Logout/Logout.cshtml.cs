using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Security.Claims;

namespace gestion_courrier_bo.Pages.Logout
{
    public class LogoutModel : PageModel
    {
       
        public async Task<IActionResult> OnGetAsync()
        {
            var claimsToRemove = User.Claims.ToList();

            foreach (var claim in claimsToRemove)
            {
                ((ClaimsIdentity)User.Identity).RemoveClaim(claim);
            }

            // Delete the authentication cookie
            await HttpContext.SignOutAsync(IdentityConstants.ApplicationScheme);

            // Redirect to the login page or any other desired page
            return RedirectToPage("/Login/Login");
        }
    }
}
