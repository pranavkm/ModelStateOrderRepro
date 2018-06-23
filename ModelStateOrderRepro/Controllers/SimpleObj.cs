using System.ComponentModel.DataAnnotations;

namespace ModelStateOrderRepro
{
    public class SimpleObj
    {
        [Required]
        public string Value { get; set; }
    }
}
