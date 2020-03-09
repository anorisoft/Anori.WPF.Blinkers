// -----------------------------------------------------------------------
// <copyright file="OpacityChangedEventArgs.cs" company="bfa solutions ltd">
// Copyright (c) bfa solutions ltd. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Anori.WPF.Blinkers.Services
{
    using System;

    /// <summary>
    ///     OpacityChangedEventArgs
    /// </summary>
    /// <seealso cref="EventArgs" />
    public class OpacityChangedEventArgs : EventArgs
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="OpacityChangedEventArgs" /> class.
        /// </summary>
        /// <param name="opacity">The opacity.</param>
        public OpacityChangedEventArgs(double opacity)
        {
            Opacity = opacity;
        }

        /// <summary>
        ///     Gets the opacity.
        /// </summary>
        /// <value>
        ///     The opacity.
        /// </value>
        public double Opacity { get; }
    }
}