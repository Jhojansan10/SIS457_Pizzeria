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
    public partial class FrmUsuario : Form
    {
        private bool esNuevo = false;
        private int idUsuario = 0;

        public FrmUsuario()
        {
            InitializeComponent();
        }

        private void gbxDatos_Enter(object sender, EventArgs e)
        {

        }

        private void FrmUsuario_Load(object sender, EventArgs e)
        {
            listar();
            cargarRoles();
        }

        private void limpiar()
        {
            idUsuario = 0;
            txtNombre.Clear();
            txtEmail.Clear();
            txtContrasena.Clear();
            cboRol.SelectedIndex = -1;
            chkEstado.Checked = true;
        }

        private void cargarRoles()
        {
            cboRol.Items.Clear();
            cboRol.Items.Add("Administrador");
            cboRol.Items.Add("Repartidor");
            cboRol.Items.Add("Cliente");
            cboRol.SelectedIndex = -1;
        }

        private void listar()
        {
            var usuarios = UsuarioCln.listar();
            dgvLista.DataSource = usuarios;
        }

        private void dgvLista_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                idUsuario = Convert.ToInt32(dgvLista.Rows[e.RowIndex].Cells["usuario_id"].Value);
                txtNombre.Text = dgvLista.Rows[e.RowIndex].Cells["nombre"].Value.ToString();
                txtEmail.Text = dgvLista.Rows[e.RowIndex].Cells["email"].Value.ToString();
                txtContrasena.Text = dgvLista.Rows[e.RowIndex].Cells["contraseña"].Value.ToString();
                cboRol.Text = dgvLista.Rows[e.RowIndex].Cells["rol"].Value.ToString();
                chkEstado.Checked = Convert.ToBoolean(dgvLista.Rows[e.RowIndex].Cells["estado"].Value);
                dgvLista.Columns["contraseña"].Visible = false;
            }
        }

        private bool validar()
        {
            bool esValido = true;
            erpNombre.SetError(txtNombre, "");
            erpEmail.SetError(txtEmail, "");
            erpContrasena.SetError(txtContrasena, "");
            erpRol.SetError(cboRol, "");

            if (string.IsNullOrWhiteSpace(txtNombre.Text))
            {
                erpNombre.SetError(txtNombre, "El nombre es obligatorio");
                esValido = false;
            }

            if (string.IsNullOrWhiteSpace(txtEmail.Text))
            {
                erpEmail.SetError(txtEmail, "El email es obligatorio");
                esValido = false;
            }

            if (string.IsNullOrWhiteSpace(txtContrasena.Text))
            {
                erpContrasena.SetError(txtContrasena, "La contraseña es obligatoria");
                esValido = false;
            }

            if (cboRol.SelectedIndex == -1)
            {
                erpRol.SetError(cboRol, "Debe seleccionar un rol");
                esValido = false;
            }

            return esValido;
        }

        private void btnGuardar_Click(object sender, EventArgs e)
        {
            if (!validar()) return;

            var usuario = new CadPizzeria.USUARIO
            {
                nombre = txtNombre.Text.Trim(),
                email = txtEmail.Text.Trim(),
                contraseña = Util.Encrypt(txtContrasena.Text.Trim()),
                rol = cboRol.Text,
                estado = chkEstado.Checked,
                fecha_registro = DateTime.Now
            };

            if (esNuevo)
            {
                UsuarioCln.insertar(usuario);
            }
            else
            {
                usuario.usuario_id = idUsuario;
                UsuarioCln.actualizar(usuario);
            }

            listar();
            limpiar();
            esNuevo = false;
        }

        private void btnNuevo_Click_1(object sender, EventArgs e)
        {
            limpiar();
            esNuevo = true;
            txtNombre.Focus();
        }

        private void btnEditar_Click_1(object sender, EventArgs e)
        {
            if (idUsuario == 0)
            {
                MessageBox.Show("Seleccione un usuario para editar", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            esNuevo = false;
        }

        private void btnEliminar_Click_1(object sender, EventArgs e)
        {
            if (idUsuario == 0)
            {
                MessageBox.Show("Seleccione un usuario para eliminar", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (MessageBox.Show("¿Está seguro de eliminar este usuario?", "::: PIZZERIA :::", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                UsuarioCln.eliminar(idUsuario);
                listar();
                limpiar();
            }
        }

        private void btnBuscar_Click_1(object sender, EventArgs e)
        {
            string criterio = txtBuscar.Text.Trim().ToLower();
            var usuarios = UsuarioCln.listar().Where(u =>
                u.nombre.ToLower().Contains(criterio) ||
                u.email.ToLower().Contains(criterio)
            ).ToList();

            dgvLista.DataSource = usuarios;
        }

    }
}
