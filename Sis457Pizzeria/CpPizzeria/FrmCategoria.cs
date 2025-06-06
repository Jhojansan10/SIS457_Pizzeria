using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ClnPizzeria;

namespace CpPizzeria
{
    public partial class FrmCategoria : Form
    {
        private bool esNuevo = false;
        private int idCategoria = 0;

        public FrmCategoria()
        {
            InitializeComponent();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void FrmCategoria_Load(object sender, EventArgs e)
        {
            listar();
        }

        private void limpiar()
        {
            idCategoria = 0;
            txtNombre.Clear();
            txtDescripcion.Clear();
            chkEstado.Checked = true;
        }

        private bool validar()
        {
            bool esValido = true;
            erpNombre.SetError(txtNombre, "");

            if (string.IsNullOrWhiteSpace(txtNombre.Text))
            {
                erpNombre.SetError(txtNombre, "El nombre es obligatorio");
                esValido = false;
            }

            return esValido;
        }

        private void listar()
        {
            var categorias = CategoriaCln.listar();
            dgvLista.DataSource = categorias;
        }


        private void btnBuscar_Click(object sender, EventArgs e)
        {
            string criterio = txtBuscar.Text.Trim().ToLower();
            var categorias = CategoriaCln.listar()
                .Where(c => c.nombre.ToLower().Contains(criterio) ||
                            c.descripcion.ToLower().Contains(criterio))
                .ToList();

            dgvLista.DataSource = categorias;
        }

        private void dgvLista_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                idCategoria = Convert.ToInt32(dgvLista.Rows[e.RowIndex].Cells["categoria_id"].Value);
                txtNombre.Text = dgvLista.Rows[e.RowIndex].Cells["nombre"].Value.ToString();
                txtDescripcion.Text = dgvLista.Rows[e.RowIndex].Cells["descripcion"].Value.ToString();
                chkEstado.Checked = Convert.ToBoolean(dgvLista.Rows[e.RowIndex].Cells["estado"].Value);
            }
        }

        private void btnNuevo_Click(object sender, EventArgs e)
        {
            limpiar();
            esNuevo = true;
            txtNombre.Focus();
        }

        private void btnEditar_Click(object sender, EventArgs e)
        {
            if (idCategoria == 0)
            {
                MessageBox.Show("Seleccione una categoría para editar", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            esNuevo = false;
        }

        private void btnEliminar_Click(object sender, EventArgs e)
        {
            if (idCategoria == 0)
            {
                MessageBox.Show("Seleccione una categoría para eliminar", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (MessageBox.Show("¿Está seguro de eliminar esta categoría?", "::: PIZZERIA :::", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                CategoriaCln.eliminar(idCategoria);
                listar();
                limpiar();
            }
        }

        private void btnGuardar_Click(object sender, EventArgs e)
        {
            if (!validar()) return;

            var categoria = new CadPizzeria.CATEGORIA
            {
                nombre = txtNombre.Text.Trim(),
                descripcion = txtDescripcion.Text.Trim(),
                estado = chkEstado.Checked
            };

            if (esNuevo)
            {
                CategoriaCln.insertar(categoria);
            }
            else
            {
                categoria.categoria_id = idCategoria;
                CategoriaCln.actualizar(categoria);
            }

            listar();
            limpiar();
            esNuevo = false;
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {

        }

        private void txtDescripcion_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
