// -----------------------------------------------------------------------
// <copyright file="IBlinkingProvider.cs" company="bfa solutions ltd">
// Copyright (c) bfa solutions ltd. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Anori.WPF.Blinkers.Services
{
    using System.Windows.Media;

    /// <summary>
    ///     Blinking provider interface.
    /// </summary>
    public interface IBrushesBlinkingProvider : IBlinkingProvider
    {
        /// <summary>
        ///     Gets the blinking brush.
        /// </summary>
        /// <value>
        ///     The blinking brush.
        /// </value>
        Brush BlinkingBrush { get; }
    }

    

    public interface IOpacityBlinkingProvider : IBlinkingProvider
    {  /// <summary>
        ///     Gets the opacity beat.
        /// </summary>
        /// <value>
        ///     The opacity beat.
        /// </value>
        double OpacityBeat { get; }

        /// <summary>
        ///     Gets or sets the opacity frame time.
        /// </summary>
        /// <value>
        ///     The opacity frame time.
        /// </value>
        int OpacityFrameTime { get; set; }

        /// <summary>
        ///     Gets or sets the opacity ramp time.
        /// </summary>
        /// <value>
        ///     The opacity ramp time.
        /// </value>
        int OpacityRampTime { get; set; }
    }

    public interface IBlinkingProvider
        {
            /// <summary>
            ///     Gets a value indicating whether [blinking beat].
            /// </summary>
            /// <value>
            ///     <c>true</c> if [blinking beat]; otherwise, <c>false</c>.
            /// </value>
            bool BlinkingBeat { get; }

      

        /// <summary>
        ///     Gets or sets the blinking interval time.
        /// </summary>
        /// <value>
        ///     The blinking interval time.
        /// </value>
        int BlinkingIntervalTime { get; set; }

       


    }
}