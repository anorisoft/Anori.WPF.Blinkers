// -----------------------------------------------------------------------
// <copyright file="BlinkingProvider.cs" company="bfa solutions ltd">
// Copyright (c) bfa solutions ltd. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Anori.WPF.Blinkers.Services
{
    using System;
    using System.ComponentModel;
    using System.Runtime.CompilerServices;
    using System.Windows.Media;
    using System.Windows.Threading;
    using JetBrains.Annotations;

    /// <summary>
    ///     The Blinking Provider class
    /// </summary>
    /// <seealso cref="IBlinkingProvider" />
    /// <seealso cref="INotifyPropertyChanged" />
    /// <seealso cref="IDisposable" />
    public class BlinkingProvider : INotifyPropertyChanged, IDisposable, IBlinkingProvider
    {
        /// <summary>
        ///     The default blinking interval time
        /// </summary>
        public const int DefaultBlinkingIntervalTime = 2500;

        /// <summary>
        ///     The blinking timer
        /// </summary>
        [NotNull]
        private readonly DispatcherTimer blinkingTimer;

        /// <summary>
        ///     The brush blinking client
        /// </summary>
        [NotNull]
        private readonly BrushBlinkingClient brushBlinkingClient;

        /// <summary>
        ///     The clients
        /// </summary>
        [NotNull]
        private readonly BlinkingClients clients = new BlinkingClients();

        /// <summary>
        ///     The opacity blinking client
        /// </summary>
        [NotNull]
        private readonly OpacityBlinkingClient opacityBlinkingClient;

        /// <summary>
        ///     The blinking beat
        /// </summary>
        private bool blinkingBeat;

        /// <summary>
        ///     The blinking interval time
        /// </summary>
        private int blinkingIntervalTime = DefaultBlinkingIntervalTime;

        /// <summary>
        ///     Initializes a new instance of the <see cref="BlinkingProvider" /> class.
        /// </summary>
        public BlinkingProvider()
        {
            brushBlinkingClient = new BrushBlinkingClient();
            brushBlinkingClient.PropertyChanged += OnBrushBlinkingClientPropertyChanged;
            brushBlinkingClient.Initialize();
            clients.Add(brushBlinkingClient);

            opacityBlinkingClient = new OpacityBlinkingClient();
            opacityBlinkingClient.PropertyChanged += OnOpacityBlinkingClientPropertyChanged;
            opacityBlinkingClient.Initialize();
            clients.Add(opacityBlinkingClient);

            blinkingTimer = new DispatcherTimer();
            blinkingTimer.Tick += OnBlinkingTick;
            blinkingTimer.Interval = new TimeSpan(0, 0, 0, 0, blinkingIntervalTime / 2);
            blinkingTimer.Start();
        }

        /// <summary>
        ///     Finalizes an instance of the <see cref="BlinkingProvider" /> class.
        /// </summary>
        ~BlinkingProvider()
        {
            Dispose();
        }

        /// <summary>
        ///     Occurs when a property value changes.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        ///     Gets a value indicating whether [blinking beat].
        /// </summary>
        /// <value>
        ///     <c>true</c> if [blinking beat]; otherwise, <c>false</c>.
        /// </value>
        public bool BlinkingBeat
        {
            get => blinkingBeat;
            private set
            {
                if (value == blinkingBeat)
                {
                    return;
                }

                blinkingBeat = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        ///     Gets the blinking brush.
        /// </summary>
        /// <value>
        ///     The blinking brush.
        /// </value>
        public Brush BlinkingBrush => brushBlinkingClient.BlinkingBrush;

        /// <summary>
        ///     Gets or sets the blinking interval time.
        /// </summary>
        /// <value>
        ///     The blinking interval time.
        /// </value>
        public int BlinkingIntervalTime
        {
            get => blinkingIntervalTime;
            set
            {
                if (value == blinkingIntervalTime)
                {
                    return;
                }

                blinkingIntervalTime = value;
                var span = new TimeSpan(0, 0, 0, 0, value / 2);
                blinkingTimer.Stop();
                blinkingTimer.Interval = span;
                blinkingTimer.Start();
                OnPropertyChanged();
            }
        }

        /// <summary>
        ///     Gets the opacity beat.
        /// </summary>
        /// <value>
        ///     The opacity beat.
        /// </value>
        public double OpacityBeat => opacityBlinkingClient.OpacityBeat;

        /// <summary>
        ///     Gets or sets the opacity frame time.
        /// </summary>
        /// <value>
        ///     The opacity frame time.
        /// </value>
        public int OpacityFrameTime
        {
            get => opacityBlinkingClient.OpacityFrameTime;
            set => opacityBlinkingClient.OpacityFrameTime = value;
        }

        /// <summary>
        ///     Gets or sets the opacity ramp time.
        /// </summary>
        /// <value>
        ///     The opacity ramp time.
        /// </value>
        public int OpacityRampTime
        {
            get => opacityBlinkingClient.OpacityRampTime;
            set => opacityBlinkingClient.OpacityRampTime = value;
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
            if (!disposing)
            {
                return;
            }

            blinkingTimer.Stop();
            blinkingTimer.Tick -= OnBlinkingTick;
            opacityBlinkingClient.PropertyChanged -= OnOpacityBlinkingClientPropertyChanged;
            brushBlinkingClient.PropertyChanged -= OnBrushBlinkingClientPropertyChanged;
            clients.Dispose();
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
        ///     Called when [blinking tick].
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        private void OnBlinkingTick(object sender, EventArgs e)
        {
            BlinkingBeat = !blinkingBeat;
            if (blinkingBeat)
            {
                clients.BlinkingOn();
            }
            else
            {
                clients.BlinkingOff();
            }
        }

        /// <summary>
        ///     Called when [brush blinking client property changed].
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="PropertyChangedEventArgs" /> instance containing the event data.</param>
        private void OnBrushBlinkingClientPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            OnPropertyChanged(e.PropertyName);
        }

        /// <summary>
        ///     Blinking client on property changed.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="PropertyChangedEventArgs" /> instance containing the event data.</param>
        private void OnOpacityBlinkingClientPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            OnPropertyChanged(e.PropertyName);
        }
    }
}