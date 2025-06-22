using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using CadPizzeria;
using ClnPizzeria;

namespace CpPizzeria
{
    public partial class FrmPlatillo : Form
    {
        private bool esNuevo = false;
        private int idPlatillo = 0;

        public FrmPlatillo()
        {
            InitializeComponent();
        }

        private void FrmPlatillo_Load(object sender, EventArgs e)
        {
            // Ajustamos al modo listado
            this.Size = new Size(835, 362);
            listar();
            cargarCategorias();
        }

        private void listar()
        {
            var lista = PlatilloCln.listar();
            dgvLista.DataSource = lista;
            if (dgvLista.Columns["platillo_id"] != null)
                dgvLista.Columns["platillo_id"].Visible = false;

            btnEditar.Enabled = lista.Count > 0;
            btnEliminar.Enabled = lista.Count > 0;
        }

        private void cargarCategorias()
        {
            using (var db = new LabPizzeriaEntities())
            {
                var categorias = db.CATEGORIA
                    .Where(c => c.estado == true)
                    .OrderBy(c => c.nombre)
                    .ToList();

                cboCategoria.DataSource = categorias;
                cboCategoria.DisplayMember = "nombre";
                cboCategoria.ValueMember = "categoria_id";
                cboCategoria.SelectedIndex = -1;
            }
        }

        private void limpiar()
        {
            idPlatillo = 0;
            txtNombre.Clear();
            txtDescripcion.Clear();
            txtPrecio.Clear();
            txtTiempo.Clear();
            txtImagen.Clear();
            cboCategoria.SelectedIndex = -1;
            chkEstado.Checked = true;
        }

        private bool validar()
        {
            bool esValido = true;
            erpNombre.SetError(txtNombre, "");
            erpPrecio.SetError(txtPrecio, "");
            erpCategoria.SetError(cboCategoria, "");

            if (string.IsNullOrWhiteSpace(txtNombre.Text))
            {
                erpNombre.SetError(txtNombre, "El nombre es obligatorio");
                esValido = false;
            }

            if (!decimal.TryParse(txtPrecio.Text.Trim(), out decimal pr) || pr < 0)
            {
                erpPrecio.SetError(txtPrecio, "Precio inválido o menor a 0");
                esValido = false;
            }

            if (cboCategoria.SelectedIndex == -1)
            {
                erpCategoria.SetError(cboCategoria, "Seleccione una categoría");
                esValido = false;
            }

            return esValido;
        }

        private void btnBuscar_Click(object sender, EventArgs e)
        {
            // Filtrar en memoria según lo ingresado
            string criterio = txtBuscar.Text.Trim().ToLower();
            var filtrados = PlatilloCln.listar()
                .Where(p =>
                    p.nombre.ToLower().Contains(criterio) ||
                    p.descripcion.ToLower().Contains(criterio)
                )
                .ToList();

            dgvLista.DataSource = filtrados;
            btnEditar.Enabled = filtrados.Count > 0;
            btnEliminar.Enabled = filtrados.Count > 0;
        }

        private void btnNuevo_Click(object sender, EventArgs e)
        {
            esNuevo = true;
            this.Size = new Size(835, 487);
            limpiar();
            txtNombre.Focus();
        }

        private void btnEditar_Click(object sender, EventArgs e)
        {
            if (dgvLista.CurrentRow == null)
            {
                MessageBox.Show(
                    "Seleccione un platillo para editar",
                    "::: PIZZERIA - AVISO :::",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning
                );
                return;
            }

            esNuevo = false;
            this.Size = new Size(835, 487);

            idPlatillo = Convert.ToInt32(dgvLista.CurrentRow.Cells["platillo_id"].Value);
            var p = PlatilloCln.obtenerUno(idPlatillo);

            txtNombre.Text = p.nombre;
            txtDescripcion.Text = p.descripcion;
            txtPrecio.Text = p.precio.ToString();
            txtTiempo.Text = p.tiempo_preparacion.ToString();
            txtImagen.Text = p.imagen_url;
            cboCategoria.SelectedValue = p.categoria_id;
            chkEstado.Checked = p.estado;

            txtNombre.Focus();
        }

        private void btnEliminar_Click(object sender, EventArgs e)
        {
            if (idPlatillo == 0)
            {
                MessageBox.Show(
                    "Seleccione un platillo para eliminar",
                    "::: PIZZERIA - AVISO :::",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning
                );
                return;
            }

            if (MessageBox.Show(
                    $"¿Está seguro de eliminar el platillo {txtNombre.Text}?",
                    "::: PIZZERIA - AVISO :::",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question
                ) == DialogResult.Yes)
            {
                PlatilloCln.eliminar(idPlatillo);
                listar();
                limpiar();
            }
        }

        private void btnGuardar_Click(object sender, EventArgs e)
        {
            if (!validar()) return;

            var platillo = new PLATILLO
            {
                nombre = txtNombre.Text.Trim(),
                descripcion = txtDescripcion.Text.Trim(),
                precio = Convert.ToDecimal(txtPrecio.Text.Trim()),
                tiempo_preparacion = Convert.ToInt32(txtTiempo.Text.Trim()),
                imagen_url = txtImagen.Text.Trim(),
                categoria_id = Convert.ToInt32(cboCategoria.SelectedValue),
                estado = chkEstado.Checked
            };

            if (esNuevo)
            {
                PlatilloCln.insertar(platillo);
            }
            else
            {
                platillo.platillo_id = idPlatillo;
                PlatilloCln.actualizar(platillo);
            }

            listar();
            this.Size = new Size(835, 362);
            limpiar();
            MessageBox.Show(
                "Platillo guardado correctamente",
                "::: PIZZERIA - AVISO :::",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information
            );
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            this.Size = new Size(835, 362);
            limpiar();
        }

        private void dgvLista_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;
            idPlatillo = Convert.ToInt32(dgvLista.Rows[e.RowIndex].Cells["platillo_id"].Value);
        }

        private void lblTitulo_Click(object sender, EventArgs e)
        {

        }

        private void gbxLista_Enter(object sender, EventArgs e)
        {

        }
    }
}
