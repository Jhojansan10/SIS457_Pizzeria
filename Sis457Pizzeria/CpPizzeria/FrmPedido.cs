using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using CadPizzeria;

namespace CpPizzeria
{
    public partial class FrmPedido : Form
    {
        private bool esNuevo = false;
        private int idPedido = 0;
        private decimal total = 0;
        private List<DETALLE_PEDIDO> detalles = new List<DETALLE_PEDIDO>();

        public FrmPedido()
        {
            InitializeComponent();
        }

        private void FrmPedido_Load(object sender, EventArgs e)
        {
            cargarUsuarios();
            cargarPlatillos();
            listar();
        }

        private void cargarUsuarios()
        {
            using (var db = new LabPizzeriaEntities())
            {
                var usuarios = db.USUARIO
                    .Where(u => u.estado == true && u.rol == "Cliente")
                    .Select(u => new { u.usuario_id, u.nombre })
                    .ToList();

                cboCliente.DataSource = usuarios;
                cboCliente.DisplayMember = "nombre";
                cboCliente.ValueMember = "usuario_id";
                cboCliente.SelectedIndex = -1;
            }
        }

        private void cargarDirecciones()
        {
            if (cboCliente.SelectedIndex == -1) return;

            using (var db = new LabPizzeriaEntities())
            {
                var direcciones = db.DIRECCION
                    .Where(d => d.usuario_id == Convert.ToInt32(cboCliente.SelectedValue))
                    .Select(d => new { d.direccion_id, d.calle })
                    .ToList();

                cboDireccion.DataSource = direcciones;
                cboDireccion.DisplayMember = "calle";
                cboDireccion.ValueMember = "direccion_id";
                cboDireccion.SelectedIndex = -1;
            }
        }

        private void cargarPlatillos()
        {
            using (var db = new LabPizzeriaEntities())
            {
                var platillos = db.PLATILLO
                    .Where(p => p.estado == true)
                    .Select(p => new { p.platillo_id, p.nombre, p.precio })
                    .ToList();

                cboPlatillo.DataSource = platillos;
                cboPlatillo.DisplayMember = "nombre";
                cboPlatillo.ValueMember = "platillo_id";
                cboPlatillo.SelectedIndex = -1;
            }
        }

        private void agregarDetalle()
        {
            if (cboPlatillo.SelectedIndex == -1 || string.IsNullOrWhiteSpace(txtCantidad.Text)) return;

            int platilloId = Convert.ToInt32(cboPlatillo.SelectedValue);
            int cantidad = Convert.ToInt32(txtCantidad.Text);

            using (var db = new LabPizzeriaEntities())
            {
                var platillo = db.PLATILLO.Find(platilloId);

                var existente = detalles.FirstOrDefault(d => d.platillo_id == platilloId);
                if (existente != null)
                {
                    existente.cantidad += cantidad;
                }
                else
                {
                    detalles.Add(new DETALLE_PEDIDO
                    {
                        platillo_id = platilloId,
                        cantidad = cantidad,
                        PLATILLO = platillo
                    });
                }
            }

            actualizarDetalle();
            txtCantidad.Clear();
            cboPlatillo.SelectedIndex = -1;
        }

        private void actualizarDetalle()
        {
            var detalleMostrar = detalles.Select(d => new
            {
                Platillo = d.PLATILLO.nombre,
                d.cantidad,
                Subtotal = d.cantidad * d.PLATILLO.precio
            }).ToList();

            dgvLista.DataSource = null;
            dgvLista.DataSource = detalleMostrar;

            total = detalles.Sum(d => d.cantidad * d.PLATILLO.precio);
            lblTotal.Text = $"Bs. {total:N2}";
        }

        private void btnAgregar_Click(object sender, EventArgs e)
        {
            agregarDetalle();
        }

        private void btnNuevo_Click(object sender, EventArgs e)
        {
            limpiar();
            esNuevo = true;
            cboCliente.Focus();
        }

        private void btnEliminar_Click(object sender, EventArgs e)
        {
            if (idPedido == 0)
            {
                MessageBox.Show("Seleccione un pedido para eliminar", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (MessageBox.Show("¿Está seguro de eliminar este pedido?", "::: PIZZERIA :::", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                using (var db = new LabPizzeriaEntities())
                {
                    var pedido = db.PEDIDO.Find(idPedido);
                    pedido.estado = chkEntregado.Checked ? "Entregado" : "Pendiente";
                    pedido.estado_registro = true;
                    db.SaveChanges();
                }

                listar();
                limpiar();
            }
        }

        private bool validar()
        {
            bool esValido = true;

            if (cboCliente.SelectedIndex == -1)
                esValido = false;

            if (cboDireccion.SelectedIndex == -1)
                esValido = false;

            if (detalles.Count == 0)
                esValido = false;

            return esValido;
        }

        private void btnGuardar_Click(object sender, EventArgs e)
        {
            if (!validar())
            {
                MessageBox.Show("Complete todos los campos requeridos", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var pedido = new PEDIDO
            {
                usuario_id = Convert.ToInt32(cboCliente.SelectedValue),
                direccion_id = Convert.ToInt32(cboDireccion.SelectedValue),
                fecha = DateTime.Now,
                total = total,
                estado = chkEntregado.Checked ? "Entregado" : "Pendiente",
                estado_registro = true
            };

            ClnPizzeria.PedidoCln.insertar(pedido, detalles);
            listar();
            limpiar();
        }

        private void limpiar()
        {
            cboCliente.SelectedIndex = -1;
            cboDireccion.DataSource = null;
            cboPlatillo.SelectedIndex = -1;
            txtCantidad.Clear();
            detalles.Clear();
            dgvLista.DataSource = null;
            lblTotal.Text = "Bs. 0.00";
            total = 0;
        }

        private void listar()
        {
            using (var db = new LabPizzeriaEntities())
            {
                var pedidos = db.PEDIDO
                    .Where(p => p.estado_registro == true)
                    .OrderByDescending(p => p.fecha)
                    .Select(p => new
                    {
                        p.pedido_id,
                        Cliente = p.USUARIO.nombre,
                        Direccion = p.DIRECCION.calle,
                        Fecha = p.fecha,
                        Entregado = p.estado,
                        Total = p.total
                    })
                    .ToList();

                dgvLista.DataSource = pedidos;
            }
        }

        private void cboCliente_SelectedIndexChanged(object sender, EventArgs e)
        {
            cargarDirecciones();
        }

        private void btnBuscar_Click(object sender, EventArgs e)
        {
            string criterio = txtBuscar.Text.Trim().ToLower();
            using (var db = new LabPizzeriaEntities())
            {
                var pedidos = db.PEDIDO
    .Where(p =>
        p.estado_registro == true &&
        (
            p.pedido_id.ToString().Contains(criterio) ||
            p.USUARIO.nombre.ToLower().Contains(criterio) ||
            p.estado.ToLower().Contains(criterio)
        )
    )
    .OrderByDescending(p => p.fecha)
    .Select(p => new
    {
        p.pedido_id,
        Cliente = p.USUARIO.nombre,
        Direccion = p.DIRECCION.calle,
        Fecha = p.fecha,
        Entregado = p.estado,
        Total = p.total
    })
    .ToList();

                dgvLista.DataSource = pedidos;
            }
        }

        private void cboPlatillo_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}