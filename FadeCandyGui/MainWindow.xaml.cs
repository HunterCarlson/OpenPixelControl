using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
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
        private const double LightToggleAnimLength = 1.8;

        private const double LedOnOpacity = 0.88;
        private const double LedOffOpacity = 0.35;
        private const double LogoOnOpacity = 1.00;
        private const double LogoOffOpacity = 0.40;


        private const double LedBlurRadius = 5;
        private const double LogoBlurRadius = 20;

        private readonly BlurEffect _ledBlurEffect;

        private readonly LetterWall _letterWall;
        private readonly BlurEffect _logoBlurEffect;
        private readonly OpcClient _opcClient;
        private CancellationTokenSource _cancellationTokenSource;
        private double _offDuration;
        private double _onDuration;
        private int _port;

        private bool _prevConnectionState;
        private string _server;

        private readonly LetterWallViewModel model;

        public MainWindow()
        {
            InitializeComponent();

            //get the model from the xaml
            model = DataContext as LetterWallViewModel;

            //disable message entry until connected
            MessageTextBox.IsEnabled = false;
            SendMessageButton.IsEnabled = false;
            StopButton.IsEnabled = false;

            //set opacity on lights to off value
            ConnectionStatusLedImage.Opacity = LedOffOpacity;
            LogoImage.Opacity = LogoOffOpacity;
            LogoImageBlurLayer.Opacity = LogoOffOpacity;

            //init effects to 0 - will animate later
            _ledBlurEffect = new BlurEffect {Radius = 0};
            _logoBlurEffect = new BlurEffect {Radius = 0};
            //assign effects
            ConnectionStatusLedImage.Effect = _ledBlurEffect;
            LogoImageBlurLayer.Effect = _logoBlurEffect;

            _opcClient = new OpcClient();

            _letterWall = new LetterWall();
        }

        private void SendMessageButton_Click(object sender, RoutedEventArgs e)
        {
            //convert string chars to light IDs
            //make commands
            //send commands 

            //do in new thread to not block ui
            //https://blogs.msdn.microsoft.com/csharpfaq/2010/07/19/parallel-programming-task-cancellation/
            //http://blog.stephencleary.com/2012/02/async-and-await.html

            //get UI values outside of task
            var message = model.Message;
            var onDuration = model.OnDuration*1000;
            var offDuration = model.OffDuration*1000;

            _cancellationTokenSource = new CancellationTokenSource();
            var token = _cancellationTokenSource.Token;

            Task.Run(async () =>
            {
                foreach (var c in message.ToUpper())
                {
                    var frame = _letterWall.CreateLetterFrame(c);
                    _opcClient.WriteFrame(frame);
                    await Task.Delay((int) onDuration, token);
                    _opcClient.TurnOffAllPixels();
                    await Task.Delay((int) offDuration, token);
                }
            }, token);
        }

        private void ConnectButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (_prevConnectionState)
                {
                    //disconnect
                    //port actuall disconnects automatically after each message sent
                    //just update UI
                }
                else
                {
                    //connect
                    //port doenst connect until messages are sent
                    //try to blink the led to see if valid port

                    //is there a tcp response message I can check?

                    _opcClient.Server = model.OpcServer;
                    _opcClient.Port = Convert.ToInt32(model.OpcPort);

                    // blink leds on connect
                    // use Task to not block UI

                    Task.Run(async () =>
                    {
                        _opcClient.SetDitheringAndInterpolation(true);

                        var animation = new FrameAnimation();

                        var onFramePixels = _opcClient.SingleColorFrame(200, 0, 0);
                        var onFrame = new Frame(onFramePixels, 500);

                        var offFramePixels = _opcClient.SingleColorFrame(OpcConstants.DarkPixel);
                        var offFrame = new Frame(offFramePixels, 500);

                        //fade red in and out 3 times
                        animation.Add(offFrame);
                        animation.Add(onFrame);
                        animation.Add(offFrame);
                        animation.Add(onFrame);
                        animation.Add(offFrame);
                        animation.Add(onFrame);
                        animation.Add(offFrame);

                        await _opcClient.PlayAnimation(animation);

                        _opcClient.SetDitheringAndInterpolation(false);
                    });
                }

                //update UI
                UpdateUiForConnectButtonClick();
                //toggle connection state
                _prevConnectionState = !_prevConnectionState;
            }
            catch (Exception exception)
            {
                Debug.WriteLine(exception);
            }
        }

        private void UpdateUiForConnectButtonClick()
        {
            //if prev was on
            if (_prevConnectionState)
            {
                //turn off

                //update button text
                ConnectButton.Content = "Connect";

                //disable message entry
                MessageTextBox.IsEnabled = false;
                SendMessageButton.IsEnabled = false;
                StopButton.IsEnabled = false;

                //disable server and port text boxes
                ServerTextBox.IsEnabled = true;
                PortTextBox.IsEnabled = true;

                //create animations
                var ledBlurRadiusAnimation = new DoubleAnimation(LedBlurRadius, 0,
                    TimeSpan.FromSeconds(LightToggleAnimLength));
                var logoImageBlurRadiusAnimation = new DoubleAnimation(LogoBlurRadius, 0,
                    TimeSpan.FromSeconds(LightToggleAnimLength));
                var ledOpacityAnimation = new DoubleAnimation(LedOnOpacity, LedOffOpacity,
                    TimeSpan.FromSeconds(LightToggleAnimLength));
                var logoOpacityAnimation = new DoubleAnimation(LogoOnOpacity, LogoOffOpacity,
                    TimeSpan.FromSeconds(LightToggleAnimLength));

                //start animations
                _ledBlurEffect.BeginAnimation(BlurEffect.RadiusProperty, ledBlurRadiusAnimation);
                _logoBlurEffect.BeginAnimation(BlurEffect.RadiusProperty, logoImageBlurRadiusAnimation);
                ConnectionStatusLedImage.BeginAnimation(OpacityProperty, ledOpacityAnimation);
                LogoImageBlurLayer.BeginAnimation(OpacityProperty, logoOpacityAnimation);
                LogoImage.BeginAnimation(OpacityProperty, logoOpacityAnimation);
            }
            else
            {
                //else turn on

                // update button text
                ConnectButton.Content = "Disconnect";


                //enable message entry
                MessageTextBox.IsEnabled = true;
                SendMessageButton.IsEnabled = true;
                StopButton.IsEnabled = true;

                //enable server and port text boxes
                ServerTextBox.IsEnabled = false;
                PortTextBox.IsEnabled = false;

                //create animations
                var ledBlurRadiusAnimation = new DoubleAnimation(0, LedBlurRadius,
                    TimeSpan.FromSeconds(LightToggleAnimLength));
                var logoImageBlurRadiusAnimation = new DoubleAnimation(0, LogoBlurRadius,
                    TimeSpan.FromSeconds(LightToggleAnimLength));
                var ledOpacityAnimation = new DoubleAnimation(LedOffOpacity, LedOnOpacity,
                    TimeSpan.FromSeconds(LightToggleAnimLength));
                var logoOpacityAnimation = new DoubleAnimation(LogoOffOpacity, LogoOnOpacity,
                    TimeSpan.FromSeconds(LightToggleAnimLength));

                //start animations
                _ledBlurEffect.BeginAnimation(BlurEffect.RadiusProperty, ledBlurRadiusAnimation);
                _logoBlurEffect.BeginAnimation(BlurEffect.RadiusProperty, logoImageBlurRadiusAnimation);
                ConnectionStatusLedImage.BeginAnimation(OpacityProperty, ledOpacityAnimation);
                LogoImageBlurLayer.BeginAnimation(OpacityProperty, logoOpacityAnimation);
                LogoImage.BeginAnimation(OpacityProperty, logoOpacityAnimation);
            }
        }

        private void stopButton_Click(object sender, RoutedEventArgs e)
        {
            if (_cancellationTokenSource != null && _cancellationTokenSource.Token.CanBeCanceled)
            {
                //cancel current animation thread
                _cancellationTokenSource.Cancel();
            }

            _opcClient.TurnOffAllPixels();
        }
    }
}