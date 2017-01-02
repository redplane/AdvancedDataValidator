using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using System.Web.Mvc;
using Toolkit.DataAnnotations.Enumerations;

namespace Toolkit.DataAnnotations.Attributes
{
    /// <summary>
    ///     This attribute is for checking whether a number is in an array of number or not.
    /// </summary>
    public class ContainsNumericsAttribute : ValidationAttribute, IClientValidatable
    {
        #region Properties

        /// <summary>
        ///     Values collection in which data must be equal.
        /// </summary>
        private readonly double[] _numericsList;

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

            // Invalid milestone.
            if ((_numericsList == null) || (_numericsList.Length < 1))
                throw new Exception("Invalid milestones.");

            // Result after validation.
            ValidationResult validationResult = null;

            // Whether property is numerics array or not.
            if (IsNumericsContainsNumerics(source, validationContext, ref validationResult) == ValidationSteps.Stop)
                return validationResult;

            // Whether property is a list or not.
            if (IsNumericsListContainsNumerics(source, validationContext, ref validationResult) == ValidationSteps.Stop)
                return validationResult;

            // Whether property is an enumerable or not.
            if (IsNumericsEnumerableContainsNumerics(source, validationContext, ref validationResult) == ValidationSteps.Stop)
                return validationResult;

            throw new Exception($"{validationContext.DisplayName} must be a collection of numeric.");
        }

