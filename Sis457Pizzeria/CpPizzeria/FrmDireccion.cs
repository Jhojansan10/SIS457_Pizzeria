using CadPizzeria;
using ClnPizzeria;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace CpPizzeria
{
    public partial class FrmDireccion : Form
    {
        private bool esNuevo;
        private int idSeleccionado;

        public FrmDireccion()
        {
            InitializeComponent();

            // Diseño en runtime
            dgvDirecciones.Dock = DockStyle.Fill;
            dgvDirecciones.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvDirecciones.MultiSelect = false;
            dgvDirecciones.BringToFront();

            // Eventos
            dgvDirecciones.SelectionChanged += dgvDirecciones_SelectionChanged;
            btnBuscar.Click += btnBuscar_Click;
            btnNuevo.Click += btnNuevo_Click;
            btnEditar.Click += btnEditar_Click;
            btnGuardar.Click += btnGuardar_Click;
            btnEliminar.Click += btnEliminar_Click;
            btnCancelar.Click += btnCancelar_Click;
        }

        private void FrmDireccion_Load(object sender, EventArgs e)
        {
            RefrescarGrilla();
            SeleccionarPrimeraFila();
            EstadoCampos(false);
        }

        #region Carga y gestión

        private void RefrescarGrilla()
        {
            dgvDirecciones.DataSource = DireccionCln.Listar();
            if (dgvDirecciones.Columns["direccion_id"] != null)
                dgvDirecciones.Columns["direccion_id"].Visible = false;
            dgvDirecciones.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        }

        private void SeleccionarPrimeraFila()
        {
            if (dgvDirecciones.Rows.Count > 0)
                dgvDirecciones.Rows[0].Selected = true;
        }

        private void EstadoCampos(bool editando)
        {
            txtDireccion.Enabled = editando;
            dtpFechaRegistro.Enabled = false;    // siempre auto
            chkEstado.Enabled = editando;

            btnGuardar.Enabled = editando;
            btnCancelar.Enabled = editando;

            btnNuevo.Enabled = !editando;
            btnEditar.Enabled = !editando && idSeleccionado != 0;
            btnEliminar.Enabled = !editando && idSeleccionado != 0;

            btnBuscar.Enabled = !editando;
            txtBuscar.Enabled = !editando;
            dgvDirecciones.Enabled = !editando;
        }

        private void LimpiarFormulario()
        {
            idSeleccionado = 0;
            esNuevo = false;
            txtDireccion.Clear();
            chkEstado.Checked = true;
            dtpFechaRegistro.Value = DateTime.Now;
        }

        private bool ValidarCampos()
        {
            if (string.IsNullOrWhiteSpace(txtDireccion.Text))
            {
                MessageBox.Show("Ingrese la dirección.", "Aviso",
                                MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }
            return true;
        }

        #endregion

        #region Eventos UI

        private void dgvDirecciones_SelectionChanged(object sender, EventArgs e)
        {
            var row = dgvDirecciones.CurrentRow;
            if (row == null || row.IsNewRow) return;

            // ID
            idSeleccionado = Convert.ToInt32(row.Cells["direccion_id"].Value);

            // Rellenar los controles con la columna correspondiente
            txtDireccion.Text = row.Cells["direccion"].Value.ToString();
            chkEstado.Checked = Convert.ToBoolean(row.Cells["estado"].Value);
            dtpFechaRegistro.Value = Convert.ToDateTime(row.Cells["fecha_registro"].Value);

            // Ajustar botones
            btnEditar.Enabled = true;
            btnEliminar.Enabled = true;
        }


        private void btnBuscar_Click(object sender, EventArgs e)
        {
            var criterio = txtBuscar.Text.Trim().ToLower();
            dgvDirecciones.DataSource = string.IsNullOrEmpty(criterio)
                ? DireccionCln.Listar()
                : DireccionCln.Buscar(criterio);
        }

        private void btnNuevo_Click(object sender, EventArgs e)
        {
            LimpiarFormulario();
            esNuevo = true;
            EstadoCampos(true);
            txtDireccion.Focus();
        }

        private void btnEditar_Click(object sender, EventArgs e)
        {
            if (idSeleccionado == 0)
            {
                MessageBox.Show("Seleccione una dirección para editar.", "Aviso",
                                MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            esNuevo = false;
            EstadoCampos(true);
        }

        private void btnGuardar_Click(object sender, EventArgs e)
        {
            if (!ValidarCampos()) return;

            var d = new DIRECCION
            {
                direccion = txtDireccion.Text.Trim(),
                estado = chkEstado.Checked,
                fecha_registro = DateTime.Now
            };

            if (esNuevo)
                DireccionCln.Insertar(d);
            else
            {
                d.direccion_id = idSeleccionado;
                DireccionCln.Actualizar(d);
            }

            RefrescarGrilla();
            LimpiarFormulario();
            EstadoCampos(false);
        }

        private void btnEliminar_Click(object sender, EventArgs e)
        {
            if (idSeleccionado == 0)
            {
                MessageBox.Show("Seleccione una dirección para eliminar.", "Aviso",
                                MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (MessageBox.Show("¿Seguro de eliminar esta dirección?", "Confirmar",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                DireccionCln.Eliminar(idSeleccionado);
                RefrescarGrilla();
                LimpiarFormulario();
                EstadoCampos(false);
            }
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            LimpiarFormulario();
            EstadoCampos(false);
        }

        #endregion
    }
}
