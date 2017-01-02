using System;
using System.ComponentModel.DataAnnotations;
using Toolkit.DataAnnotations.Attributes;
using Toolkit.DataAnnotations.Attributes.Text;
using Toolkit.DataAnnotations.Enumerations;

namespace ToolKit.DataAnnotation.Test.Models
{
    public class Account
    {
        /// <summary>
        /// Name of account
        /// </summary>
        [Required]
        [StringComparesProperty(nameof(FirstName),StringComparison.InvariantCultureIgnoreCase, -1)]
        public string FullName { get; set; }

        [Required]
        public string FirstName { get; set; }
    }
}