        /// <summary>
        ///     Override format error message to support multi parameters and multilingual.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public override string FormatErrorMessage(string name)
        {
            var milestoneList = string.Join(",", _numericsList);
            return string.Format(CultureInfo.CurrentCulture, ErrorMessageString, name, milestoneList);
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
        /// <param name="numericsList"></param>
        public ContainsNumericsAttribute(double[] numericsList)
        {
            _numericsList = numericsList.Distinct().ToArray();
        }

        /// <summary>
        ///   Initiate attributes with comparision target.
        /// </summary>
        /// <param name="numericsList"></param>
        public ContainsNumericsAttribute(int[] numericsList)
        {
            _numericsList = numericsList.Select(Convert.ToDouble).Distinct().ToArray();
        }

        /// <summary>
        ///   Initiate attributes with comparision target.
        /// </summary>
        /// <param name="numericsList"></param>
        public ContainsNumericsAttribute(object[] numericsList)
        {
            _numericsList = numericsList.Select(Convert.ToDouble).Distinct().ToArray();
        }

        /// <summary>
        /// Check whether property contains numerics list or not.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="validationContext"></param>
        /// <param name="validationResult"></param>
        private ValidationSteps IsNumericsContainsNumerics(object value, ValidationContext validationContext, ref ValidationResult validationResult)
        {
            // Find numerics values from object.
            double[] numerics = null;

            // Value is signed byte array.
            if (value is sbyte[])
            {
                var sbytes = (sbyte[])value;
                numerics = sbytes.Select(Convert.ToDouble).ToArray();
            }
            else if (value is byte[])
            {
                var bytes = (byte[])value;
                numerics = bytes.Select(Convert.ToDouble).ToArray();
            }
            else if (value is short[])
            {
                var shorts = (short[])value;
                numerics = shorts.Select(Convert.ToDouble).ToArray();
            }
            else if (value is ushort[])
            {
                var ushorts = (ushort[])value;
                numerics = ushorts.Select(Convert.ToDouble).ToArray();
            }
            else if (value is int[])
            {
                var ints = (int[])value;
                numerics = ints.Select(Convert.ToDouble).ToArray();
            }
            else if (value is uint[])
            {
                var uints = (uint[])value;
                numerics = uints.Select(Convert.ToDouble).ToArray();
            }
            else if (value is long[])
            {
                var longs = (long[])value;
                numerics = longs.Select(Convert.ToDouble).ToArray();
            }
            else if (value is ulong[])
            {
                var ulongs = (ulong[])value;
                numerics = ulongs.Select(Convert.ToDouble).ToArray();
            }
            else if (value is float[])
            {
                var floats = (float[])value;
                numerics = floats.Select(Convert.ToDouble).ToArray();
            }
            else if (value is double[])
            {
                numerics = (double[])value;
            }
            else if (value is decimal[])
            {
                var decimals = (decimal[])value;
                numerics = decimals.Select(Convert.ToDouble).ToArray();
            }

            // Invalid cast.
            if (numerics == null)
                return ValidationSteps.Next;

            // Invalid numeric collection or source length is shorter than target one length.
            if (numerics.Length < _numericsList.Length)
            {
                validationResult = new ValidationResult(FormatErrorMessage(validationContext.DisplayName));
                return ValidationSteps.Stop;
            }
            // Collection doesn't contain all element comes from target.
            if (_numericsList.Any(x => !numerics.Contains(x)))
            {
                validationResult = new ValidationResult(FormatErrorMessage(validationContext.DisplayName));
                return ValidationSteps.Stop;
            }

            validationResult = ValidationResult.Success;
            return ValidationSteps.Stop;

        }

        /// <summary>
        /// Check whether property is a list and contains target collection or not.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="validationContext"></param>
        /// <param name="validationResult"></param>
        /// <returns></returns>
        private ValidationSteps IsNumericsListContainsNumerics(object value, ValidationContext validationContext, ref ValidationResult validationResult)
        {
            // Value is not a list.
            if (!(value is IList))
                return ValidationSteps.Next;

            // Find numerics values from object.
            double[] numerics = null;

            // Value is signed byte array.
            if (value is IList<sbyte>)
            {
                var sbytes = (IList<sbyte>)value;
                numerics = sbytes.Select(Convert.ToDouble).ToArray();
            }
            else if (value is IList<byte>)
            {
                var bytes = (IList<byte>)value;
                numerics = bytes.Select(Convert.ToDouble).ToArray();
            }
            else if (value is IList<short>)
            {
                var shorts = (IList<short>)value;
                numerics = shorts.Select(Convert.ToDouble).ToArray();
            }
            else if (value is IList<ushort>)
            {
                var ushorts = (IList<ushort>)value;
                numerics = ushorts.Select(Convert.ToDouble).ToArray();
            }
            else if (value is IList<int>)
            {
                var ints = (IList<int>)value;
                numerics = ints.Select(Convert.ToDouble).ToArray();
            }
            else if (value is IList<uint>)
            {
                var uints = (IList<uint>)value;
                numerics = uints.Select(Convert.ToDouble).ToArray();
            }
            else if (value is IList<long>)
            {
                var longs = (IList<long>)value;
                numerics = longs.Select(Convert.ToDouble).ToArray();
            }
            else if (value is IList<IList>)
            {
                var ulongs = (IList<IList>)value;
                numerics = ulongs.Select(Convert.ToDouble).ToArray();
            }
            else if (value is IList<float>)
            {
                var floats = (IList<float>)value;
                numerics = floats.Select(Convert.ToDouble).ToArray();
            }
            else if (value is IList<double>)
            {
                var doubles = (IList<double>)value;
                numerics = doubles.Select(x => x).ToArray();
            }
            else if (value is IList<decimal>)
            {
                var decimals = (IList<decimal>)value;
                numerics = decimals.Select(Convert.ToDouble).ToArray();
            }

            // Invalid cast.
            if (numerics == null)
                return ValidationSteps.Next;

            // Invalid numeric collection or source length is shorter than target one length.
            if (numerics.Length < _numericsList.Length)
            {
                validationResult = new ValidationResult(FormatErrorMessage(validationContext.DisplayName));
                return ValidationSteps.Stop;
            }

            // Collection doesn't contain all element comes from target.
            if (_numericsList.Any(x => !numerics.Contains(x)))
            {
                validationResult = new ValidationResult(FormatErrorMessage(validationContext.DisplayName));
                return ValidationSteps.Stop;
            }

            validationResult = ValidationResult.Success;
            return ValidationSteps.Stop;
        }

        /// <summary>
        /// Check whether property is a list and contains target collection or not.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="validationContext"></param>
        /// <param name="validationResult"></param>
        /// <returns></returns>
        private ValidationSteps IsNumericsEnumerableContainsNumerics(object value, ValidationContext validationContext, ref ValidationResult validationResult)
        {
            // Value is not a list.
            if (!(value is IEnumerable))
                return ValidationSteps.Next;

            // Find numerics values from object.
            double[] numerics = null;

            // Value is signed byte array.
            if (value is IEnumerable<sbyte>)
            {
                var sbytes = (IEnumerable<sbyte>)value;
                numerics = sbytes.Select(Convert.ToDouble).ToArray();
            }
            else if (value is IEnumerable<byte>)
            {
                var bytes = (IEnumerable<byte>)value;
                numerics = bytes.Select(Convert.ToDouble).ToArray();
            }
            else if (value is IEnumerable<short>)
            {
                var shorts = (IEnumerable<short>)value;
                numerics = shorts.Select(Convert.ToDouble).ToArray();
            }
            else if (value is IEnumerable<ushort>)
            {
                var ushorts = (IEnumerable<ushort>)value;
                numerics = ushorts.Select(Convert.ToDouble).ToArray();
            }
            else if (value is IEnumerable<int>)
            {
                var ints = (IEnumerable<int>)value;
                numerics = ints.Select(Convert.ToDouble).ToArray();
            }
            else if (value is IEnumerable<uint>)
            {
                var uints = (IEnumerable<uint>)value;
                numerics = uints.Select(Convert.ToDouble).ToArray();
            }
            else if (value is IEnumerable<long>)
            {
                var longs = (IEnumerable<long>)value;
                numerics = longs.Select(Convert.ToDouble).ToArray();
            }
            else if (value is IEnumerable<IEnumerable>)
            {
                var ulongs = (IEnumerable<IEnumerable>)value;
                numerics = ulongs.Select(Convert.ToDouble).ToArray();
            }
            else if (value is IEnumerable<float>)
            {
                var floats = (IEnumerable<float>)value;
                numerics = floats.Select(Convert.ToDouble).ToArray();
            }
            else if (value is IEnumerable<double>)
            {
                var doubles = (IEnumerable<double>)value;
                numerics = doubles.Select(x => x).ToArray();
            }
            else if (value is IEnumerable<decimal>)
            {
                var decimals = (IEnumerable<decimal>)value;
                numerics = decimals.Select(Convert.ToDouble).ToArray();
            }

            // Invalid cast.
            if (numerics == null)
                return ValidationSteps.Next;

            // Invalid numeric collection or source length is shorter than target one length.
            if (numerics.Length < _numericsList.Length)
            {
                validationResult = new ValidationResult(FormatErrorMessage(validationContext.DisplayName));
                return ValidationSteps.Stop;
            }
            // Collection doesn't contain all element comes from target.
            if (_numericsList.Any(x => !numerics.Contains(x)))
            {
                validationResult = new ValidationResult(FormatErrorMessage(validationContext.DisplayName));
                return ValidationSteps.Stop;
            }

            validationResult = ValidationResult.Success;
            return ValidationSteps.Stop;
        }


        #endregion
    }
}