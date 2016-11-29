using System;
using Toolkit.DataAnnotations.Attributes;
using Toolkit.DataAnnotations.Enumerations;

namespace ToolKit.DataAnnotation.Test.Models
{
    public class Account
    {
        [IsLowerCase]
        public char Time { get; set; }
        
    }
}