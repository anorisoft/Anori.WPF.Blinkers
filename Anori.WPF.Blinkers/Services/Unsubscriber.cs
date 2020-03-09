// -----------------------------------------------------------------------
// <copyright file="Unsubscriber.cs" company="bfa solutions ltd">
// Copyright (c) bfa solutions ltd. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Anori.WPF.Blinkers.Services
{
    using System;

    using JetBrains.Annotations;

    /// <summary>
    /// Unsubscriber class
    /// </summary>
    /// <seealso cref="IDisposable" />
    public class Unsubscriber : IDisposable
    {
        /// <summary>
        /// The action
        /// </summary>
        private readonly Action action;

        /// <summary>
        /// Initializes a new instance of the <see cref="Unsubscriber"/> class.
        /// </summary>
        /// <param name="action">The action.</param>
        public Unsubscriber([NotNull] Action action)
        {
            this.action = action ?? throw new ArgumentNullException(nameof(action));
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            action();
        }
    }
}