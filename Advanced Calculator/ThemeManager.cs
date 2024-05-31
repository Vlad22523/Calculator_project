using MyCustomControls;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Advanced_Calculator
{
    public static class ThemeManager
    {
        public static bool IsDarkMode { get; set; } = true;

        public static void ApplyTheme(Form form)
        {
            form.BackColor = IsDarkMode ? Color.FromArgb(30, 30, 30) : Color.White;

            foreach (Control control in form.Controls)
            {
                if (control is Button)
                {
                    control.BackColor = IsDarkMode ? Color.FromArgb(50, 50, 50) : SystemColors.Control;
                    control.ForeColor = IsDarkMode ? Color.White : SystemColors.ControlText;
                }
                else if (control is TextBoxBase textBox)
                {
                    textBox.BorderStyle = BorderStyle.FixedSingle;
                    textBox.ForeColor = IsDarkMode ? Color.White : SystemColors.ControlText;
                    textBox.BackColor = IsDarkMode ? Color.FromArgb(50, 50, 50) : SystemColors.Control;

                    Pen borderPen = new Pen(IsDarkMode ? Color.White : Color.Black);
                    textBox.Paint += (sender, e) =>
                    {
                        e.Graphics.DrawRectangle(borderPen, 0, 0, textBox.Width - 1, textBox.Height - 1);
                    };
                }
            }
        }

        public static void UpdateToggleButton(MoonSunToggleButton toggleButton)
        {
            toggleButton.Checked = !IsDarkMode;
        }
    }
}
