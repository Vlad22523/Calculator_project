using System.Drawing;
using System;
using System.Windows.Forms;

namespace MyCustomControls
{
    public partial class MoonSunToggleButton : CheckBox
    {
        private Image moonImage;
        private Image sunImage;

        public MoonSunToggleButton()
        {

            moonImage = Properties.Resources.moon;
            sunImage = Properties.Resources.sun;

            this.Appearance = Appearance.Button;
            this.TextAlign = ContentAlignment.MiddleCenter;
            this.FlatStyle = FlatStyle.Flat;
            this.FlatAppearance.BorderSize = 0;
            this.Size = new Size(16, 16);
            this.Image = sunImage;
        }

        protected override void OnCheckedChanged(EventArgs e)
        {
            base.OnCheckedChanged(e);
            this.Image = this.Checked ? moonImage : sunImage;
        }
    }

}
