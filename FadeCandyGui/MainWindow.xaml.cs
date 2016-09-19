using System;
using System.Windows;
using System.Windows.Controls.Primitives;
using System.Windows.Media.Animation;
using System.Windows.Media.Effects;
using OpenPixelControl;

namespace FadeCandyGui
{
    /// <summary>
    ///     Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private double _offDuration;
        private double _onDuration;
        private OpcClient _opcClient;
        private int _port;
        private string _server;

        private bool _prevButtonState = false;


        private const double LightToggleAnimLength = 1.8;

        private const double LedOnOpacity = 0.88;
        private const double LedOffOpacity = 0.35;
        private const double LogoOnOpacity = 1.00;
        private const double LogoOffOpacity = 0.40;

        public MainWindow()
        {
            InitializeComponent();

            // set lighting effects to off
            ConnectionStatusLedImage.Opacity = LedOffOpacity;
            LogoImage.Opacity = LogoOffOpacity;
            LogoImageBlurLayer.Opacity = LogoOffOpacity;

            //_opcClient = new OpcClient();
        }

        private void SendMessageButton_Click(object sender, RoutedEventArgs e)
        {
            //convert string chars to light IDs
            //make commands
            //send commands 
        }

        private void OnDurationSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            _onDuration = e.NewValue;
        }

        private void OffDurationSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            _offDuration = e.NewValue;
        }

        private void ConnectButton_Click(object sender, RoutedEventArgs e)
        {
            // if prev was on
            if (_prevButtonState)
            {
                // turn off

                //clear previous effects
                ConnectionStatusLedImage.ClearValue(EffectProperty);
                LogoImageBlurLayer.ClearValue(EffectProperty);

                //create new effects
                var ledBlurEffect = new BlurEffect();
                var logoBlurEffect = new BlurEffect();

                //assign effects
                ConnectionStatusLedImage.Effect = ledBlurEffect;
                LogoImageBlurLayer.Effect = logoBlurEffect;

                //create animations
                var ledBlurRadiusAnimation = new DoubleAnimation(5, 0, TimeSpan.FromSeconds(LightToggleAnimLength));
                var logoImageBlurRadiusAnimation = new DoubleAnimation(20, 0, TimeSpan.FromSeconds(LightToggleAnimLength));
                var ledOpacityAnimation = new DoubleAnimation(LedOnOpacity, LedOffOpacity, TimeSpan.FromSeconds(LightToggleAnimLength));
                var logoOpacityAnimation = new DoubleAnimation(LogoOnOpacity, LogoOffOpacity, TimeSpan.FromSeconds(LightToggleAnimLength));

                //start animations
                ledBlurEffect.BeginAnimation(BlurEffect.RadiusProperty, ledBlurRadiusAnimation);
                logoBlurEffect.BeginAnimation(BlurEffect.RadiusProperty, logoImageBlurRadiusAnimation);
                ConnectionStatusLedImage.BeginAnimation(OpacityProperty, ledOpacityAnimation);
                LogoImageBlurLayer.BeginAnimation(OpacityProperty, logoOpacityAnimation);
                LogoImage.BeginAnimation(OpacityProperty, logoOpacityAnimation);
            }
            else
            {
                //else turn on

                //clear previous effects
                ConnectionStatusLedImage.ClearValue(EffectProperty);
                LogoImageBlurLayer.ClearValue(EffectProperty);

                //create new effects
                var ledBlurEffect = new BlurEffect();
                var logoBlurEffect = new BlurEffect();

                //assign effects
                ConnectionStatusLedImage.Effect = ledBlurEffect;
                LogoImageBlurLayer.Effect = logoBlurEffect;

                //create animations
                var ledBlurRadiusAnimation = new DoubleAnimation(0, 5, TimeSpan.FromSeconds(LightToggleAnimLength));
                var logoImageBlurRadiusAnimation = new DoubleAnimation(0, 20, TimeSpan.FromSeconds(LightToggleAnimLength));
                var ledOpacityAnimation = new DoubleAnimation(LedOffOpacity, LedOnOpacity, TimeSpan.FromSeconds(LightToggleAnimLength));
                var logoOpacityAnimation = new DoubleAnimation(LogoOffOpacity, LogoOnOpacity, TimeSpan.FromSeconds(LightToggleAnimLength));

                //start animations
                ledBlurEffect.BeginAnimation(BlurEffect.RadiusProperty, ledBlurRadiusAnimation);
                logoBlurEffect.BeginAnimation(BlurEffect.RadiusProperty, logoImageBlurRadiusAnimation);
                ConnectionStatusLedImage.BeginAnimation(OpacityProperty, ledOpacityAnimation);
                LogoImageBlurLayer.BeginAnimation(OpacityProperty, logoOpacityAnimation);
                LogoImage.BeginAnimation(OpacityProperty, logoOpacityAnimation);
            }

            //toggle button state
            _prevButtonState = !_prevButtonState;
        }

    }
}