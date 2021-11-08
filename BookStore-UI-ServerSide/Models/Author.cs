using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace BookStore_UI_ServerSide.Models
{
    public class Author
{
        public int Id { get; set; }
        
        [Required]
        [Display(Name = "Imie")]
        [DataType(DataType.Text)]
        public string FirstName { get; set; }
        
        [Required]
        [Display(Name = "Nazwisko")]
        [DataType(DataType.Text)]
        public string LastName { get; set; }
        
        [Required]
        [Display(Name = "O sobie")]
        [StringLength(250)]
        
        public string Bio { get; set; }

        public virtual IList<Book> Books { get; set; }
    }
}
