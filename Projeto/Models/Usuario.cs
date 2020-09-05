using Microsoft.AspNetCore.Identity;

namespace Projeto.Models
{
    public class Usuario : IdentityUser
    {
        public string Status { get; set; }
        public string Perfil { get; set; }
    }
}