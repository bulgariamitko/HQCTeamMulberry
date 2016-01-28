// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Type.cs" company="">
//   
// </copyright>
// <summary>
//   Defines the Type type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Poker
{
    using Validators;

    /// <summary>
    /// The type.
    /// </summary>
    public class Type
    {
        /// <summary>
        /// The power.
        /// </summary>
        private double power;

        /// <summary>
        /// The current.
        /// </summary>
        private double current;

        /// <summary>
        /// Initializes a new instance of the <see cref="Type"/> class.
        /// </summary>
        public Type()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Type"/> class.
        /// </summary>
        /// <param name="power">
        /// The power.
        /// </param>
        /// <param name="current">
        /// The current.
        /// </param>
        public Type(double power, double current)
        {
            this.Power = power;
            this.Current = current;
        }

        /// <summary>
        /// Gets or sets the power.
        /// </summary>
        public double Power
        {
            get
            {
                return this.power;
            }

            set
            {
                PropertyValueValidator.ValidateForNull(value, "Power");
                this.power = value;
            }
        }

        /// <summary>
        /// Gets or sets the current.
        /// </summary>
        public double Current
        {
            get
            {
                return this.current;
            }

            set
            {
                PropertyValueValidator.ValidateForNull(value, "Current");
                PropertyValueValidator.ValidateForNegativeDoubleNumber(value, "Current");
                this.current = value;
            }
        }
    }
}
