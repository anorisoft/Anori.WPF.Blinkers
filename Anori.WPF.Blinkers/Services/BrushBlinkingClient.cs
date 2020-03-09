// -----------------------------------------------------------------------
// <copyright file="BrushBlinkingClient.cs" company="bfa solutions ltd">
// Copyright (c) bfa solutions ltd. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Anori.WPF.Blinkers.Services
{
    using System;
    using System.ComponentModel;
    using System.Runtime.CompilerServices;
    using System.Windows;
    using System.Windows.Media;
    using System.Windows.Media.Animation;
    using JetBrains.Annotations;

    /// <summary>
    ///     Brush Blinking Client class.
    /// </summary>
    /// <seealso cref="IBlinkingClient" />
    public class BrushBlinkingClient : IBlinkingClient
    {
        /// <summary>
        ///     The default ramp time
        /// </summary>
        public const int DefaultRampTime = 700;

        /// <summary>
        ///     The blinking brush
        /// </summary>
        private SolidColorBrush blinkingBrush;

        /// <summary>
        ///     The color
        /// </summary>
        private Color color;

        /// <summary>
        ///     The off color animation
        /// </summary>
        private ColorAnimation offColorAnimation;

        /// <summary>
        ///     The on color animation
        /// </summary>
        private ColorAnimation onColorAnimation;

        /// <summary>
        ///     The ramp time
        /// </summary>
        private int rampTime;

        /// <summary>
        ///     Initializes a new instance of the <see cref="BrushBlinkingClient" /> class.
        /// </summary>
        public BrushBlinkingClient()
        {
            BlinkingBrush = new SolidColorBrush { Color = Colors.Transparent };
            rampTime = DefaultRampTime;
            color = DefaultColor;
            UpdateAnimations();
        }

        /// <summary>
        ///     Occurs when a property value changes.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        ///     Gets the blinking brush.
        /// </summary>
        /// <value>
        ///     The blinking brush.
        /// </value>
        public SolidColorBrush BlinkingBrush
        {
            get => blinkingBrush;
            private set
            {
                if (Equals(value, blinkingBrush))
                {
                    return;
                }

                blinkingBrush = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        ///     Gets or sets the color.
        /// </summary>
        /// <value>
        ///     The color.
        /// </value>
        public Color Color
        {
            get => color;
            set
            {
                if (color == value)
                {
                    return;
                }

                color = value;
                UpdateAnimations();
                OnPropertyChanged();
            }
        }

        /// <summary>
        ///     Gets the default color.
        /// </summary>
        /// <value>
        ///     The default color.
        /// </value>
        public Color DefaultColor { get; } = Colors.Yellow;

        /// <summary>
        ///     Gets or sets the ramp time.
        /// </summary>
        /// <value>
        ///     The ramp time.
        /// </value>
        public int RampTime
        {
            get => rampTime;
            set
            {
                if (rampTime == value)
                {
                    return;
                }

                rampTime = value;
                UpdateAnimations();
                OnPropertyChanged();
            }
        }

        /// <summary>
        ///     Blinking off.
        /// </summary>
        public void BlinkingOff()
        {
            var animation = offColorAnimation;
            BlinkingBrush.BeginAnimation(SolidColorBrush.ColorProperty, animation);
        }

        /// <summary>
        ///     Blinking on.
        /// </summary>
        public void BlinkingOn()
        {
            var animation = onColorAnimation;
            BlinkingBrush.BeginAnimation(SolidColorBrush.ColorProperty, animation);
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
        ///     Initializes this instance.
        /// </summary>
        public void Initialize()
        {
            UpdateAnimations();
            BlinkingBrush.BeginAnimation(SolidColorBrush.ColorProperty, offColorAnimation);
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
        }

        /// <summary>
        ///     Called when [property changed].
        /// </summary>
        /// <param name="propertyName">Name of the property.</param>
        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        /// <summary>
        ///     Updates the animations.
        /// </summary>
        private void UpdateAnimations()
        {
            var toColor = Color;
            var duration = RampTime;
            onColorAnimation = new ColorAnimation
            {
                To = toColor,
                Duration = new Duration(TimeSpan.FromMilliseconds(duration)),
                AutoReverse = false
            };

            offColorAnimation = new ColorAnimation
            {
                To = Color.FromArgb(0, toColor.R, toColor.G, toColor.B),
                Duration = new Duration(TimeSpan.FromMilliseconds(duration)),
                AutoReverse = false
            };
        }
    }
}