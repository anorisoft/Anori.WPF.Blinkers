// -----------------------------------------------------------------------
// <copyright file="BlinkingClients.cs" company="bfa solutions ltd">
// Copyright (c) bfa solutions ltd. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Anori.WPF.Blinkers.Services
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    ///     The Blinking Clients container
    /// </summary>
    /// <seealso cref="List{IBlinkingClient}" />
    /// <seealso cref="IDisposable" />
    public class BlinkingClients : List<IBlinkingClient>, IDisposable
    {
        /// <summary>
        ///     Blinking off.
        /// </summary>
        public void BlinkingOff()
        {
            foreach (var blinkingClient in this)
            {
                blinkingClient.BlinkingOff();
            }
        }

        /// <summary>
        ///     Blinking on.
        /// </summary>
        public void BlinkingOn()
        {
            foreach (var blinkingClient in this)
            {
                blinkingClient.BlinkingOn();
            }
        }

        /// <summary>
        ///     Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        ///     Releases unmanaged and - optionally - managed resources.
        /// </summary>
        /// <param name="disposing">
        ///     <c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only
        ///     unmanaged resources.
        /// </param>
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                foreach (var blinkingClient in this)
                {
                    blinkingClient.Dispose();
                }
            }
        }
    }
}