using CadPizzeria;
using ClnPizzeria;
using System;
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

            // ————— Ajustes de diseño en tiempo de ejecución —————
            // Ocupa todo el espacio de su contenedor
            dgvDirecciones.Dock = DockStyle.Fill;
            // Que las filas se seleccionen completas
            dgvDirecciones.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvDirecciones.MultiSelect = false;
            // Traerlo por encima de cualquier otro control (p. ej. panel amarillo)
            dgvDirecciones.BringToFront();

            // Suscribimos el evento que rellena idSeleccionado + campos
            dgvDirecciones.SelectionChanged += dgvDirecciones_SelectionChanged;
        }

        private void FrmDireccion_Load(object sender, EventArgs e)
        {
            CargarClientes();
            RefrescarGrilla();
            SeleccionarPrimeraFila();
            EstadoCampos(false);
        }

        private void CargarClientes()
        {
            using (var db = new LabPizzeriaEntities())
            {
                var lista = db.USUARIO
                    .Where(u => u.estado && u.rol == "Cliente")
                    .Select(u => new { u.usuario_id, u.nombre })
                    .ToList();

                cboCliente.DataSource = lista;
                cboCliente.DisplayMember = "nombre";
                cboCliente.ValueMember = "usuario_id";
                cboCliente.SelectedIndex = -1;
            }
        }

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
            {
                dgvDirecciones.Rows[0].Selected = true;
                // Dispara SelectionChanged y carga los campos
            }
        }

        private void EstadoCampos(bool editando)
        {
            cboCliente.Enabled = editando;
            txtCalle.Enabled = editando;
            txtCiudad.Enabled = editando;
            txtCodigoPostal.Enabled = editando;
            txtIndicaciones.Enabled = editando;

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
            cboCliente.SelectedIndex = -1;
            txtCalle.Clear();
            txtCiudad.Clear();
            txtCodigoPostal.Clear();
            txtIndicaciones.Clear();
        }

        private bool ValidarCampos()
        {
            if (cboCliente.SelectedIndex == -1 ||
                string.IsNullOrWhiteSpace(txtCalle.Text) ||
                string.IsNullOrWhiteSpace(txtCiudad.Text) ||
                string.IsNullOrWhiteSpace(txtCodigoPostal.Text))
            {
                MessageBox.Show("Complete todos los campos obligatorios.",
                                "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }
            return true;
        }

        private void dgvDirecciones_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvDirecciones.CurrentRow == null ||
                dgvDirecciones.CurrentRow.IsNewRow)
                return;

            // Capturamos el ID
            var row = dgvDirecciones.CurrentRow;
            idSeleccionado = Convert.ToInt32(row.Cells["direccion_id"].Value);

            // Habilitamos botones
            btnEditar.Enabled = true;
            btnEliminar.Enabled = true;

            // —— Aquí rellenamos los campos del formulario ——
            cboCliente.Text = row.Cells["Cliente"].Value.ToString();
            txtCalle.Text = row.Cells["calle"].Value.ToString();
            txtCiudad.Text = row.Cells["ciudad"].Value.ToString();
            txtCodigoPostal.Text = row.Cells["codigo_postal"].Value.ToString();
            txtIndicaciones.Text = row.Cells["indicaciones"].Value.ToString();
        }

        private void btnBuscar_Click(object sender, EventArgs e)
        {
            var criterio = txtBuscar.Text.Trim();
            dgvDirecciones.DataSource =
                string.IsNullOrEmpty(criterio)
                    ? DireccionCln.Listar()
                    : DireccionCln.Buscar(criterio);
        }

        private void btnNuevo_Click(object sender, EventArgs e)
        {
            LimpiarFormulario();
            esNuevo = true;
            EstadoCampos(true);
            cboCliente.Focus();
        }

        private void btnEditar_Click(object sender, EventArgs e)
        {
            if (idSeleccionado == 0)
            {
                MessageBox.Show("Seleccione una dirección para editar.",
                                "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            esNuevo = false;
            EstadoCampos(true);
        }

        private void btnEliminar_Click(object sender, EventArgs e)
        {
            if (idSeleccionado == 0)
            {
                MessageBox.Show("Seleccione una dirección para eliminar.",
                                "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (MessageBox.Show("¿Seguro de eliminar esta dirección?",
                                "::: PIZZERIA :::",
                                MessageBoxButtons.YesNo,
                                MessageBoxIcon.Question)
                == DialogResult.Yes)
            {
                DireccionCln.Eliminar(idSeleccionado);
                MessageBox.Show("Dirección eliminada.",
                                "::: PIZZERIA :::",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Information);
                RefrescarGrilla();
                LimpiarFormulario();
            }
        }

        private void btnGuardar_Click(object sender, EventArgs e)
        {
            if (!ValidarCampos()) return;

            var d = new DIRECCION
            {
                usuario_id = (int)cboCliente.SelectedValue,
                calle = txtCalle.Text.Trim(),
                ciudad = txtCiudad.Text.Trim(),
                codigo_postal = txtCodigoPostal.Text.Trim(),
                indicaciones = txtIndicaciones.Text.Trim(),
                fecha_registro = DateTime.Now,
                estado = true
            };

            if (esNuevo)
                DireccionCln.Insertar(d);
            else
            {
                d.direccion_id = idSeleccionado;
                DireccionCln.Actualizar(d);
            }

            MessageBox.Show("Dirección guardada correctamente.",
                            "::: PIZZERIA :::",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Information);

            RefrescarGrilla();
            LimpiarFormulario();
            EstadoCampos(false);
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            LimpiarFormulario();
            EstadoCampos(false);
        }

        private void dgvDirecciones_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
    }
}
