using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using Button = System.Windows.Forms.Button;

namespace Advanced_Calculator
{
    public partial class Form2 : Form
    {
        public static bool isDarkMode = true;

        [DllImport("dwmapi", PreserveSig = false)]
        static extern void DwmSetWindowAttribute(IntPtr hwnd, int dwAttribute, in bool pvAttribute, int cbAttribute);

        protected override void OnHandleCreated(EventArgs e)
        {
            const int DWMWA_USE_IMMERSIVE_DARK_MODE = 20;
            DwmSetWindowAttribute(Handle, DWMWA_USE_IMMERSIVE_DARK_MODE, true, Marshal.SizeOf<bool>());
        }

        public Form2()
        {
            InitializeComponent();
            ThemeManager.ApplyTheme(this);
            ThemeManager.UpdateToggleButton(moonSunToggleButton1);
        }

        private CalculatorLogic calculatorLogic = new CalculatorLogic();
        private bool isDecimalPointPressed = false;
        private bool isTrigonometric = false;
        private bool isConversion = false;

        private void MoonSunToggleButton1_CheckedChanged(object sender, EventArgs e)
        {
            ThemeManager.IsDarkMode = !moonSunToggleButton1.Checked;
            ThemeManager.ApplyTheme(Form1.ActiveForm);

            foreach (Form form in Application.OpenForms)
            {
                if (form is Form1 form1)
                {
                    ThemeManager.UpdateToggleButton(form1.moonSunToggleButton1);
                }
                else if (form is Form2 form2)
                {
                    ThemeManager.UpdateToggleButton(form2.moonSunToggleButton1);
                }
            }
        }

        private void UpdateColors()
        {
            this.BackColor = isDarkMode ? Color.FromArgb(30, 30, 30) : Color.White;

            foreach (Control control in this.Controls)
            {
                if (control is System.Windows.Forms.Button)
                {
                    control.BackColor = isDarkMode ? Color.FromArgb(50, 50, 50) : SystemColors.Control;
                    control.ForeColor = isDarkMode ? Color.White : SystemColors.ControlText;
                }

                if (control == txtBox || control == txtDis)
                {
                    control.BackColor = isDarkMode ? Color.FromArgb(50, 50, 50) : SystemColors.Control;
                    control.ForeColor = isDarkMode ? Color.White : SystemColors.ControlText;

                    if (control is TextBoxBase textBox)
                    {
                        textBox.BorderStyle = BorderStyle.FixedSingle;
                        textBox.ForeColor = isDarkMode ? Color.White : SystemColors.ControlText;
                        textBox.BackColor = isDarkMode ? Color.FromArgb(50, 50, 50) : SystemColors.Control;

                        Pen borderPen = new Pen(isDarkMode ? Color.White : Color.Black);

                        textBox.Paint += (sender, e) =>
                        {
                            e.Graphics.DrawRectangle(borderPen, 0, 0, textBox.Width - 1, textBox.Height - 1);
                        };
                    }
                }
            }
        }

        private void NumberButton_Click(object sender, EventArgs e)
        {
            Button button = (Button)sender;
            if (txtBox.Text == "0" || isTrigonometric || isConversion)
                txtBox.Text = "";

            if (button.Text == ",")
            {
                if (!isDecimalPointPressed)
                {
                    txtBox.Text += button.Text;
                    isDecimalPointPressed = true;
                }
            }
            else
            {
                txtBox.Text += button.Text;
                isDecimalPointPressed = false;
            }

            isTrigonometric = false;
            isConversion = false;
        }

        private void OperatorButton_Click(object sender, EventArgs e)
        {

        }

        private void BtnEquals_Click(object sender, EventArgs e)
        {
            try
            {
                if (isTrigonometric || isConversion) return;
                calculatorLogic.Calculate(decimal.Parse(txtBox.Text));
                txtBox.Text = calculatorLogic.Result.ToString();
                txtDis.Text += txtBox.Text + "=";
                calculatorLogic.Clear();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void TrigonometricButton_Click(object sender, EventArgs e)
        {
            Button button = (Button)sender;
            string function = button.Text.ToLower();

            try
            {
                if (!decimal.TryParse(txtBox.Text, out decimal inputValue))
                {
                    throw new FormatException("Invalid input format.");
                }

                calculatorLogic.FirstNumber = inputValue;

                calculatorLogic.TrigonometricButtonPressed(function);

                double result = (double)calculatorLogic.Result;
                if (double.IsNaN(result) || double.IsInfinity(result))
                {
                    throw new ArgumentException("Result is out of range for this function.");
                }

                txtBox.Text = calculatorLogic.Result.ToString();
                txtDis.Text = button.Text + "(" + calculatorLogic.FirstNumber.ToString() + ")=" + txtBox.Text;
                isTrigonometric = true;

                calculatorLogic.Clear();
            }
            catch (FormatException ex)
            {
                MessageBox.Show(ex.Message);
                calculatorLogic.Clear();
            }
            catch (DivideByZeroException ex)
            {
                MessageBox.Show(ex.Message);
                calculatorLogic.Clear();
            }
            catch (ArgumentException ex)
            {
                MessageBox.Show(ex.Message);
                calculatorLogic.Clear();
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred: " + ex.Message);
                calculatorLogic.Clear();
            }
        }

        private void ConversionButton_Click(object sender, EventArgs e)
        {
            Button button = (Button)sender;
            string system = button.Text.ToLower();

            try
            {
                if (!int.TryParse(txtBox.Text, out int inputValue))
                {
                    throw new FormatException("Invalid input format for conversion. Please enter an integer.");
                }

                if (inputValue < 0 && (system == "bin" || system == "oct"))
                {
                    throw new OverflowException("Cannot convert negative numbers to binary or octal.");
                }

                calculatorLogic.FirstNumber = inputValue;
                calculatorLogic.ConversionButtonPressed(system);
                txtBox.Text = calculatorLogic.Result.ToString();
                txtDis.Text = button.Text + "(" + calculatorLogic.FirstNumber.ToString() + ")=" + txtBox.Text;
                isConversion = true;
                calculatorLogic.Clear();
            }
            catch (FormatException ex)
            {
                MessageBox.Show(ex.Message);
                calculatorLogic.Clear();
            }
            catch (OverflowException ex)
            {
                MessageBox.Show(ex.Message);
                calculatorLogic.Clear();
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred: " + ex.Message);
                calculatorLogic.Clear();
            }
        }



        private void BtnC_Click(object sender, EventArgs e)
        {
            calculatorLogic.Clear();
            txtBox.Text = "0";
            txtDis.Text = "";
            isDecimalPointPressed = false;
        }

        private void BtnCE_Click(object sender, EventArgs e)
        {
            txtBox.Text = "0";
            isDecimalPointPressed = false;
        }

        private void BtnBackspace_Click(object sender, EventArgs e)
        {
            if (txtBox.Text.Length > 1)
                txtBox.Text = txtBox.Text.Substring(0, txtBox.Text.Length - 1);
            else
                txtBox.Text = "0";
        }

        private void BtnMP_Click(object sender, EventArgs e)
        {
            if (txtBox.Text.StartsWith("-"))
                txtBox.Text = txtBox.Text.Substring(1);
            else
                txtBox.Text = "-" + txtBox.Text;
        }

        private void BtnDot_Click(object sender, EventArgs e)
        {
            if (!txtBox.Text.Contains(","))
            {
                txtBox.Text += ",";
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            Form1 form1 = new Form1();
            form1.Show();
            this.Close();
        }

        private void Form2_Activated(object sender, EventArgs e)
        {
            ThemeManager.ApplyTheme(this);
        }
    }
}
