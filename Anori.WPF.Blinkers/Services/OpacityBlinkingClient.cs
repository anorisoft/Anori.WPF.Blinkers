// -----------------------------------------------------------------------
// <copyright file="OpacityBlinkingClient.cs" company="bfa solutions ltd">
// Copyright (c) bfa solutions ltd. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Anori.WPF.Blinkers.Services
{
    using System;
    using System.ComponentModel;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;
    using System.Windows.Threading;
    using JetBrains.Annotations;

    /// <summary>
    ///     Brush Blinking Client class.
    /// </summary>
    /// <seealso cref="IBlinkingClient" />
    public class OpacityBlinkingClient : IBlinkingClient, IObservable<double>
    {
        /// <summary>
        ///     The default opacity frame time
        /// </summary>
        public const int DefaultOpacityFrameTime = 40;

        /// <summary>
        ///     The default opacity ramp time
        /// </summary>
        public const int DefaultOpacityRampTime = 700;

        /// <summary>
        ///     The isBlinking stop watch
        /// </summary>
        [NotNull]
        private readonly Stopwatch blinkingStopWatch;

        /// <summary>
        ///     The opacity lock
        /// </summary>
        [NotNull]
        private readonly object opacityLock = new object();

        /// <summary>
        ///     The opacity timer
        /// </summary>
        [NotNull]
        private readonly DispatcherTimer opacityTimer;

        /// <summary>
        ///     The isBlinking
        /// </summary>
        private bool isBlinking;

        /// <summary>
        ///     The opacity beat
        /// </summary>
        private double opacityBeat;

        /// <summary>
        ///     The opacity frame time
        /// </summary>
        private int opacityFrameTime = DefaultOpacityFrameTime;

        /// <summary>
        ///     The opacity ramp time
        /// </summary>
        private int opacityRampTime = DefaultOpacityRampTime;

        /// <summary>
        ///     The opacity step
        /// </summary>
        private double opacityStep;

        /// <summary>
        ///     The opacity time spam
        /// </summary>
        private TimeSpan opacityTimeSpan = TimeSpan.FromMilliseconds(DefaultOpacityRampTime);

        /// <summary>
        ///     Initializes a new instance of the <see cref="OpacityBlinkingClient" /> class.
        /// </summary>
        public OpacityBlinkingClient()
        {
            blinkingStopWatch = new Stopwatch();

            opacityTimer = new DispatcherTimer();
        }

        /// <summary>
        ///     Occurs when [opacity changed].
        /// </summary>
        public event EventHandler<OpacityChangedEventArgs> OpacityChanged;

        /// <summary>
        ///     Occurs when a property value changes.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        ///     Gets the opacity beat.
        /// </summary>
        /// <value>
        ///     The opacity beat.
        /// </value>
        public double OpacityBeat
        {
            get => opacityBeat;
            private set
            {
                if (value.Equals(opacityBeat))
                {
                    return;
                }

                opacityBeat = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        ///     Gets or sets the opacity frame time.
        /// </summary>
        /// <value>
        ///     The opacity frame time.
        /// </value>
        public int OpacityFrameTime
        {
            get => opacityFrameTime;
            set
            {
                if (value == opacityFrameTime)
                {
                    return;
                }

                opacityFrameTime = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        ///     Gets or sets the opacity ramp time.
        /// </summary>
        /// <value>
        ///     The opacity ramp time.
        /// </value>
        public int OpacityRampTime
        {
            get => opacityRampTime;
            set
            {
                if (value == opacityRampTime)
                {
                    return;
                }

                opacityRampTime = value;
                opacityTimeSpan = TimeSpan.FromMilliseconds(value);
                opacityStep = 1 / opacityTimeSpan.TotalMilliseconds;
                OnPropertyChanged();
            }
        }

        /// <summary>
        ///     Blinking off.
        /// </summary>
        public void BlinkingOff()
        {
            SetupTransition(false);
        }

        /// <summary>
        ///     Blinking on.
        /// </summary>
        public void BlinkingOn()
        {
            SetupTransition(true);
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
            opacityTimer.Tick += OnOpacityTick;
            opacityTimer.Interval = new TimeSpan(0, 0, 0, 0, opacityFrameTime);

            opacityStep = 1 / opacityTimeSpan.TotalMilliseconds;
        }

        /// <summary>
        ///     Notifies the provider that an observer is to receive notifications.
        /// </summary>
        /// <param name="observer">The object that is to receive notifications.</param>
        /// <returns>
        ///     A reference to an interface that allows observers to stop receiving notifications before the provider has finished
        ///     sending them.
        /// </returns>
        public IDisposable Subscribe(IObserver<double> observer)
        {
            void OnOpacityChanged(object sender, OpacityChangedEventArgs args) => observer.OnNext(args.Opacity);
            OpacityChanged += OnOpacityChanged;
            return new Unsubscriber(
                () =>
                    {
                        OpacityChanged -= OnOpacityChanged;
                        observer.OnCompleted();
                    });
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
                opacityTimer.Stop();
                blinkingStopWatch.Stop();
                opacityTimer.Tick -= OnOpacityTick;
            }
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
        ///     Called when [opacity tick].
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        private void OnOpacityTick(object sender, EventArgs e)
        {
            lock (opacityLock)
            {
                var milliseconds = blinkingStopWatch.ElapsedMilliseconds;
                if (milliseconds >= opacityTimeSpan.TotalMilliseconds)
                {
                    opacityTimer.Stop();
                    blinkingStopWatch.Stop();
                    blinkingStopWatch.Reset();
                    SetOpacityBeat(isBlinking ? 1 : 0);
                }
                else
                {
                    if (isBlinking)
                    {
                        SetOpacityBeat(milliseconds * opacityStep);
                    }
                    else
                    {
                        SetOpacityBeat(1 - milliseconds * opacityStep);
                    }
                }
            }
        }

        /// <summary>
        ///     Sets the opacity beat.
        /// </summary>
        /// <param name="opacity">The opacity.</param>
        private void SetOpacityBeat(double opacity)
        {
            OpacityBeat = opacity;
            OpacityChanged?.Invoke(this, new OpacityChangedEventArgs(opacity));
        }

        /// <summary>
        ///     Setups the transition.
        /// </summary>
        /// <param name="blinking">if set to <c>true</c> [isBlinking].</param>
        private void SetupTransition(bool blinking)
        {
            opacityTimer.Stop();
            blinkingStopWatch.Stop();
            opacityTimer.Interval = new TimeSpan(0, 0, 0, 0, opacityFrameTime);
            isBlinking = blinking;
            opacityTimer.Start();
            blinkingStopWatch.Start();
        }
    }
}