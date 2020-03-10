using System;
using System.Collections.Generic;
using System.Windows.Media;

namespace Anori.WPF.Blinkers.Services
{
    public class BrushBlinkingClients : Dictionary<Color, IBlinkingClient>, IDisposable
    {
        /// <summary>
        ///     Blinking off.
        /// </summary>
        public void BlinkingOff()
        {
            foreach (var blinkingClient in this)
            {
                blinkingClient.Value.BlinkingOff();
            }
        }

        /// <summary>
        ///     Blinking on.
        /// </summary>
        public void BlinkingOn()
        {
            foreach (var blinkingClient in this)
            {
                blinkingClient.Value.BlinkingOn();
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
                    blinkingClient.Value.Dispose();
                }
            }
        }
    }
}