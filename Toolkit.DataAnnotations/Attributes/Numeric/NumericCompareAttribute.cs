using System;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using Toolkit.DataAnnotations.Enumerations;

namespace Toolkit.DataAnnotations.Attributes.Numeric
{
    /// <summary>
    ///     This attribute is used for comparing 2 numeric typed attributes.
    /// </summary>
    public class NumericCompareAttribute : ValidationAttribute
    {
        #region Constructor

        /// <summary>
        ///     Initialize an instance of NumericCompareAttribute class with long milestone.
        /// </summary>
        /// <param name="milestone"></param>
        /// <param name="comparision"></param>
        public NumericCompareAttribute(long milestone, NumericComparision comparision)
        {
            _milestone = Convert.ToDouble(milestone);
            _comparision = comparision;
        }

        /// <summary>
        ///     Initialize an instance of NumericCompareAttribute class with integer milestone.
        /// </summary>
        /// <param name="milestone"></param>
        /// <param name="comparision"></param>
        public NumericCompareAttribute(int milestone, NumericComparision comparision)
        {
            _milestone = milestone;
            _comparision = comparision;
        }

        /// <summary>
        ///     Initialize an instance of NumericCompareAttribute class with byte milestone.
        /// </summary>
        /// <param name="milestone"></param>
        /// <param name="comparision"></param>
        public NumericCompareAttribute(byte milestone, NumericComparision comparision)
        {
            _milestone = milestone;
            _comparision = comparision;
        }

        #endregion

        #region Property

        /// <summary>
        ///     Minimum value which property should be greater than.
        /// </summary>
        private readonly double _milestone;

        /// <summary>
        ///     Comparision mode.
        /// </summary>
        private readonly NumericComparision _comparision;

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

            // Convert value to int.
            var convertedValue = Convert.ToDouble(value);

            switch (_comparision)
            {
                case NumericComparision.Lower:
                {
                    if (convertedValue >= _milestone)
                        return
                            new ValidationResult(string.Format(FormatErrorMessage(validationContext.DisplayName),
                                _milestone));

                    break;
                }
                case NumericComparision.LowerEqual:
                {
                    if (convertedValue > _milestone)
                        return
                            new ValidationResult(string.Format(FormatErrorMessage(validationContext.DisplayName),
                                _milestone));
                    
                    break;
                }
                case NumericComparision.Equal: // Value must be equal to milestone.
                {
                    if (!Equals(convertedValue, _milestone))
                        return
                            new ValidationResult(string.Format(FormatErrorMessage(validationContext.DisplayName),
                                _milestone));
                    break;
                }
                case NumericComparision.GreaterEqual: // Value must be larger than or equal to milestone.
                {
                    if (convertedValue < _milestone)
                        return
                            new ValidationResult(string.Format(FormatErrorMessage(validationContext.DisplayName),
                                _milestone));
                    break;
                }
                case NumericComparision.Greater: // Value must be larger than milestone.
                {
                    if (convertedValue <= _milestone)
                        return
                            new ValidationResult(string.Format(FormatErrorMessage(validationContext.DisplayName),
                                _milestone));
                    break;
                }
            }

            return ValidationResult.Success;
        }

        /// <summary>
        ///     Override format error message to support multi parameters and multilingual.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public override string FormatErrorMessage(string name)
        {
            return string.Format(CultureInfo.CurrentCulture, ErrorMessageString, name, _milestone);
        }
        
        #endregion
    }
}