using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Chess
{
    public partial class UserInput : Form
    {
        public string Data;
        public UserInput()
        {
            InitializeComponent();
            label1.Text = "You Have Promotion To Change This Pawn To \n a Queen, Knight, Rook ,or Bishop\n So Enter the first letter of piece  You need. \n An Example 'k' of knight";
        }
        private void Ok_Click(object sender, EventArgs e)
        {
            Data = Input.Text;
            this.Close();
        }
        private void UserInput_Load(object sender, EventArgs e)
        {
        }
    }
}
