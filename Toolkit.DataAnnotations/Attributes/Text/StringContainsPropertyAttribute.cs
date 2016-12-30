using System;
using System.ComponentModel.DataAnnotations;
using System.Globalization;

namespace Toolkit.DataAnnotations.Attributes.Text
{
    public class StringContainsPropertyAttribute : ValidationAttribute
    {
        #region Properties

        /// <summary>
        /// Property which should be used for comparing with the current property.
        /// </summary>
        private readonly string _propertyName;
        
        #endregion

        #region Constructors

        /// <summary>
        ///    Initiate attribute with properties.
        /// </summary>
        /// <param name="propertyName"></param>
        public StringContainsPropertyAttribute(string propertyName)
        {
            // Invalid property name.
            if (string.IsNullOrEmpty(propertyName))
                throw new ArgumentNullException(nameof(propertyName));

            _propertyName = propertyName;
        }

        #endregion

        #region Methods

        /// <summary>
        ///     Check whether the validation is valid or not.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="validationContext"></param>
        /// <returns></returns>
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            #region Original model

            // Value hasn't been defined.
            if (value == null)
                return ValidationResult.Success;

            // Retrieve model information.
            var targetProperty = validationContext.ObjectType.GetProperty(_propertyName);

            // Target model hasn't been initialized.
            if (targetProperty == null)
                return ValidationResult.Success;

            #endregion

            #region Compared property

            // Retrieve value from target model.
            var targetValue = targetProperty.GetValue(validationContext.ObjectInstance, null);

            // Find the target string.
            var targetText = (string) targetValue;
            if (string.IsNullOrEmpty(targetText))
                return ValidationResult.Success;

            #endregion

            #region Comparisions

            // Value is not a string.
            if (!(value is string))
                throw new ArgumentException($"{nameof(value)} must be a string");

            // Find the source property value.
            var sourceValue = (string)value;

            // Source and target don't match.
            if (!sourceValue.Contains(targetText))
                return new ValidationResult(FormatErrorMessage(validationContext.DisplayName));

            #endregion

            return ValidationResult.Success;
        }

        /// <summary>
        ///     Override format error message to support multi parameters and multilingual.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public override string FormatErrorMessage(string name)
        {
            return string.Format(CultureInfo.CurrentCulture, ErrorMessageString, name, _propertyName);
        }

        #endregion
    }
}