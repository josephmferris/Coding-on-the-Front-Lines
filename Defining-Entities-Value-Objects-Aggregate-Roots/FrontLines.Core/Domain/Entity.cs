namespace FrontLines.Core.Domain
{
    using System;

    /// <summary>
    /// When implemented, represents an Entity concept within the definition of Domain-Driven Design.
    /// </summary>
    public abstract class Entity
    {
        #region Public Properties

        /// <summary>
        /// Gets the entity identifier.
        /// </summary>
        /// <value>
        /// The identifier.
        /// </value>
        public Guid Id { get; private set; }

        #endregion

        #region Protected Methods

        /// <summary>
        /// Generates the identity for the <see cref="Entity" /> implementation.
        /// </summary>
        /// <exception cref="Exception">An identity for this Entity has already been established and cannot be changed.</exception>
        protected void GenerateIdentity()
        {
            if (this.Id == Guid.Empty)
            {
                this.Id = Guid.NewGuid();
            }

            throw new Exception("An identity for this Entity has already been established and cannot be changed.");
        }

        #endregion
    }
}