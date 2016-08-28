using System;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using AdvancedDataValidator.Enumerations;

namespace AdvancedDataValidator.ApiValidators
{
    public class UnixCurrentTimeCompareAttribute : ValidationAttribute
    {
        #region Constructors

        /// <summary>
        ///     Initialize comparision attribute.
        /// </summary>
        /// <param name="comparision"></param>
        public UnixCurrentTimeCompareAttribute(Comparision comparision)
        {
            _comparision = comparision;
            _originalUnixTime = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
        }

        #endregion

        #region Properties

        /// <summary>
        ///     Comparision mode.
        /// </summary>
        private readonly Comparision _comparision;

        /// <summary>
        ///     Time should be start from 1970.
        /// </summary>
        private readonly DateTime _originalUnixTime;

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
            // Value hasn't been defined. Treat this validation be successful.
            if (value == null)
                return ValidationResult.Success;

            // Every input will be casted to milliseconds.
            double unixTime;

            // Cast value to date time.
            if (value is DateTime)
            {
                var comparedTime = (DateTime) value;
                unixTime = UtcToMillisecond(comparedTime);
            }
            else
                unixTime = Convert.ToDouble(value);

            // Find the current unix time.
            var unixNow = UtcToMillisecond(DateTime.UtcNow);

            #region Comparision doing

            switch (_comparision)
            {
                case Comparision.Lower:
                    if (unixTime >= unixNow)
                        return new ValidationResult(FormatErrorMessage(validationContext.DisplayName));
                    break;
                case Comparision.LowerEqual:
                    if (unixTime > unixNow)
                        return new ValidationResult(FormatErrorMessage(validationContext.DisplayName));
                    break;
                case Comparision.Equal:
                    if (!unixTime.Equals(unixTime))
                        return new ValidationResult(FormatErrorMessage(validationContext.DisplayName));
                    break;
                case Comparision.NotEqual:
                    if (unixTime.Equals(unixNow))
                        return new ValidationResult(FormatErrorMessage(validationContext.DisplayName));
                    break;
                case Comparision.GreaterEqual:
                    if (unixTime < unixNow)
                        return new ValidationResult(FormatErrorMessage(validationContext.DisplayName));
                    break;
                case Comparision.Greater:
                    if (unixTime <= unixNow)
                        return new ValidationResult(FormatErrorMessage(validationContext.DisplayName));
                    break;
                default:
                    throw new NotImplementedException();
            }

            return ValidationResult.Success;

            #endregion
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
        ///     This function is for converting DateTime instance to milliseconds.
        /// </summary>
        /// <param name="convertedDateTime"></param>
        /// <returns></returns>
        private double UtcToMillisecond(DateTime convertedDateTime)
        {
            return convertedDateTime.ToUniversalTime()
                .Subtract(_originalUnixTime)
                .TotalMilliseconds;
        }

        #endregion
    }
}