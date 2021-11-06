using System.ComponentModel.DataAnnotations;

namespace AspNetCore5.Models
{
    public class FormViewModel
    {
        [Required]
        public string Input01 { get; set; }

        public string Input02 { get; set; }

        public string Input03 { get; set; }

        public string Password { get; set; }
    }
}
