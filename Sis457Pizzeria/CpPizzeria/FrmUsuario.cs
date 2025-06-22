using System;
using System.Data.Entity.Infrastructure;
using System.Data.SqlClient;
using System.Linq;
using System.Windows.Forms;
using CadPizzeria;
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

            // Validación en tiempo real de CI
            txtCI.TextChanged += TxtCI_TextChanged;
            // Botón Cancelar limpia el formulario
            btnCancelar.Click   += BtnCancelar_Click;
        }

        private void FrmUsuario_Load(object sender, EventArgs e)
        {
            dtpFechaRegistro.Value = DateTime.Now;
            CargarRoles();
            Listar();
        }

        private void CargarRoles()
        {
            cboRol.Items.Clear();
            cboRol.Items.AddRange(new string[] {
                "Administrador",
                "Cliente",
                "Repartidor"
            });
            cboRol.SelectedIndex = -1;
        }

        private void Listar()
        {
            var lista = UsuarioCln.listar();
            dgvLista.DataSource = lista;

            // Ocultar contraseña
            if (dgvLista.Columns["contraseña"] != null)
                dgvLista.Columns["contraseña"].Visible = false;

            // Ocultar el ID de usuario
            if (dgvLista.Columns["usuario_id"] != null)
                dgvLista.Columns["usuario_id"].Visible = false;
        }

        private void Limpiar()
        {
            idUsuario               = 0;
            esNuevo                 = false;
            txtCI.Clear();
            txtNombre.Clear();
            txtEmail.Clear();
            txtContrasena.Clear();
            txtDireccion.Clear();
            cboRol.SelectedIndex    = -1;
            chkEstado.Checked       = true;
            dtpFechaRegistro.Value  = DateTime.Now;

            erpCI.Clear();
            erpNombre.Clear();
            erpEmail.Clear();
            erpContrasena.Clear();
            erpDireccion.Clear();
            erpRol.Clear();
        }

        private bool Validar()
        {
            bool ok = true;
            erpCI.Clear();
            erpNombre.Clear();
            erpEmail.Clear();
            erpContrasena.Clear();
            erpDireccion.Clear();
            erpRol.Clear();

            var rol = cboRol.Text.Trim();

            if (string.IsNullOrWhiteSpace(txtCI.Text))
            {
                erpCI.SetError(txtCI, "El CI es obligatorio");
                ok = false;
            }

            if (string.IsNullOrWhiteSpace(txtNombre.Text))
            {
                erpNombre.SetError(txtNombre, "El nombre es obligatorio");
                ok = false;
            }

            switch (rol)
            {
                case "Administrador":
                case "Empleado":
                    if (string.IsNullOrWhiteSpace(txtEmail.Text))
                    {
                        erpEmail.SetError(txtEmail, "El email es obligatorio");
                        ok = false;
                    }
                    if (string.IsNullOrWhiteSpace(txtContrasena.Text))
                    {
                        erpContrasena.SetError(txtContrasena, "La contraseña es obligatoria");
                        ok = false;
                    }
                    if (string.IsNullOrWhiteSpace(txtDireccion.Text))
                    {
                        erpDireccion.SetError(txtDireccion, "La dirección es obligatoria");
                        ok = false;
                    }
                    break;

                case "Repartidor":
                    if (string.IsNullOrWhiteSpace(txtDireccion.Text))
                    {
                        erpDireccion.SetError(txtDireccion, "La dirección es obligatoria");
                        ok = false;
                    }
                    break;

                case "Cliente":
                    // solo CI y nombre
                    break;

                default:
                    erpRol.SetError(cboRol, "Seleccione un rol");
                    ok = false;
                    break;
            }

            return ok;
        }

        private void TxtCI_TextChanged(object sender, EventArgs e)
        {
            var ci = txtCI.Text.Trim();
            if (string.IsNullOrEmpty(ci)) return;

            bool dup = esNuevo
                ? UsuarioCln.ExisteCi(ci)
                : UsuarioCln.ExisteCi(ci, idUsuario);

            if (dup)
                erpCI.SetError(txtCI, "Ya existe un usuario con ese CI");
            else
                erpCI.Clear();
        }

        private void btnNuevo_Click_1(object sender, EventArgs e)
        {
            Limpiar();
            esNuevo = true;
            txtCI.Focus();
        }

        private void btnEditar_Click_1(object sender, EventArgs e)
        {
            if (idUsuario == 0)
            {
                MessageBox.Show("Seleccione un usuario para editar",
                    "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            esNuevo = false;
        }

        private void dgvLista_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;
            var row = dgvLista.Rows[e.RowIndex];
            idUsuario = Convert.ToInt32(row.Cells["usuario_id"].Value);
            txtCI.Text           = row.Cells["ci"].Value?.ToString()      ?? "";
            txtNombre.Text       = row.Cells["nombre"].Value.ToString();
            txtEmail.Text        = row.Cells["email"].Value?.ToString()   ?? "";
            txtContrasena.Text   = ""; // nunca recuperamos el hash
            txtDireccion.Text    = row.Cells["direccion"].Value?.ToString() ?? "";
            cboRol.Text          = row.Cells["rol"].Value.ToString();
            chkEstado.Checked    = Convert.ToBoolean(row.Cells["estado"].Value);
            dtpFechaRegistro.Value = Convert.ToDateTime(row.Cells["fecha_registro"].Value);
        }

        private void btnGuardar_Click(object sender, EventArgs e)
        {
            if (!Validar()) return;

            string ciVal = txtCI.Text.Trim();

            // Verifico duplicado antes de guardar
            if (esNuevo && UsuarioCln.ExisteCi(ciVal))
            {
                MessageBox.Show("Ya existe un usuario con ese CI.",
                    "::: PIZZERIA - ERROR :::", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtCI.Focus();
                return;
            }
            if (!esNuevo && UsuarioCln.ExisteCi(ciVal, idUsuario))
            {
                MessageBox.Show("Ya existe otro usuario con ese CI.",
                    "::: PIZZERIA - ERROR :::", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtCI.Focus();
                return;
            }

            var u = new USUARIO
            {
                ci              = ciVal,
                nombre          = txtNombre.Text.Trim(),
                email           = txtEmail.Text.Trim(),
                contraseña      = string.IsNullOrWhiteSpace(txtContrasena.Text)
                                  ? ""
                                  : Util.Encrypt(txtContrasena.Text.Trim()),
                direccion       = txtDireccion.Text.Trim(),
                rol             = cboRol.Text,
                estado          = chkEstado.Checked,
                fecha_registro  = DateTime.Now
            };

            try
            {
                if (esNuevo)
                    UsuarioCln.insertar(u);
                else
                {
                    u.usuario_id = idUsuario;
                    UsuarioCln.actualizar(u);
                }

                MessageBox.Show("Usuario guardado correctamente",
                    "::: PIZZERIA - AVISO :::", MessageBoxButtons.OK, MessageBoxIcon.Information);

                Listar();
                Limpiar();
            }
            catch (DbUpdateException dbEx)
            {
                var sqlEx = dbEx.GetBaseException() as SqlException;
                MessageBox.Show(sqlEx?.Message ?? dbEx.Message,
                    "::: PIZZERIA - ERROR :::", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.GetBaseException().Message,
                    "::: PIZZERIA - ERROR :::", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnCancelar_Click(object sender, EventArgs e)
        {
            Limpiar();
        }

        private void btnEliminar_Click_1(object sender, EventArgs e)
        {
            if (idUsuario == 0)
            {
                MessageBox.Show("Seleccione un usuario para eliminar",
                    "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (MessageBox.Show("¿Eliminar este usuario?", "::: PIZZERIA :::",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                UsuarioCln.eliminar(idUsuario);
                Listar();
                Limpiar();
            }
        }

        private void btnBuscar_Click_1(object sender, EventArgs e)
        {
            string criterio = txtBuscar.Text.Trim().ToLower();
            var resultados = UsuarioCln.listar()
                .Where(u =>
                    u.ci.ToLower().Contains(criterio) ||
                    u.nombre.ToLower().Contains(criterio) ||
                    u.email.ToLower().Contains(criterio))
                .ToList();
            dgvLista.DataSource = resultados;
        }
    }
}

