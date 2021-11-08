using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BookStore_API.DTOs
{
    public class UserDTO
    {
        [Required]
        [Display(Name = "Nazwa uzytkownika jako e-mail")]
        public string UserName { get; set; }

        [Required]
        [StringLength(50, MinimumLength = 4, ErrorMessage = "{0} must contain from  {2} to {1} characters.")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        
        [StringLength(50, MinimumLength = 4, ErrorMessage = "{0} must contain from  {2} to {1} characters.")]
        [DataType(DataType.Password)]
        public string ConfirmPassword { get; set; }
    }
}
