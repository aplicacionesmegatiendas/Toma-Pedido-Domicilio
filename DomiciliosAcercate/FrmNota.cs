using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DomiciliosEntrecaminos
{
    public partial class FrmNota : Form
    {
        DataGridViewCell cell;
        public FrmNota( DataGridViewCell cell)
        {
            InitializeComponent();
            this.cell = cell;
        }

        private void txt_nota_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode.Equals(Keys.Enter))
            {
                cell.Value = txt_nota.Text.Trim();
                this.Close();
            }
        }
    }
}
