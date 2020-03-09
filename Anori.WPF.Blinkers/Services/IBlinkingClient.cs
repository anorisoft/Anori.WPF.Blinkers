// -----------------------------------------------------------------------
// <copyright file="IBlinkingClient.cs" company="bfa solutions ltd">
// Copyright (c) bfa solutions ltd. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Anori.WPF.Blinkers.Services
{
    using System;
    using System.ComponentModel;

    /// <summary>
    ///     Blinking Client Interface
    /// </summary>
    public interface IBlinkingClient : IDisposable, IInitializable, INotifyPropertyChanged
    {
        /// <summary>
        ///     Blinking on.
        /// </summary>
        void BlinkingOn();

        /// <summary>
        ///     Blinking off.
        /// </summary>
        void BlinkingOff();
    }
}