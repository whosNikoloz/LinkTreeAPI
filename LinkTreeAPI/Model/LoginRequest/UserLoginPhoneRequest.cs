using System.ComponentModel.DataAnnotations;

namespace LinkTreeAPI.Model.LoginRequest
{
    public class UserLoginPhoneRequest
    {
        [Required]
        public string PhoneNumber { get; set; } = string.Empty;


        [Required, DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
