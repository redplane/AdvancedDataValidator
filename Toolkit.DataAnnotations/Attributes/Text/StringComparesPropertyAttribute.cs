using System;
using System.ComponentModel.DataAnnotations;
using System.Globalization;

namespace Toolkit.DataAnnotations.Attributes.Text
{
    /// <summary>
    /// Compare 2 properties which are strings
    /// Less than 0 : property A precedes property B in the sort order.
    /// 0           : property A occurs in the same position as property B in the sort order.
    /// More than 0 : property B follows property A in the sort order.
    /// </summary>
    public class StringComparesPropertyAttribute : ValidationAttribute
    {
        #region Properties

        /// <summary>
        /// Property which should be used for comparing with the current property.
        /// </summary>
        private readonly string _propertyName;

        /// <summary>
        /// Whether the strings are compared case-insensitive or not.
        /// </summary>
        private readonly StringComparison _stringComparison;

        /// <summary>
        /// Comparision mode.
        /// </summary>
        private readonly int _comparisionMode;

        #endregion

        #region Constructors

        /// <summary>
        ///    Initiate attribute with properties.
        /// </summary>
        /// <param name="propertyName"></param>
        /// <param name="stringComparision"></param>
        /// <param name="comparisionMode"></param>
        public StringComparesPropertyAttribute(string propertyName, StringComparison stringComparision, int comparisionMode)
        {
            // Invalid property name.
            if (string.IsNullOrEmpty(propertyName))
                throw new ArgumentNullException(nameof(propertyName));

            _propertyName = propertyName;
            _stringComparison = stringComparision;
            _comparisionMode = comparisionMode;
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
            var targetText = (string)targetValue;
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
            if (string.Compare(sourceValue, targetText, _stringComparison) != _comparisionMode)
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
            // No error message is defined for the attribute.
            if (string.IsNullOrEmpty(ErrorMessage))
                return string.Format(CultureInfo.CurrentCulture, $"{name} cannot be compared to {_propertyName}");
            
            return string.Format(CultureInfo.CurrentCulture, ErrorMessageString, name, _propertyName);
        }

        #endregion
    }
}