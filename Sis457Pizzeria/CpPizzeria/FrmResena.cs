using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using CadPizzeria;

namespace CpPizzeria
{
    public partial class FrmResena : Form
    {
        private bool esNuevo = false;
        private int idResena = 0;

        public FrmResena()
        {
            InitializeComponent();
        }

        private void FrmResena_Load(object sender, EventArgs e)
        {
            cargarClientes();
            cargarPedidos();
            cargarCalificaciones();
            listar();
        }

        private void cargarClientes()
        {
            using (var db = new LabPizzeriaEntities())
            {
                var clientes = db.USUARIO
                    .Where(u => u.estado == true && u.rol == "Cliente")
                    .Select(u => new { u.usuario_id, u.nombre })
                    .ToList();

                cboCliente.DataSource = clientes;
                cboCliente.DisplayMember = "nombre";
                cboCliente.ValueMember = "usuario_id";
                cboCliente.SelectedIndex = -1;
            }
        }

        private void cargarPedidos()
        {
            using (var db = new LabPizzeriaEntities())
            {
                var pedidos = db.PEDIDO
                    .Where(p => p.estado_registro == true)
                    .Select(p => new { p.pedido_id })
                    .ToList();

                cboPedido.DataSource = pedidos;
                cboPedido.DisplayMember = "pedido_id";
                cboPedido.ValueMember = "pedido_id";
                cboPedido.SelectedIndex = -1;
            }
        }

        private void cargarCalificaciones()
        {
            cboCalificacion.Items.Clear();
            for (int i = 1; i <= 5; i++)
                cboCalificacion.Items.Add(i);
            cboCalificacion.SelectedIndex = -1;
        }

        private void listar()
        {
            using (var db = new LabPizzeriaEntities())
            {
                var resenas = db.RESENA
                    .Where(r => r.estado_registro == true)
                    .Select(r => new
                    {
                        r.resena_id,
                        Cliente = r.USUARIO.nombre,
                        Pedido = r.pedido_id,
                        Calificación = r.calificacion,
                        Comentario = r.comentario,
                        Fecha = r.fecha
                    })
                    .OrderByDescending(r => r.Fecha)
                    .ToList();

                dgvResenas.DataSource = resenas;
            }
        }

        private void btnBuscar_Click(object sender, EventArgs e)
        {
            string criterio = txtBuscar.Text.Trim().ToLower();

            using (var db = new LabPizzeriaEntities())
            {
                var resenas = db.RESENA
                    .Where(r => r.estado_registro == true &&
                        (r.USUARIO.nombre.ToLower().Contains(criterio) ||
                         r.comentario.ToLower().Contains(criterio) ||
                         r.calificacion.ToString().Contains(criterio)))
                    .OrderByDescending(r => r.fecha)
                    .Select(r => new
                    {
                        r.resena_id,
                        Cliente = r.USUARIO.nombre,
                        Pedido = r.PEDIDO.pedido_id,
                        r.calificacion,
                        r.comentario,
                        r.fecha
                    })
                    .ToList();

                dgvResenas.DataSource = resenas;
            }
        }

        private void btnNuevo_Click(object sender, EventArgs e)
        {
            limpiarFormulario();
            esNuevo = true;
            cboCliente.Focus();
        }

        private void btnEditar_Click(object sender, EventArgs e)
        {
            if (idResena == 0)
            {
                MessageBox.Show("Seleccione una reseña para editar.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            esNuevo = false;
        }

        private void btnEliminar_Click(object sender, EventArgs e)
        {
            if (idResena == 0)
            {
                MessageBox.Show("Seleccione una reseña para eliminar.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (MessageBox.Show("¿Está seguro de eliminar esta reseña?", "::: PIZZERIA :::",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                using (var db = new LabPizzeriaEntities())
                {
                    var resena = db.RESENA.Find(idResena);
                    resena.estado_registro = false;
                    db.SaveChanges();
                }

                MessageBox.Show("Reseña eliminada correctamente", "::: PIZZERIA :::", MessageBoxButtons.OK, MessageBoxIcon.Information);
                listar();
                limpiarFormulario();
            }
        }

        private void btnGuardar_Click(object sender, EventArgs e)
        {
            if (cboCliente.SelectedIndex == -1 || cboPedido.SelectedIndex == -1 ||
        cboCalificacion.SelectedIndex == -1 || string.IsNullOrWhiteSpace(txtComentario.Text))
            {
                MessageBox.Show("Complete todos los campos obligatorios.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var resena = new RESENA
            {
                usuario_id = Convert.ToInt32(cboCliente.SelectedValue),
                pedido_id = Convert.ToInt32(cboPedido.SelectedValue),
                calificacion = Convert.ToByte(cboCalificacion.SelectedItem),
                comentario = txtComentario.Text.Trim(),
                fecha = dtpFecha.Value,
                estado_registro = true
            };

            using (var db = new LabPizzeriaEntities())
            {
                if (esNuevo)
                {
                    db.RESENA.Add(resena);
                }
                else
                {
                    resena.resena_id = idResena;
                    var original = db.RESENA.Find(idResena);
                    original.usuario_id = resena.usuario_id;
                    original.pedido_id = resena.pedido_id;
                    original.calificacion = resena.calificacion;
                    original.comentario = resena.comentario;
                    original.fecha = resena.fecha;
                }

                db.SaveChanges();
            }

            MessageBox.Show("Reseña guardada correctamente.", "::: PIZZERIA :::", MessageBoxButtons.OK, MessageBoxIcon.Information);
            listar();
            limpiarFormulario();
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            limpiarFormulario();
        }

        private void dgvResenas_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                idResena = Convert.ToInt32(dgvResenas.Rows[e.RowIndex].Cells["resena_id"].Value);
                cboCliente.Text = dgvResenas.Rows[e.RowIndex].Cells["Cliente"].Value.ToString();
                cboPedido.Text = dgvResenas.Rows[e.RowIndex].Cells["Pedido"].Value.ToString();
                cboCalificacion.Text = dgvResenas.Rows[e.RowIndex].Cells["Calificación"].Value.ToString();
                txtComentario.Text = dgvResenas.Rows[e.RowIndex].Cells["Comentario"].Value.ToString();
                dtpFecha.Value = Convert.ToDateTime(dgvResenas.Rows[e.RowIndex].Cells["Fecha"].Value);
            }
        }

        private void limpiarFormulario()
        {
            idResena = 0;
            esNuevo = false;
            cboCliente.SelectedIndex = -1;
            cboPedido.SelectedIndex = -1;
            cboCalificacion.SelectedIndex = -1;
            txtComentario.Clear();
            dtpFecha.Value = DateTime.Today;
        }

        private void txtComentario_TextChanged(object sender, EventArgs e)
        {

        }

        private void dtpFecha_ValueChanged(object sender, EventArgs e)
        {

        }
    }
}
