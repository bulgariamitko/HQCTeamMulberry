// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PropertyValueValidator.cs" company="">
//   
// </copyright>
// <summary>
//   Static class who validate property value
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Poker.Validators
{
    using System;

    /// <summary>
    /// Static class who validate property value
    /// </summary>
    public static class PropertyValueValidator
    {
        /// <summary>
        /// Validate for null.
        /// </summary>
        /// <param name="value">
        /// The value.
        /// </param>
        /// <param name="propertyName">
        /// The property name.
        /// </param>
        /// <typeparam name="T">
        /// </typeparam>
        /// <exception cref="ArgumentNullException">
        /// </exception>
        public static void ValidateForNull<T>(T value, string propertyName)
        {
            if (value == null)
            {
                var msg = string.Format("{0} cannot be null.", propertyName);
                throw new ArgumentNullException(msg);
            }
        }

        /// <summary>
        /// The validate for negative number.
        /// </summary>
        /// <param name="value">
        /// The value.
        /// </param>
        /// <param name="propertyName">
        /// The property name.
        /// </param>
        /// <exception cref="ArgumentOutOfRangeException">
        /// </exception>
        public static void ValidateForNegativeNumber(int value, string propertyName)
        {
            if (value < 0)
            {
                var msg = string.Format("{0} cannot be negative.", propertyName);
                throw new ArgumentOutOfRangeException(msg);
            }
        }

        /// <summary>
        /// The validate for empty or null string.
        /// </summary>
        /// <param name="value">
        /// The value.
        /// </param>
        /// <param name="propertyName">
        /// The property name.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// </exception>
        public static void ValidateForEmptyOrNullString(string value, string propertyName)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                var msg = string.Format("{0} cannot be null or white space.", propertyName);
                throw new ArgumentNullException(msg);
            }
        }
    }
}
