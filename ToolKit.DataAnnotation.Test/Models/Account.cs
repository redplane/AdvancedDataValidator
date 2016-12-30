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
        [StringEqualsProperty("FirstName", StringComparison.InvariantCultureIgnoreCase)]
        public string FullName { get; set; }

        [Required]
        public string FirstName { get; set; }
    }
}