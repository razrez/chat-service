using Microsoft.AspNetCore.Identity;

namespace Chat.Infrastructure.Persistence.Models;

public class User : IdentityUser
{
    public string Login { get; set; }
    public int Rating { get; set; }
    
}