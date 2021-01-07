using System;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace RSAReciver
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly RsaDecrypt _rsa;
        private readonly TcpServer _tcpServer;
        const int PortNo = 13000;
        const string ServerIp = "127.0.0.1";
        
        // this delegate is used tu update the UI Cipher text
        delegate void UpdateCipherText(string cipher);


        public MainWindow()
        {
            InitializeComponent();
            // Here i create a tcp server
            _tcpServer = new TcpServer();
            // here i Start the tcp server
            Task.Run((() => _tcpServer.TcpServerStart(ServerIp, PortNo)));
            // here i subscribe to the event when message was received
            _tcpServer.MessageReceived += _tcpServer_MessageReceived;
            // instantiates the Rsa Decrypt class 
            _rsa = new RsaDecrypt();
            // Creates new keys
            _rsa.AssignNewKey();

            // Displays All the Key Data to the Window START ---------------------
            // Public Date
            ExponentText.Text = Convert.ToBase64String(_rsa._publicKey.Exponent);
            ModulusText.Text = Convert.ToBase64String(_rsa._publicKey.Modulus);
            // Private Data
            DText.Text = Convert.ToBase64String(_rsa._privateKey.D);
            DpText.Text = Convert.ToBase64String(_rsa._privateKey.DP);
            DqText.Text = Convert.ToBase64String(_rsa._privateKey.DQ);
            InverseQText.Text = Convert.ToBase64String(_rsa._privateKey.InverseQ);
            PText.Text = Convert.ToBase64String(_rsa._privateKey.P);
            QText.Text = Convert.ToBase64String(_rsa._privateKey.Q);
        }

        private void _tcpServer_MessageReceived(object sender, string e)
        {
            
            Dispatcher.BeginInvoke(new UpdateCipherText(UpdateCipher), new object[] {e});
        }

        private void UpdateCipher(string cipher)
        {
            CipherBytesText.Text = cipher;
        }
        /// <summary>
        /// Decrypts the Data
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Decrypt_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(CipherBytesText.Text))
            {
                MessageBox.Show("You must enter code to decrypt");
            }
            else
            {
                byte[] decryptedBytes = _rsa.DecryptData(Convert.FromBase64String(CipherBytesText.Text));
                DecryptedText.Text = Encoding.UTF8.GetString(decryptedBytes);

                //SGVsbG8gVGVhY2hlciE=
            }
        }

        private void SendKey_Click(object sender, RoutedEventArgs e)
        {
            TcpClient client = new TcpClient();
            client.Connect(ServerIp, ModulusText.Text);
        }
    }
}