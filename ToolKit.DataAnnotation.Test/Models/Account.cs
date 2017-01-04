using System;
using System.ComponentModel.DataAnnotations;
using Toolkit.DataAnnotations.Attributes;
using Toolkit.DataAnnotations.Attributes.Text;
using Toolkit.DataAnnotations.Enumerations;

namespace ToolKit.DataAnnotation.Test.Models
{
    public class Account
    {
        [Required]
        [StartsWithProperty(nameof(FirstName), ErrorMessage = "FULLNAME_SHOULD_START_WITH")]
        [EndsWithProperty(nameof(LastName), ErrorMessage = "FULLNAME_SHOULD_END_WITH")]
        [StringContainsProperty(nameof(MiddleName), ErrorMessage = "FULLNAME_SHOULD_CONTAIN_MIDDLE_NAME")]
        public string FullName { get; set; }

        [Required]
        public string FirstName { get; set; }

        [Required]
        public string MiddleName { get; set; }

        [Required]
        public string LastName { get; set; }
    }
}