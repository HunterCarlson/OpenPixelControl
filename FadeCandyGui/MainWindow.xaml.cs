using System.Windows;
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


        const double onOpacity = 88;
        const double offOpacity = 88;

        public MainWindow()
        {
            InitializeComponent();

            ConnectionStatusLedImage.Opacity = offOpacity;
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

        private void button_Click(object sender, RoutedEventArgs e)
        {

        }

        private void ConnectButton_Click(object sender, RoutedEventArgs e)
        {
            // 0 - 100
            // 88 = on
            // 35 = off


            ConnectionStatusLedImage.Opacity = onOpacity;
        }
    }
}