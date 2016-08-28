using System.Collections;
using System.ComponentModel.DataAnnotations;

namespace AdvancedDataValidator.ApiValidators
{
    /// <summary>
    ///     This attribute is for checking whether dictionary key number exceed the maximum.
    /// </summary>
    public class DictionaryLengthAttribute : ValidationAttribute
    {
        #region Properties

        /// <summary>
        ///     Length of key.
        /// </summary>
        private readonly int _length;

        #endregion

        #region Constructor

        /// <summary>
        ///     Initialize an instance of RegexMatchAttribute class.
        /// </summary>
        /// <param name="length"></param>
        public DictionaryLengthAttribute(int length)
        {
            _length = length;
        }

        #endregion

        #region Methods

        /// <summary>
        ///     Check whether regular expression is valid or not.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="validationContext"></param>
        /// <returns></returns>
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            // Cast the value to Dictionary<string, string>()
            var dict = value as IDictionary;

            // Key length is defined.
            if (dict?.Keys.Count > _length)
                return new ValidationResult(FormatErrorMessage(validationContext.DisplayName));

            return ValidationResult.Success;
        }

        #endregion
    }
}