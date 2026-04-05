using System;
using System.Numerics;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;
using System.Security.Cryptography;

namespace WindowsFormsApp2
{
    public partial class Form1 : Form
    {
        private BigInteger p = BigInteger.Parse("640241945116935222531336144390977239169"); 
        private BigInteger g = 2; 
        private BigInteger x = 234567890; 
        private BigInteger y; 

        public Form1()
        {
            InitializeComponent();
            y = BigInteger.ModPow(g, x, p); 
            SetupInterface();
        }


        private void BtnEncrypt_Click(object sender, EventArgs e)
        {
            string input = txtInput.Text;
            if (string.IsNullOrEmpty(input)) return;

            Stopwatch sw = Stopwatch.StartNew();

            byte[] bytes = Encoding.Default.GetBytes(input);
            BigInteger m = new BigInteger(bytes);

            if (m >= p) { MessageBox.Show("Текст слишком длинный для данного модуля P!"); return; }

            BigInteger k = 12345; 
            BigInteger a = BigInteger.ModPow(g, k, p);
            BigInteger b = (BigInteger.ModPow(y, k, p) * m) % p;

            sw.Stop();

            txtCipherA.Text = Convert.ToBase64String(a.ToByteArray());
            txtCipherB.Text = Convert.ToBase64String(b.ToByteArray());
            lblTimeEnc.Text = $"Время: {sw.Elapsed.TotalMilliseconds:F4} мс";
        }

        private void BtnDecrypt_Click(object sender, EventArgs e)
        {
            try
            {
                Stopwatch sw = Stopwatch.StartNew();

                BigInteger a = new BigInteger(Convert.FromBase64String(txtCipherA.Text));
                BigInteger b = new BigInteger(Convert.FromBase64String(txtCipherB.Text));

                BigInteger exponent = p - 1 - x;
                BigInteger m = (b * BigInteger.ModPow(a, exponent, p)) % p;

                byte[] decodedBytes = m.ToByteArray();
                txtResult.Text = Encoding.Default.GetString(decodedBytes);

                sw.Stop();
                lblTimeDec.Text = $"Время: {sw.Elapsed.TotalMilliseconds:F4} мс";
            }
            catch { MessageBox.Show("Ошибка в данных шифра!"); }
        }

        #region Оформление интерфейса (можно заменить на конструктор)
        private TextBox txtInput, txtCipherA, txtCipherB, txtResult;
        private Label lblTimeEnc, lblTimeDec;

        private void SetupInterface()
        {
            this.Text = "Эль-Гамаль Шифратор";
            this.Size = new System.Drawing.Size(450, 450);

            Label l1 = new Label { Text = "ФИО (Исходный текст):", Top = 10, Left = 10, Width = 200 };
            txtInput = new TextBox { Top = 30, Left = 10, Width = 400 };

            Button btnEnc = new Button { Text = "Зашифровать", Top = 60, Left = 10, Width = 150 };
            btnEnc.Click += BtnEncrypt_Click;
            lblTimeEnc = new Label { Top = 65, Left = 170, Width = 200 };

            Label l2 = new Label { Text = "Зашифрованные блоки (Base64):", Top = 100, Left = 10, Width = 200 };
            txtCipherA = new TextBox { Top = 120, Left = 10, Width = 400, ReadOnly = true };
            txtCipherB = new TextBox { Top = 150, Left = 10, Width = 400, ReadOnly = true };

            Button btnDec = new Button { Text = "Расшифровать", Top = 190, Left = 10, Width = 150 };
            btnDec.Click += BtnDecrypt_Click;
            lblTimeDec = new Label { Top = 195, Left = 170, Width = 200 };

            Label l3 = new Label { Text = "Результат расшифрования:", Top = 230, Left = 10, Width = 200 };
            txtResult = new TextBox { Top = 250, Left = 10, Width = 400, ReadOnly = true };

            this.Controls.AddRange(new Control[] { l1, txtInput, btnEnc, lblTimeEnc, l2, txtCipherA, txtCipherB, btnDec, lblTimeDec, l3, txtResult });
        }
        #endregion
    }
}
