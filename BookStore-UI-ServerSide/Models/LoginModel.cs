using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BookStore_UI_ServerSide.Models
{
    public class LoginModel
    {
        [Required]
        [Display(Name = "Nazwa uzytkownika jako e-mail")]
        public string UserName { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
