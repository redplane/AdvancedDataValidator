﻿using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using Toolkit.DataAnnotations.Enumerations;

namespace Toolkit.DataAnnotations.Attributes.Numeric
{
    public class NumericPropertyCompareAttribute : ValidationAttribute
    {
        #region Constructor

        /// <summary>
        ///     Initialize the validation attribute.
        /// </summary>
        /// <param name="model"></param>
        /// <param name="comparision"></param>
        public NumericPropertyCompareAttribute(string model, NumericComparision comparision)
        {
            // Invalid property name.
            if (model == null)
                throw new ArgumentNullException(nameof(model));

            // Update property name.
            _model = model;
            _comparision = comparision;
        }

        #endregion

        #region Properties

        /// <summary>
        ///     Property which is used for comparing with the source one.
        /// </summary>
        private readonly string _model;

        /// <summary>
        ///     Model display name.
        /// </summary>
        public string ModelDisplayName { get; internal set; }

        /// <summary>
        ///     Which way should 2 models be compared with each other.
        /// </summary>
        private readonly NumericComparision _comparision;

        #endregion

        #region Methods

        /// <summary>
        ///     Override the base function to display error message with custom parameters.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public override string FormatErrorMessage(string name)
        {
            return string.Format(CultureInfo.CurrentCulture, ErrorMessageString, name, ModelDisplayName ?? _model);
        }

        /// <summary>
        ///     This is for checking comparision is valid or not.
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
            var modelInfo = validationContext.ObjectType.GetProperty(_model);

            // Target model hasn't been initialized.
            if (modelInfo == null)
                return ValidationResult.Success;

            #endregion

            #region Compared property

            // Retrieve value from target model.
            var modelValue = modelInfo.GetValue(validationContext.ObjectInstance, null);

            if (modelValue == null)
                return ValidationResult.Success;

            #endregion

            // Convert both value to long.
            var compareOrigin = Convert.ToInt64(value);
            var milestone = Convert.ToInt64(modelValue);

            switch (_comparision)
            {
                case NumericComparision.Lower:
                {
                    if (compareOrigin >= milestone)
                    {
                        if (ModelDisplayName == null)
                            ModelDisplayName = FindPropertyDisplayName(validationContext.ObjectType, _model);

                        return new ValidationResult(FormatErrorMessage(validationContext.DisplayName));
                    }
                    break;
                }
                case NumericComparision.LowerEqual:
                {
                    if (compareOrigin > milestone)
                    {
                        if (ModelDisplayName == null)
                            ModelDisplayName = FindPropertyDisplayName(validationContext.ObjectType, _model);

                        return new ValidationResult(FormatErrorMessage(validationContext.DisplayName));
                    }
                    break;
                }
                case NumericComparision.Equal: // Value must be equal to milestone.
                {
                    if (compareOrigin != milestone)
                    {
                        if (ModelDisplayName == null)
                            ModelDisplayName = FindPropertyDisplayName(validationContext.ObjectType, _model);

                        return new ValidationResult(FormatErrorMessage(validationContext.DisplayName));
                    }
                    break;
                }
                case NumericComparision.GreaterEqual: // Value must be larger than or equal to milestone.
                {
                    if (compareOrigin < milestone)
                    {
                        if (ModelDisplayName == null)
                            ModelDisplayName = FindPropertyDisplayName(validationContext.ObjectType, _model);

                        return new ValidationResult(FormatErrorMessage(validationContext.DisplayName));
                    }
                    break;
                }
                case NumericComparision.Greater: // Value must be larger than milestone.
                {
                    if (compareOrigin <= milestone)
                    {
                        if (ModelDisplayName == null)
                            ModelDisplayName = FindPropertyDisplayName(validationContext.ObjectType, _model);

                        return new ValidationResult(FormatErrorMessage(validationContext.DisplayName));
                    }
                    break;
                }
                case NumericComparision.NotEqual:
                {
                    if (compareOrigin == milestone)
                    {
                        if (ModelDisplayName == null)
                            ModelDisplayName = FindPropertyDisplayName(validationContext.ObjectType, _model);

                        return new ValidationResult(FormatErrorMessage(validationContext.DisplayName));
                    }
                    break;
                }
            }

            return ValidationResult.Success;
        }

        /// <summary>
        ///     Retrieve display name of a property
        /// </summary>
        /// <param name="containerType"></param>
        /// <param name="propertyName"></param>
        /// <returns></returns>
        private string FindPropertyDisplayName(Type containerType, string propertyName)
        {
            var typeDescriptor = FindTypeDescriptor(containerType);
            var property = typeDescriptor.GetProperties().Find(propertyName, true);
            if (property == null)
                throw new ArgumentException(string.Format(CultureInfo.CurrentCulture,
                    "The property {0}.{1} could not be found", containerType.FullName, propertyName));
            var attributes = property.Attributes.Cast<Attribute>();
            var enumerable = attributes as Attribute[] ?? attributes.ToArray();
            var display = enumerable.OfType<DisplayAttribute>().FirstOrDefault();
            if (display != null)
                return display.GetName();

            var displayName = enumerable.OfType<DisplayNameAttribute>().FirstOrDefault();
            if (displayName != null)
                return displayName.DisplayName;

            return propertyName;
        }

        /// <summary>
        ///     Find the descryptor from a type.
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        private ICustomTypeDescriptor FindTypeDescriptor(Type type)
        {
            return new AssociatedMetadataTypeTypeDescriptionProvider(type).GetTypeDescriptor(type);
        }

        #endregion
    }
}