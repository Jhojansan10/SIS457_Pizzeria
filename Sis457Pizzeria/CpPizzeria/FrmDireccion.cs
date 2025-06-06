using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlTypes;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using CadPizzeria;
using ClnPizzeria;

namespace CpPizzeria
{
    public partial class FrmDireccion : Form
    {
        private bool esNuevo = false;
        private int idDireccion = 0;

        public FrmDireccion()
        {
            InitializeComponent();
        }

        private void FrmDireccion_Load(object sender, EventArgs e)
        {
            cargarClientes();
            listar();
        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void cargarClientes()
        {
            using (var db = new LabPizzeriaEntities())
            {
                var clientes = db.USUARIO
                    .Where(c => c.estado == true && c.rol == "Cliente")
                    .Select(c => new { c.usuario_id, c.nombre })
                    .ToList();

                cboCliente.DataSource = clientes;
                cboCliente.DisplayMember = "nombre";
                cboCliente.ValueMember = "usuario_id";
                cboCliente.SelectedIndex = -1;
            }
        }

        private void listar()
        {
            dgvDirecciones.DataSource = DireccionCln.listar();
        }

        private void btnBuscar_Click(object sender, EventArgs e)
        {
            string criterio = txtBuscar.Text.Trim();
            if (criterio == "")
                listar();
            else
                dgvDirecciones.DataSource = DireccionCln.buscar(criterio);
        }
        

        private void btnNuevo_Click(object sender, EventArgs e)
        {
            limpiarFormulario();
            esNuevo = true;
            cboCliente.Focus();
        }

        private void btnEditar_Click(object sender, EventArgs e)
        {
            if (idDireccion == 0)
            {
                MessageBox.Show("Seleccione una dirección para editar.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            esNuevo = false;
        }

        private void btnEliminar_Click(object sender, EventArgs e)
        {
            if (idDireccion == 0)
            {
                MessageBox.Show("Seleccione una dirección para eliminar.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (MessageBox.Show("¿Está seguro de eliminar esta dirección?", "::: PIZZERIA :::",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                DireccionCln.eliminar(idDireccion);
                MessageBox.Show("Dirección eliminada correctamente", "::: PIZZERIA :::", MessageBoxButtons.OK, MessageBoxIcon.Information);
                listar();
                limpiarFormulario();
            }
        }

        private void btnGuardar_Click(object sender, EventArgs e)
        {
            if (cboCliente.SelectedIndex == -1 ||
        string.IsNullOrWhiteSpace(txtCalle.Text) ||
        string.IsNullOrWhiteSpace(txtCiudad.Text) ||
        string.IsNullOrWhiteSpace(txtCodigoPostal.Text))
            {
                MessageBox.Show("Complete los campos obligatorios.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var direccion = new DIRECCION
            {
                usuario_id = Convert.ToInt32(cboCliente.SelectedValue),
                calle = txtCalle.Text.Trim(),
                ciudad = txtCiudad.Text.Trim(),
                codigo_postal = txtCodigoPostal.Text.Trim(),
                indicaciones = txtIndicaciones.Text.Trim()
            };

            if (esNuevo)
                DireccionCln.insertar(direccion);
            else
            {
                direccion.direccion_id = idDireccion;
                DireccionCln.actualizar(direccion);
            }

            MessageBox.Show("Dirección guardada correctamente.", "::: PIZZERIA :::", MessageBoxButtons.OK, MessageBoxIcon.Information);
            listar();
            limpiarFormulario();
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            limpiarFormulario();
        }

        private void dgvDirecciones_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                idDireccion = Convert.ToInt32(dgvDirecciones.Rows[e.RowIndex].Cells["direccion_id"].Value);
                cboCliente.Text = dgvDirecciones.Rows[e.RowIndex].Cells["Cliente"].Value.ToString();
                txtCalle.Text = dgvDirecciones.Rows[e.RowIndex].Cells["calle"].Value.ToString();
                txtCiudad.Text = dgvDirecciones.Rows[e.RowIndex].Cells["ciudad"].Value.ToString();
                txtCodigoPostal.Text = dgvDirecciones.Rows[e.RowIndex].Cells["codigo_postal"].Value.ToString();
                txtIndicaciones.Text = dgvDirecciones.Rows[e.RowIndex].Cells["indicaciones"].Value.ToString();
            }
        }

        private void limpiarFormulario()
        {
            idDireccion = 0;
            esNuevo = false;
            cboCliente.SelectedIndex = -1;
            txtCalle.Clear();
            txtCiudad.Clear();
            txtCodigoPostal.Clear();
            txtIndicaciones.Clear();
        }

    }
}
