using System;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using System.Text;
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
        private RsaDecrypt _rsa;

        public MainWindow()
        {
            InitializeComponent();
            _rsa = new RsaDecrypt();
            _rsa.AssignNewKey();

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
    }
}