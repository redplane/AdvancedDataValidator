using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Web.Mvc;

namespace Toolkit.DataAnnotations.Attributes.Text
{
    /// <summary>
    ///     This attribute is for checking whether a number is in an array of number or not.
    /// </summary>
    public class StartsWithAttribute : ValidationAttribute, IClientValidatable
    {
        #region Properties

        /// <summary>
        ///     String which data should start with.
        /// </summary>
        private readonly string _prefix;
        
        /// <summary>
        /// Mode of string comparision.
        /// </summary>
        private readonly StringComparison _stringComparison;
        
        #endregion

        #region Methods

        /// <summary>
        ///     Check whether property is valid or not.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="validationContext"></param>
        /// <returns></returns>
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            // Value is null. This mean no validation is specified.
            if (value == null)
                return ValidationResult.Success;
            
            // Prefix is null.
            if (_prefix == null)
                return ValidationResult.Success;
            
            // Value is not a string.
            if (!(value is string))
                throw new Exception($"{validationContext.DisplayName} must be a string");

            var content = (string) value;
            if (!content.StartsWith(_prefix, _stringComparison))
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
            return string.Format(CultureInfo.CurrentCulture, ErrorMessageString, name, _prefix);
        }

        /// <summary>
        /// Construct data annotation on client-side.
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
        /// Initiate attribute with settings.
        /// </summary>
        /// <param name="prefix"></param>
        public StartsWithAttribute(string prefix)
        {
            _prefix = prefix;
            _stringComparison = StringComparison.Ordinal;
        }


        /// <summary>
        /// Initiate attribute with settings.
        /// </summary>
        /// <param name="prefix"></param>
        /// <param name="stringComparison"></param>
        public StartsWithAttribute(string prefix, StringComparison stringComparison)
        {
            _prefix = prefix;
            _stringComparison = stringComparison;
        }

        #endregion
    }
}