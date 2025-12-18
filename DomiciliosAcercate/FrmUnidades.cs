using System;
using System.Configuration;
using System.Windows.Forms;

namespace DomiciliosEntrecaminos
{
    public partial class FrmUnidades : Form
    {
        FrmPpal frm;
        public FrmUnidades(FrmPpal frm)
        {
            InitializeComponent();
            this.frm = frm;
        }

        private void ObtenerExistencia(string rowid_tpv, string rowid_item_ext, string id_sucursal_perfil, string rowid_tercero)
        {
            Item item = new Item();
            Configuracion configuracion = new Configuracion();
            try
            {
                string[] res = item.ObtenerExistencia(rowid_tpv, rowid_item_ext, id_sucursal_perfil, rowid_tercero);
                if (res != null)
                {
                    object[] conf = configuracion.ObtenerConfiguracion();

                    string lp = conf[6].ToString();

                    lbl_existencia.Text = res[3];
					Configuracion.IdSucursalCliente = res[0];
                    if (lp.Equals(string.Empty))
                    {
						Configuracion.IdListaPrecio = res[1];
                    }
                    else
                    {
                        Configuracion.IdListaPrecio = lp;
                    }
					Configuracion.IdInstalacion = res[2];
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void FrmUnidades_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (dgv_unds.SelectedRows.Count > 0)
                {
                    if (Convert.ToDecimal(dgv_unds[2, dgv_unds.CurrentRow.Index].Value.ToString().Replace('.', Configuracion.separador).Replace(',', Configuracion.separador)) > 0)
                    {
                        frm.txt_unidad.Text = Convert.ToString(dgv_unds[1, dgv_unds.CurrentRow.Index].Value);
                        frm.txt_precio.Text = Convert.ToString(dgv_unds[2, dgv_unds.CurrentRow.Index].Value);
                        Configuracion.RowidItemExt = Convert.ToString(dgv_unds[0, dgv_unds.CurrentRow.Index].Value);
						Configuracion.UnidadMedida = Convert.ToString(dgv_unds[1, dgv_unds.CurrentRow.Index].Value);
                        this.DialogResult = DialogResult.OK;
                        this.Close();
                    }
                }
            }
        }

        private void btn_cerrar_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void FrmUnidades_Load(object sender, EventArgs e)
        {
            if (dgv_unds.RowCount > 0)
            {
                try
                {
                    ObtenerExistencia(frm.txt_tpv.Text.Trim(), Convert.ToString(dgv_unds[0, 0].Value), Convert.ToString(frm.cmb_sucursal.SelectedValue).Trim(), Configuracion.RowidTercero);

                    Item item = new Item();
                    ////////////PARA OBTENER EL PRECIO DE VENTA REAL/////////////
                    for (int i = 0; i < dgv_unds.RowCount; i++)
                    {
                        string[] res = item.ObtenerPrecio(Configuracion.IdListaPrecio, dgv_unds[0, i].Value.ToString(), dgv_unds[1, i].Value.ToString());
                        dgv_unds[2, i].Value = res[1];
                    }
                    ///////////////////////////////////////////////////////////
                    this.Top = this.Owner.Height / 2;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
    }
}
