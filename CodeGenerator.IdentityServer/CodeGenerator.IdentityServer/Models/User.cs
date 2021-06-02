using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace CodeGenerator.IdentityServer.Models
{
    public class User : IdentityUser<int>
    {
        [PersonalData]
        [MaxLength(256)]
        public string FirstName { get; set; }

        [PersonalData]
        [MaxLength(256)]
        public string LastName { get; set; }
    }
}
 