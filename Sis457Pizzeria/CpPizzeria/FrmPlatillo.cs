using System;
using System.Data;
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

        private void listar()
        {
            dgvLista.DataSource = PlatilloCln.listar();
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

            if (string.IsNullOrWhiteSpace(txtPrecio.Text))
            {
                erpPrecio.SetError(txtPrecio, "El precio es obligatorio");
                esValido = false;
            }

            if (cboCategoria.SelectedIndex == -1)
            {
                erpCategoria.SetError(cboCategoria, "Seleccione una categoría");
                esValido = false;
            }

            return esValido;
        }


        private void FrmPlatillo_Load_1(object sender, EventArgs e)
        {
            cargarCategorias();
            listar();
        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void btnBuscar_Click(object sender, EventArgs e)
        {
            string criterio = txtBuscar.Text.Trim().ToLower();

            var platillos = PlatilloCln.listar()
                .Where(p => p.nombre.ToLower().Contains(criterio) ||
                            p.descripcion.ToLower().Contains(criterio))
                .ToList();

            dgvLista.DataSource = platillos;
        }

        private void btnNuevo_Click_1(object sender, EventArgs e)
        {
            limpiar();
            esNuevo = true;
            txtNombre.Focus();
        }

        private void btnEditar_Click_1(object sender, EventArgs e)
        {
            if (idPlatillo == 0)
            {
                MessageBox.Show("Seleccione un platillo para editar", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            esNuevo = false;
        }

        private void btnEliminar_Click_1(object sender, EventArgs e)
        {
            if (idPlatillo == 0)
            {
                MessageBox.Show("Seleccione un platillo para eliminar", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (MessageBox.Show("¿Está seguro de eliminar este platillo?", "::: PIZZERIA :::",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                PlatilloCln.eliminar(idPlatillo);
                listar();
                limpiar();
            }
        }

        private void btnGuardar_Click_1(object sender, EventArgs e)
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
                PlatilloCln.insertar(platillo);
            else
            {
                platillo.platillo_id = idPlatillo;
                PlatilloCln.actualizar(platillo);
            }

            listar();
            limpiar();
            esNuevo = false;
        }

        private void btnCancelar_Click_1(object sender, EventArgs e)
        {

        }

        private void dgvLista_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                idPlatillo = Convert.ToInt32(dgvLista.Rows[e.RowIndex].Cells["platillo_id"].Value);
                txtNombre.Text = dgvLista.Rows[e.RowIndex].Cells["nombre"].Value.ToString();
                txtDescripcion.Text = dgvLista.Rows[e.RowIndex].Cells["descripcion"].Value.ToString();
                txtPrecio.Text = dgvLista.Rows[e.RowIndex].Cells["precio"].Value.ToString();
                txtTiempo.Text = dgvLista.Rows[e.RowIndex].Cells["tiempo_preparacion"].Value.ToString();
                txtImagen.Text = dgvLista.Rows[e.RowIndex].Cells["imagen_url"].Value.ToString();
                cboCategoria.Text = dgvLista.Rows[e.RowIndex].Cells["categoria"].Value.ToString();
                chkEstado.Checked = Convert.ToBoolean(dgvLista.Rows[e.RowIndex].Cells["estado"].Value);
            }
        }
    }
}
