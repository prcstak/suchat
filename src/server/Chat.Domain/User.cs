using Microsoft.AspNetCore.Identity;

namespace Chat.Domain;

public class User : IdentityUser
{
    public UserRoles Role { get; set; } 
    public List<Message> Messages { get; set; }
}