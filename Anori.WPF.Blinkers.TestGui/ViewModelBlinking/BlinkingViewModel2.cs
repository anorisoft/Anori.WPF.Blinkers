using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Timers;
using System.Windows.Media;
using System.Windows.Threading;
using JetBrains.Annotations;

namespace Anori.WPF.Blinkers.TestGui.ViewModelBlinking
{
    public class BlinkingCollectionViewModel2 : IDisposable

    {
        public ObservableCollection<BlinkingViewModel2> Items { get; } = new ObservableCollection<BlinkingViewModel2>();

        public void Dispose()
        {
            foreach (var blinkingViewModel in Items)
            {
                blinkingViewModel.Dispose();
            }
        }
    }

    public class BlinkingViewModel2 : INotifyPropertyChanged, IDisposable

    {
        private static readonly Timer Timer;
        private readonly SolidColorBrush _brushOff;
        private readonly SolidColorBrush _brushOn;
        private bool _isBlinkingOn;

        private SolidColorBrush brush;

        static BlinkingViewModel2()
        {
            Timer = new Timer {Interval = 500};
            Timer.Start();
        }

        public BlinkingViewModel2()
        {
            _brushOn = new SolidColorBrush(Colors.Red);
            _brushOff = new SolidColorBrush(Colors.Black);

            Brush = _brushOff;
            Timer.Elapsed += OnTick;
        }

        public SolidColorBrush Brush
        {
            get => brush;
            private set
            {
                if (value.Equals(brush)) return;
                brush = value;
                OnPropertyChanged();
            }
        }

        public void Dispose()
        {
            if (Timer == null) return;
            Timer.Stop();
            Timer.Elapsed -= OnTick;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnTick(object sender, EventArgs e)
        {
            _isBlinkingOn = !_isBlinkingOn;
            Brush = _isBlinkingOn ? _brushOn : _brushOff;
        }

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}