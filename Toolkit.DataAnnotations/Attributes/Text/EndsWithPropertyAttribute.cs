using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Web.Mvc;

namespace Toolkit.DataAnnotations.Attributes.Text
{
    public class EndsWithPropertyAttribute : ValidationAttribute, IClientValidatable
    {
        #region Constructors

        /// <summary>
        ///     Initialize an instance of EpochTimeCompareAttribute with given information.
        /// </summary>
        /// <param name="propertyName"></param>
        public EndsWithPropertyAttribute(string propertyName)
        {
            // Invalid property name.
            if (string.IsNullOrEmpty(propertyName))
                throw new ArgumentNullException(nameof(propertyName));

            _propertyName = propertyName;
        }

        #endregion

        #region Properties

        /// <summary>
        ///     Property which should be used for comparing with the current property.
        /// </summary>
        private readonly string _propertyName;

        /// <summary>
        ///     Display name of target property.
        /// </summary>
        private string _propertyDisplayName;

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

            if (targetValue == null)
                return ValidationResult.Success;

            // Find the target string.
            var targetText = (string) targetValue;
            if (string.IsNullOrEmpty(targetText))
                return ValidationResult.Success;

            #endregion

            #region EndsWith comparision

            // Value is not a string.
            if (!(value is string))
                throw new ArgumentException($"{nameof(value)} must be a string");

            // Find the source property value.
            var sourceValue = (string) value;

            // Source doesn't start with the target.
            if (!sourceValue.EndsWith(targetText))
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
            if (string.IsNullOrEmpty(ErrorMessage))
                return string.Format(CultureInfo.CurrentCulture, ErrorMessageString, name, _propertyName);

            return string.Format(CultureInfo.CurrentCulture, ErrorMessageString, name, _propertyName);
        }

        /// <summary>
        /// Find validation rule on client-side.
        /// </summary>
        /// <param name="metadata"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public IEnumerable<ModelClientValidationRule> GetClientValidationRules(ModelMetadata metadata,
            ControllerContext context)
        {
            if (metadata.ContainerType != null)
                if (_propertyDisplayName == null)
                    _propertyDisplayName =
                        ModelMetadataProviders.Current.GetMetadataForProperty(() => metadata.Model,
                            metadata.ContainerType, _propertyName).GetDisplayName();

            var modelClientValidationRule = new ModelClientValidationRule();
            modelClientValidationRule.ErrorMessage = ErrorMessageString;
            modelClientValidationRule.ValidationType = "endswith";
            modelClientValidationRule.ValidationParameters.Add("property", _propertyName);
            modelClientValidationRule.ValidationParameters.Add("display", _propertyDisplayName);
            yield return modelClientValidationRule;
        }

        #endregion
    }
}