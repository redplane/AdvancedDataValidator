using System.ComponentModel.DataAnnotations;
using Toolkit.Validators.Attributes;
using Toolkit.Validators.Enumerations;
using Validator.Mvc.Resources;

namespace Validator.Mvc.Models
{
    public class AccountViewModel
    {
        [Required(ErrorMessageResourceType = typeof(Language), ErrorMessageResourceName = "ValueIsRequired")]
        public string Name { get; set; }
        
        [Required(ErrorMessageResourceType = typeof(Language), ErrorMessageResourceName = "ValueIsRequired")]
        [NumericCompare(100, Comparision.Greater, ErrorMessageResourceType = typeof(Language), ErrorMessageResourceName = "ValueMustBeGreaterThan")]
        public int Age { get; set; }
    }
}