using System.ComponentModel.DataAnnotations;
using System.Runtime.InteropServices;

namespace LinkTreeAPI.Model.LoginRequest
{
    public class UserLoginEmailRequest
    {
        [Required,EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required, DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
