using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Media;
using System.Windows.Threading;
using JetBrains.Annotations;

namespace Anori.WPF.Blinkers.TestGui.ViewModelBlinking
{
    public class BlinkingCollectionViewModel : IDisposable

    {
        public ObservableCollection<BlinkingViewModel> Items { get; } = new ObservableCollection<BlinkingViewModel>();

        public void Dispose()
        {
            foreach (var blinkingViewModel in Items) blinkingViewModel.Dispose();
        }
    }

    public class BlinkingViewModel : INotifyPropertyChanged, IDisposable

    {
        private static readonly DispatcherTimer Timer;
        private readonly SolidColorBrush _brushOff;
        private readonly SolidColorBrush _brushOn;
        private bool _isBlinkingOn;

        private SolidColorBrush brush;

        static BlinkingViewModel()
        {
            Timer = new DispatcherTimer {Interval = TimeSpan.FromMilliseconds(500)};
            Timer.Start();
        }

        public BlinkingViewModel()
        {
            _brushOn = new SolidColorBrush(Colors.Red);
            _brushOff = new SolidColorBrush(Colors.Black);

            Brush = _brushOff;
            Timer.Tick += OnTick;
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
            if (Timer != null)
            {
                Timer.Stop();
                Timer.Tick -= OnTick;
            }
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