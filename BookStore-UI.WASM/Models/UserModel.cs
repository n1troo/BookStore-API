using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BookStore_UI.WASM.Models
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
    public class LoginModel
    {
        [Required]
        [Display(Name = "Nazwa uzytkownika")]
        public string UserName { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }

}
