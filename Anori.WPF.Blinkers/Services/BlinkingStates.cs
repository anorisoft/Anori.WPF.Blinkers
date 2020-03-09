// -----------------------------------------------------------------------
// <copyright file="BlinkingStates.cs" company="bfa solutions ltd">
// Copyright (c) bfa solutions ltd. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Anori.WPF.Blinkers.Services
{
    using JetBrains.Annotations;

    /// <summary>
    ///     The Blinking States class.
    /// </summary>
    public static class BlinkingStates
    {
        /// <summary>
        ///     The group name
        /// </summary>
        [NotNull]
        public const string GroupName = "BlinkingStates";

        /// <summary>
        ///     The blinking state
        /// </summary>
        [NotNull]
        public const string BlinkingState = "Blinking";

        /// <summary>
        ///     The not blinking state
        /// </summary>
        [NotNull]
        public const string NotBlinkingState = "NotBlinking";
    }
}