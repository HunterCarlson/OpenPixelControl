using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
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


        public MainWindow()
        {
            InitializeComponent();

            //init text boxes to default server values
            ServerTextBox.Text = "127.0.0.1";
            PortTextBox.Text = OpcConstants.DefaultPort.ToString();

            // init sliders
            OnDurationSlider.Value = 1.0;
            OffDurationSlider.Value = 1.0;

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
            var message = MessageTextBox.Text.ToUpper();
            var onDuration = OnDurationSlider.Value*1000;
            var offDuration = OffDurationSlider.Value*1000;

            _cancellationTokenSource = new CancellationTokenSource();
            var token = _cancellationTokenSource.Token;

            Task.Run(async () =>
            {
                foreach (var c in message)
                {
                    var frame = _letterWall.CreateLetterFrame(c);
                    _opcClient.WriteFrame(frame);
                    await Task.Delay((int) onDuration, token);
                    _opcClient.TurnOffAllPixels();
                    await Task.Delay((int) offDuration, token);
                }
            }, token);
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


                    _opcClient.Server = ServerTextBox.Text;
                    _opcClient.Port = Convert.ToInt32(PortTextBox.Text);


                    // blink leds on connect
                    // do in new thread to not block UI
                    // http://stackoverflow.com/questions/363377/how-do-i-run-a-simple-bit-of-code-in-a-new-thread
                    var backgroundWorker = new BackgroundWorker();

                    backgroundWorker.DoWork += delegate
                    {
                        _opcClient.SetDitheringAndInterpolation(true);

                        var frame = _opcClient.SingleColorFrame(200, 0, 0);

                        _opcClient.TurnOffAllPixels();
                        Thread.Sleep(500);
                        _opcClient.WriteFrame(frame);
                        Thread.Sleep(500);
                        _opcClient.TurnOffAllPixels();
                        Thread.Sleep(500);
                        _opcClient.WriteFrame(frame);
                        Thread.Sleep(500);
                        _opcClient.TurnOffAllPixels();
                        Thread.Sleep(500);
                        _opcClient.WriteFrame(frame);
                        Thread.Sleep(500);
                        _opcClient.TurnOffAllPixels();
                        Thread.Sleep(500);

                        _opcClient.SetDitheringAndInterpolation(false);
                    };

                    backgroundWorker.RunWorkerCompleted += delegate { Debug.WriteLine("Blink done"); };
                    backgroundWorker.RunWorkerAsync();
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
            //cancel current animation thread
            _cancellationTokenSource.Cancel();
            //set all leds to off - do it twice to bypass interpolation
            _opcClient.TurnOffAllPixels();
            _opcClient.TurnOffAllPixels();
        }
    }
}