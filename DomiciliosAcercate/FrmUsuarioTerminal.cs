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
    public partial class FrmUsuarioTerminal : Form
    {
        public FrmUsuarioTerminal()
        {
            InitializeComponent();
        }

        private void SoloNumeros(object sender, KeyPressEventArgs e)
        {
            if (!(char.IsDigit(e.KeyChar) || char.IsControl(e.KeyChar) || char.IsPunctuation(e.KeyChar)))
            {
                e.Handled = true;
            }
            
            else if (!(e.KeyChar == '1' || e.KeyChar == '2' || e.KeyChar == '3' || e.KeyChar == '4' || e.KeyChar == '5' || e.KeyChar == '6' || e.KeyChar == '7' || e.KeyChar == '8' || e.KeyChar == '9' || e.KeyChar == '0' || e.KeyChar == (char)8))
            {
                e.Handled = true;
            }
        }

        private void ListarTerminales()
        {
            Terminal terminal = new Terminal();
            dgv_terminales.AutoGenerateColumns = false;
            dgv_terminales.Columns[0].DataPropertyName = "cn_terminal";
            dgv_terminales.Columns[1].DataPropertyName = "cn_numero";
            dgv_terminales.Columns[2].DataPropertyName = "cn_usuario";
            dgv_terminales.Columns[3].DataPropertyName = "cn_descripcion";
            dgv_terminales.DataSource = terminal.ListarTerminales();
        }
        private void btn_cerrar_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void FrmUsuarioTerminal_Load(object sender, EventArgs e)
        {
            try
            {
                ListarTerminales();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message,"Error",MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btn_guardar_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txt_terminal.Text)||string.IsNullOrEmpty(txt_descripcion.Text))
            {
                MessageBox.Show("Escriba el número de la terminal y la descripción","Aviso",MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            try
            {
                Terminal terminal = new Terminal();
				terminal.CrearTerminal(Convert.ToInt32(txt_terminal.Text.Trim()), txt_descripcion.Text.Trim());
                MessageBox.Show("Terminal creada exitosamente", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                ListarTerminales();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void txt_terminal_Leave(object sender, EventArgs e)
        {
            txt_usuario.Text = "usuario" + txt_terminal.Text;
        }

        private void dgv_terminales_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex==4)
            {
                try
                {
                    new FrmEditarDescripcionUsuarioTerminal(Convert.ToInt32(dgv_terminales[0, dgv_terminales.CurrentRow.Index].Value),
                                                        Convert.ToString(dgv_terminales[2, dgv_terminales.CurrentRow.Index].Value),
                                                        Convert.ToString(dgv_terminales[3, dgv_terminales.CurrentRow.Index].Value)).ShowDialog(this);
                    ListarTerminales();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
    }
}
