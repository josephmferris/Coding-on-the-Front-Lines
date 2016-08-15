namespace FrontLines.Core.Domain
{
    using System;
    using System.Collections.Generic;
    using System.Reflection;

    /// <summary>
    /// When implemented, represents a Value Object concept within the definition of Domain-Driven Design.
    /// </summary>
    /// <typeparam name="TValueObject">The type of the implementation, for equality comparison.</typeparam>
    /// <seealso cref="System.IEquatable{TValueObject}" />
    /// <remarks>
    /// This class is derived from Jimmy Bogard's implementation of ValueObject, as describer on his blog posting,
    /// entitled "Generic Value Object Equality".  At the time of this writing, the article is available at:
    /// https://lostechies.com/jimmybogard/2007/06/25/generic-value-object-equality/
    /// </remarks>
    public abstract class ValueObject<TValueObject> : IEquatable<TValueObject> where TValueObject : ValueObject<TValueObject>
    {
        #region Private Constant Declarations

        /// <summary>
        /// Provides a prime number which is used in incrementing a seeded hash code.
        /// </summary>
        private const int HashCodeMultiplier = 59;

        /// <summary>
        /// Provides a prime number which is used in the initial seeding of a hash code.
        /// </summary>
        private const int HashCodeSeedValue = 17;

        #endregion

        #region Operators

        /// <summary>
        /// Implements the operator ==.
        /// </summary>
        /// <param name="firstInstance">The first instance in the equality comparison.</param>
        /// <param name="secondInstance">The second instance in the equality comparison.</param>
        /// <returns>
        ///     <c>true</c> when both instances are equal; otherwise, <c>false</c>.
        /// </returns>
        public static bool operator ==(ValueObject<TValueObject> firstInstance, ValueObject<TValueObject> secondInstance)
        {
            if (ReferenceEquals(firstInstance, secondInstance))
            {
                return true;
            }

            if (((object)firstInstance == null) || ((object)secondInstance == null))
            {
                return false;
            }

            return firstInstance.Equals(secondInstance);
        }

        /// <summary>
        /// Implements the operator !=.
        /// </summary>
        /// <param name="firstInstance">The first instance in the equality comparison.</param>
        /// <param name="secondInstance">The second instance in the equality comparison.</param>
        /// <returns>
        ///     <c>true</c> when both instances are not equal; otherwise, <c>false</c>.
        /// </returns>
        public static bool operator !=(ValueObject<TValueObject> firstInstance, ValueObject<TValueObject> secondInstance)
        {
            return !(firstInstance == secondInstance);
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Indicates whether the current object is equal to another object of the same type.
        /// </summary>
        /// <param name="other">An object to compare with this object.</param>
        /// <returns>
        /// true if the current object is equal to the <paramref name="other" /> parameter; otherwise, false.
        /// </returns>
        public virtual bool Equals(TValueObject other)
        {
            if (other == null)
            {
                return false;
            }
            
            var instanceType = this.GetType();
            var otherType = other.GetType();

            if (instanceType != otherType)
            {
                return false;
            }

            var fields = instanceType.GetFields(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);
            
            foreach (var field in fields)
            {
                var otherValue = field.GetValue(other);
                var instanceValue = field.GetValue(this);
                
                if (otherValue == null)
                {
                    if (instanceValue != null)
                    {
                        return false;
                    }
                }
                else if (!otherValue.Equals(instanceValue))
                {
                    return false;
                }
            }
            
            return true;
        }

        #endregion

        #region Public Overrides

        /// <summary>
        /// Determines whether the specified <see cref="System.Object" />, is equal to this instance.
        /// </summary>
        /// <param name="target">The <see cref="System.Object" /> to compare with this instance.</param>
        /// <returns>
        ///   <c>true</c> if the specified <see cref="System.Object" /> is equal to this instance; otherwise, <c>false</c>.
        /// </returns>
        public override bool Equals(object target)
        {
            if (target == null)
            {
                return false;
            }

            var other = target as TValueObject;
            return this.Equals(other);
        }

        /// <summary>
        /// Returns a hash code for this instance.
        /// </summary>
        /// <returns>
        /// A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table. 
        /// </returns>
        public override int GetHashCode()
        {
            var fields = this.GetFields();
            var hashCode = HashCodeSeedValue;
            
            foreach (var field in fields)
            {
                var value = field.GetValue(this);
                
                if (value != null)
                {
                    hashCode = (hashCode * HashCodeMultiplier) + value.GetHashCode();
                }
            }

            return hashCode;
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Retrieves the fields for this instance, used for hash code calculation.
        /// </summary>
        /// <returns>A <see cref="IEnumerable{FieldInfo}" /> containing a traversal of contained fields.</returns>
        private IEnumerable<FieldInfo> GetFields()
        {
            var instanceType = this.GetType();
            var fields = new List<FieldInfo>();

            while (instanceType != typeof(object))
            {
                if (instanceType == null)
                {
                    break;
                }

                fields.AddRange(instanceType.GetFields(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public));
                instanceType = instanceType.BaseType;
            }
            
            return fields;
        }

        #endregion
    }
}
