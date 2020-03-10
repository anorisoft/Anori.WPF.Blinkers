namespace Anori.WPF.Blinkers
{
    using Anori.WPF.Blinkers.Services;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Media;

    /// <summary>
    ///     The Led Control class
    /// </summary>
    /// <seealso cref="CheckBox" />
    [TemplateVisualState(GroupName = BlinkingStates.GroupName, Name = BlinkingStates.BlinkingState)]
    [TemplateVisualState(GroupName = BlinkingStates.GroupName, Name = BlinkingStates.NotBlinkingState)]
    public class LedControl : CheckBox
    {
        /// <summary>
        ///     The on color property
        /// </summary>
        public static readonly DependencyProperty OnColorProperty = DependencyProperty.Register(
            nameof(OnColor),
            typeof(Color),
            typeof(LedControl),
            new PropertyMetadata(Colors.Green));

        /// <summary>
        ///     The off color property
        /// </summary>
        public static readonly DependencyProperty OffColorProperty = DependencyProperty.Register(
            nameof(OffColor),
            typeof(Color),
            typeof(LedControl),
            new PropertyMetadata(Colors.Red));

        /// <summary>
        ///     The blinking color property
        /// </summary>
        public static readonly DependencyProperty BlinkingColorProperty = DependencyProperty.Register(
            nameof(BlinkingColor),
            typeof(Color),
            typeof(LedControl),
            new PropertyMetadata(Colors.Yellow, PropertyChangedCallback));

        private static void PropertyChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
          var provider = new BrushesBlinkingProvider(((Color)e.NewValue));
          BlinkingService.AddProvider(((Color)e.NewValue).ToString(), provider);
          ((LedControl)d).BlinkingProvider = provider;
        }

        /// <summary>
        ///     The is blinking property
        /// </summary>
        public static readonly DependencyProperty IsBlinkingProperty = DependencyProperty.Register(
            nameof(IsBlinking),
            typeof(bool),
            typeof(LedControl),
            new PropertyMetadata(false, OnIsBlinkingChanged));

        /// <summary>
        ///     The blinking provider.
        /// </summary>
        public static readonly DependencyProperty BlinkingProviderProperty = DependencyProperty.Register(
            nameof(BlinkingProvider),
            typeof(IBlinkingProvider),
            typeof(LedControl),
            new PropertyMetadata(BlinkingService.DefaultProvider));

        /// <summary>
        ///     Initializes static members of the <see cref="LedControl" /> class.
        /// </summary>
        static LedControl()
        {
            DefaultStyleKeyProperty.OverrideMetadata(
                typeof(LedControl),
                new FrameworkPropertyMetadata(typeof(LedControl)));
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="LedControl" /> class.
        /// </summary>
        public LedControl()
        {
            Loaded += OnLoaded;
        }

        /// <summary>
        ///     Gets or sets the color of the blinking.
        /// </summary>
        /// <value>
        ///     The color of the blinking.
        /// </value>
        public Color BlinkingColor
        {
            get => (Color)GetValue(BlinkingColorProperty);

            set => SetValue(BlinkingColorProperty, value);
        }

        /// <summary>
        ///     Gets or sets the color of the on.
        /// </summary>
        /// <value>
        ///     The color of the on.
        /// </value>
        public Color OnColor
        {
            get => (Color)GetValue(OnColorProperty);

            set => SetValue(OnColorProperty, value);
        }

        /// <summary>
        ///     Gets or sets the color of the off.
        /// </summary>
        /// <value>
        ///     The color of the off.
        /// </value>
        public Color OffColor
        {
            get => (Color)GetValue(OffColorProperty);

            set => SetValue(OffColorProperty, value);
        }

        /// <summary>
        ///     Gets or sets a value indicating whether this instance is blinking.
        /// </summary>
        /// <value>
        ///     <c>true</c> if this instance is blinking; otherwise, <c>false</c>.
        /// </value>
        public bool IsBlinking
        {
            get => (bool)GetValue(IsBlinkingProperty);

            set => SetValue(IsBlinkingProperty, value);
        }

        /// <summary>
        ///     Gets or sets the blinking provider.
        /// </summary>
        /// <value>
        ///     The blinking provider.
        /// </value>
        public IBlinkingProvider BlinkingProvider
        {
            get => (IBlinkingProvider)GetValue(BlinkingProviderProperty);

            set => SetValue(BlinkingProviderProperty, value);
        }

        /// <summary>
        ///     Called when [is blinking changed].
        /// </summary>
        /// <param name="d">The d.</param>
        /// <param name="e">The <see cref="DependencyPropertyChangedEventArgs" /> instance containing the event data.</param>
        private static void OnIsBlinkingChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if ((bool)e.NewValue)
            {
                VisualStateManager.GoToState((LedControl)d, BlinkingStates.BlinkingState, true);
            }
            else
            {
                VisualStateManager.GoToState((LedControl)d, BlinkingStates.NotBlinkingState, true);
            }
        }

        /// <summary>
        ///     Called when [loaded].
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="RoutedEventArgs" /> instance containing the event data.</param>
        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            Loaded -= OnLoaded;
            VisualStateManager.GoToState(
                this,
                IsBlinking ? BlinkingStates.BlinkingState : BlinkingStates.NotBlinkingState,
                true);
        }
    }
}