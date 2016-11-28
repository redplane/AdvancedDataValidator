using System.Collections.Generic;
using Toolkit.DataAnnotations.Attributes;

namespace ToolKit.DataAnnotation.Test.Models
{
    public class Account
    {
        [ContainsNumerics(new [] {1, 2, 3, 4})]
        public int[] Roles { get; set; }

        [ContainsNumerics(new [] {1, 2})]
        public int[] Positions { get; set; }
    }
}