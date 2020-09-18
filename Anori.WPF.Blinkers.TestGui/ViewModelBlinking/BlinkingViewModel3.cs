using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Timers;
using System.Windows.Media;
using JetBrains.Annotations;

namespace Anori.WPF.Blinkers.TestGui.ViewModelBlinking
{
    public class BlinkingCollectionViewModel3 : IDisposable

    {
        public ObservableCollection<BlinkingViewModel3> Items { get; } = new ObservableCollection<BlinkingViewModel3>();

        public void Dispose()
        {
            foreach (var blinkingViewModel in Items) blinkingViewModel.Dispose();
        }
    }

    public class BlinkingViewModel3 : INotifyPropertyChanged, IDisposable

    {
        private static readonly Timer Timer;
        private static readonly Color _brushOff;
        private static readonly Color _brushOn;
        private static bool _isBlinkingOn;

        private static SolidColorBrush brush;

        static BlinkingViewModel3()
        {
            Timer = new Timer {Interval = 500};
            Timer.Start();

            _brushOn = Colors.Red;
            _brushOff = Colors.Black;

            brush = new SolidColorBrush(_brushOff);
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

        private static void OnTick(object sender, EventArgs e)
        {
            try
            {
                _isBlinkingOn = !_isBlinkingOn;
                brush.Color = _isBlinkingOn ? _brushOn : _brushOff;
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception);
                throw;
            }
        }

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}