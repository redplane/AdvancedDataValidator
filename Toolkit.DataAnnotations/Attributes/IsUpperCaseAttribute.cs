using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Text.RegularExpressions;
using System.Web.Mvc;

namespace Toolkit.DataAnnotations.Attributes
{
    /// <summary>
    ///     This attribute is for checking whether a number is in an array of number or not.
    /// </summary>
    public class IsUpperCaseAttribute : ValidationAttribute, IClientValidatable
    {
        #region Properties
        
        /// <summary>
        /// Lower case regular expression.
        /// </summary>
        private readonly Regex _regexUppercase;

        #endregion

        #region Methods

        /// <summary>
        ///     Check whether property is valid or not.
        /// </summary>
        /// <param name="source"></param>
        /// <param name="validationContext"></param>
        /// <returns></returns>
        protected override ValidationResult IsValid(object source, ValidationContext validationContext)
        {
            // Value is null. This mean no validation is specified.
            if (source == null)
                return ValidationResult.Success;

            // No matter what source is (array of chars , string ...) everything should be casted into string.
            string comparer = null;

            // Source is an array of characters.
            if (source is char[])
            {
                var characters = (char[]) source;
                comparer = string.Join("", characters);
            }
            else if (source is IList<char> || source is IEnumerable<char>)
            {
                var characters = (IEnumerable<char>) source;
                comparer = string.Join("", characters);
            }
            else if (source is char)
            {
                var character = (char) source;
                comparer = $"{character}";
            }
            
            // Cannot cast data.
            if (comparer == null)
                throw new Exception($"{validationContext.DisplayName} must be an array of character or a string");

            // Check whether string matches the regex or not.
            if (!_regexUppercase.IsMatch(comparer))
                return new ValidationResult(FormatErrorMessage(validationContext.DisplayName));

            return ValidationResult.Success;
        }

        /// <summary>
        ///     Override format error message to support multi parameters and multilingual.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public override string FormatErrorMessage(string name)
        {
            return string.Format(CultureInfo.CurrentCulture, ErrorMessageString, name);
        }

        /// <summary>
        /// Check whether numeric 
        /// </summary>
        /// <param name="metadata"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public IEnumerable<ModelClientValidationRule> GetClientValidationRules(ModelMetadata metadata, ControllerContext context)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region Constructor

        /// <summary>
        ///   Initiate attributes with comparision target.
        /// </summary>
        public IsUpperCaseAttribute()
        {
            // Regular expression to check whether whole string contains only lower-cased characters or not.
            _regexUppercase = new Regex("^[A-Z]+$");
        }
        
        #endregion
    }
}