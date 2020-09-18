using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Timers;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Threading;
using JetBrains.Annotations;

namespace Anori.WPF.Blinkers.TestGui.ViewModelBlinking
{
    public class BlinkingCollectionViewModel5 : IDisposable

    {
        public ObservableCollection<BlinkingViewModel5> Items { get; } = new ObservableCollection<BlinkingViewModel5>();

        public void Dispose()
        {
            foreach (var blinkingViewModel in Items) blinkingViewModel.Dispose();
        }
    }

    public class BlinkingViewModel5 : INotifyPropertyChanged, IDisposable

    {
        public static Dictionary<Dispatcher, SolidColorBrush> BrushDictionary =
            new Dictionary<Dispatcher, SolidColorBrush>();

        private static readonly Color BrushOff;
        private static readonly Color BrushOn;
        private static readonly Timer Timer;
        private static bool _isBlinkingOn;

        private SolidColorBrush brush;
        private static ColorAnimation _onColorAnimation;
        private static ColorAnimation _offColorAnimation;

        static BlinkingViewModel5()
        {
            Timer = new Timer { Interval = 1000 };
            Timer.Start();

            BrushOn = Colors.Red;
            BrushOff = Colors.Black;

            Timer.Elapsed += OnTick;

            
        }

        public BlinkingViewModel5()
        {
            var dispatcher = Dispatcher.CurrentDispatcher;
            if (BrushDictionary.TryGetValue(dispatcher, out brush)) return;

            var duration = 500;
            _onColorAnimation = new ColorAnimation
            {
                To = BrushOn,
                Duration = new Duration(TimeSpan.FromMilliseconds(duration)),
                AutoReverse = false
            };

            _offColorAnimation = new ColorAnimation
            {
                To = BrushOff,
                Duration = new Duration(TimeSpan.FromMilliseconds(duration)),
                AutoReverse = false
            };
            brush = new SolidColorBrush();
            BrushDictionary.Add(dispatcher, brush);
        }

        public event PropertyChangedEventHandler PropertyChanged;

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
        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private static void OnTick(object sender, EventArgs e)
        {
            try
            {
                _isBlinkingOn = !_isBlinkingOn;
                foreach (var item in BrushDictionary)
                    item.Key.Invoke(() =>
                    {
                        var animation = _isBlinkingOn ? _onColorAnimation : _offColorAnimation;
                        item.Value.BeginAnimation(SolidColorBrush.ColorProperty, animation); 
                    });
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception);
                throw;
            }
        }
    }
}