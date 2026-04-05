using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp1
{

        public partial class Form1 : Form
        {
            private ElGamalCipher cipher;
            private TextBox txtInput;
            private TextBox txtOutput;
            private TextBox txtP;
            private TextBox txtG;
            private TextBox txtX;
            private TextBox txtY;
            private TextBox txtK;
            private Button btnGenerateKeys;
            private Button btnEncrypt;
            private Button btnDecrypt;
            private Label lblEncryptTime;
            private Label lblDecryptTime;
            private RadioButton rbBase64;
            private RadioButton rbASCII;

            public Form1()
            {
                InitializeComponent();
                cipher = new ElGamalCipher();
            }

            private void InitializeComponent()
            {

            }

            private void BtnGenerateKeys_Click(object sender, EventArgs e)
            {
                try
                {
                    BigInteger p, g, x, y;
                    ElGamalKeyGenerator.GenerateKeys(out p, out g, out x, out y);
                    txtP.Text = p.ToString();
                    txtG.Text = g.ToString();
                    txtX.Text = x.ToString();
                    txtY.Text = y.ToString();
                    txtK.Text = ElGamalKeyGenerator.GenerateRandomK(p).ToString();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка генерации ключей: {ex.Message}");
                }
            }

            private void BtnEncrypt_Click(object sender, EventArgs e)
            {
                try
                {
                    BigInteger p = BigInteger.Parse(txtP.Text);
                    BigInteger g = BigInteger.Parse(tpG.Text);
                    BigInteger y = BigInteger.Parse(txtY.Text);
                    BigInteger k = BigInteger.Parse(txtK.Text);

                    string input = txtInput.Text;
                    Encoding encoding = rbASCII.Checked ? Encoding.ASCII : Encoding.UTF8;

                    Stopwatch sw = Stopwatch.StartNew();
                    List<ElGamalCipher.CipherPair> encrypted = cipher.Encrypt(input, p, g, y, k, encoding);
                    sw.Stop();

                    string result = ElGamalCipher.CipherToString(encrypted);
                    txtOutput.Text = result;
                    lblEncryptTime.Text = $"Время шифрования: {sw.ElapsedMilliseconds} мс ({sw.ElapsedTicks} тиков)";
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка шифрования: {ex.Message}");
                }
            }

            private void BtnDecrypt_Click(object sender, EventArgs e)
            {
                try
                {
                    BigInteger p = BigInteger.Parse(txtP.Text);
                    BigInteger x = BigInteger.Parse(txtX.Text);

                    string cipherText = txtOutput.Text;
                    Encoding encoding = rbASCII.Checked ? Encoding.ASCII : Encoding.UTF8;

                    Stopwatch sw = Stopwatch.StartNew();
                    string decrypted = cipher.Decrypt(cipherText, p, x, encoding);
                    sw.Stop();

                    txtInput.Text = decrypted;
                    lblDecryptTime.Text = $"Время дешифрования: {sw.ElapsedMilliseconds} мс ({sw.ElapsedTicks} тиков)";
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка дешифрования: {ex.Message}");
                }
            }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
    }
    }

