using System.ComponentModel.DataAnnotations;

namespace BookStore_UI_ServerSide.Models
{
    public class RegistrationModel
    {
        [Required]
        public string UserName { get; set; }

        [Required]
        [StringLength(50, MinimumLength = 4, ErrorMessage = "{0} must contain from  {2} to {1} characters.")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required]
        [StringLength(50, MinimumLength = 4, ErrorMessage = "{0} must contain from  {2} to {1} characters.")]
        [DataType(DataType.Password)]
        public string ConfirmPassword { get; set; }
    }
}
