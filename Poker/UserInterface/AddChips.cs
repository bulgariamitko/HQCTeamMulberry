namespace Poker
{
    using System;
    using System.Drawing;
    using System.Windows.Forms;

    public partial class AddChips : Form
    {
        private const int MaxChips = 100000000;
        private int a;

        public AddChips()
        {
            FontFamily fontFamily = new FontFamily("Arial");
            this.InitializeComponent();
            this.ControlBox = false;
            this.label1.BorderStyle = BorderStyle.FixedSingle;
        }

        public int A
        {
            get { return this.a; }
            set { this.a = value; }
        }

        public void Button1Click(object sender, EventArgs e)
        {
            int parsedValue;
            if (int.Parse(this.textBox1.Text) > MaxChips)
            {
                throw new InvalidOperationException(string.Format("The maximium chips you can add is {0}", MaxChips));
            }

            if (!int.TryParse(this.textBox1.Text, out parsedValue))
            {
                throw new InvalidOperationException("This is a number only field");
            }

            this.a = int.Parse(this.textBox1.Text);
            this.Close();
        }

        private void Button2Click(object sender, EventArgs e)
        {
            var message = "Are you sure?";
            var title = "Quit";
            var result = MessageBox.Show(message, title, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            switch (result)
            {
                case DialogResult.No:
                    break;
                case DialogResult.Yes:
                    Application.Exit();
                    break;
            }
        }
    }
}
