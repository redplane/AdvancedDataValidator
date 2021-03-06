﻿using System;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;

namespace Toolkit.DataAnnotations.Attributes.Enumeration
{
    /// <summary>
    ///     This attribute is for checking whether a number is in an array or not.
    /// </summary>
    public class InEnumerationsArrayAttribute : ValidationAttribute
    {
        #region Properties

        /// <summary>
        ///     Values collection in which data must be equal.
        /// </summary>
        private readonly int[] _milesStone;

        #endregion

        #region Constructor

        /// <summary>
        ///     Initialize an instance of IntsMatchAttribute class.
        /// </summary>
        /// <param name="milestones"></param>
        public InEnumerationsArrayAttribute(int[] milestones)
        {
            _milesStone = milestones;
        }

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

            // Invalid milestone.
            if (_milesStone == null || _milesStone.Length < 1)
                throw new Exception("Invalid milestones.");

            // Convert value to int.
            var casted = Convert.ToInt32(value);

            if (!_milesStone.Any(x => x.Equals(casted)))
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
            // convert array to string array.
            var milestoneList = string.Join(",", _milesStone);

            return string.Format(CultureInfo.CurrentCulture, ErrorMessageString, name, milestoneList);
        }

        #endregion
    }
}