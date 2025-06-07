using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using CadPizzeria;
using ClnPizzeria;

namespace CpPizzeria
{
    public partial class FrmPedido : Form
    {
        private bool esNuevo = false;
        private int idPedido = 0;
        private decimal total = 0m;
        private List<DETALLE_PEDIDO> detalles = new List<DETALLE_PEDIDO>();

        public FrmPedido()
        {
            InitializeComponent();
        }

        private void FrmPedido_Load(object sender, EventArgs e)
        {
            // 1) Carga inicial de datos
            cargarUsuarios();
            cargarPlatillos();
            listar();

            // 2) Suscripción al cambio de cliente
            cboCliente.SelectedIndexChanged += cboCliente_SelectedIndexChanged;

            // 3) Arranca sin nada seleccionado (igual que platillos)
            cboCliente.SelectedIndex = -1;
        }

        private void cargarUsuarios()
        {
            using (var db = new LabPizzeriaEntities())
            {
                var usuarios = db.USUARIO
                    .Where(u => u.estado && u.rol == "Cliente")
                    .Select(u => new { u.usuario_id, u.nombre })
                    .ToList();

                cboCliente.DisplayMember = "nombre";
                cboCliente.ValueMember = "usuario_id";
                cboCliente.DataSource = usuarios;
            }
        }

        private void cboCliente_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cboCliente.SelectedIndex == -1)
            {
                cboDireccion.DataSource = null;
                return;
            }

            int usuarioId = (int)cboCliente.SelectedValue;
            cargarDirecciones(usuarioId);
        }

        private void cargarDirecciones(int usuarioId)
        {
            using (var db = new LabPizzeriaEntities())
            {
                var lista = db.DIRECCION
                    .Where(d => d.estado && d.usuario_id == usuarioId)
                    .Select(d => new { d.direccion_id, d.calle })
                    .ToList();

                cboDireccion.DisplayMember = "calle";
                cboDireccion.ValueMember = "direccion_id";
                cboDireccion.DataSource = lista;
                cboDireccion.SelectedIndex = -1;
            }
        }

        private void cargarPlatillos()
        {
            using (var db = new LabPizzeriaEntities())
            {
                var platillos = db.PLATILLO
                    .Where(p => p.estado)
                    .Select(p => new { p.platillo_id, p.nombre, p.precio })
                    .ToList();

                cboPlatillo.DisplayMember = "nombre";
                cboPlatillo.ValueMember = "platillo_id";
                cboPlatillo.DataSource = platillos;
                cboPlatillo.SelectedIndex = -1;
            }
        }

        private void btnAgregar_Click(object sender, EventArgs e)
        {
            if (cboPlatillo.SelectedIndex == -1 ||
                string.IsNullOrWhiteSpace(txtCantidad.Text)) return;

            int platilloId = Convert.ToInt32(cboPlatillo.SelectedValue);
            int cantidad = Convert.ToInt32(txtCantidad.Text);

            using (var db = new LabPizzeriaEntities())
            {
                var platillo = db.PLATILLO.Find(platilloId);
                var existente = detalles.FirstOrDefault(d => d.platillo_id == platilloId);

                if (existente != null)
                    existente.cantidad += cantidad;
                else
                    detalles.Add(new DETALLE_PEDIDO
                    {
                        platillo_id = platilloId,
                        cantidad = cantidad,
                        PLATILLO = platillo
                    });
            }

            actualizarDetalle();
            txtCantidad.Clear();
            cboPlatillo.SelectedIndex = -1;
        }

        private void actualizarDetalle()
        {
            var mostrado = detalles.Select(d => new
            {
                Platillo = d.PLATILLO.nombre,
                d.cantidad,
                Subtotal = d.cantidad * d.PLATILLO.precio
            }).ToList();

            dgvLista.DataSource = mostrado;
            total = detalles.Sum(d => d.cantidad * d.PLATILLO.precio);
            lblTotal.Text = $"Bs. {total:N2}";
        }

        private void btnNuevo_Click(object sender, EventArgs e)
        {
            limpiar();
            esNuevo = true;
            cboCliente.Focus();
        }

        private void btnGuardar_Click(object sender, EventArgs e)
        {
            if (cboCliente.SelectedIndex == -1 ||
                cboDireccion.SelectedIndex == -1 ||
                detalles.Count == 0)
            {
                MessageBox.Show("Complete todos los campos requeridos",
                                "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var pedido = new PEDIDO
            {
                usuario_id = (int)cboCliente.SelectedValue,
                direccion_id = (int)cboDireccion.SelectedValue,
                fecha = DateTime.Now,
                total = total,
                estado = chkEntregado.Checked ? "Entregado" : "Pendiente",
                estado_registro = true
            };

            PedidoCln.insertar(pedido, detalles);
            listar();
            limpiar();
        }

        private void btnEliminar_Click(object sender, EventArgs e)
        {
            if (idPedido == 0)
            {
                MessageBox.Show("Seleccione un pedido para eliminar",
                                "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (MessageBox.Show("¿Seguro de eliminar este pedido?",
                                "::: PIZZERIA :::",
                                MessageBoxButtons.YesNo,
                                MessageBoxIcon.Question)
                == DialogResult.Yes)
            {
                using (var db = new LabPizzeriaEntities())
                {
                    var p = db.PEDIDO.Find(idPedido);
                    p.estado = chkEntregado.Checked ? "Entregado" : "Pendiente";
                    p.estado_registro = true;
                    db.SaveChanges();
                }
                listar();
                limpiar();
            }
        }

        private void btnBuscar_Click(object sender, EventArgs e)
        {
            string crit = txtBuscar.Text.Trim().ToLower();
            using (var db = new LabPizzeriaEntities())
            {
                dgvLista.DataSource = db.PEDIDO
                    .Where(p => p.estado_registro &&
                                (p.pedido_id.ToString().Contains(crit) ||
                                 p.USUARIO.nombre.ToLower().Contains(crit) ||
                                 p.estado.ToLower().Contains(crit)))
                    .OrderByDescending(p => p.fecha)
                    .Select(p => new
                    {
                        p.pedido_id,
                        Cliente = p.USUARIO.nombre,
                        Direccion = p.DIRECCION.calle,
                        Fecha = p.fecha,
                        Entregado = p.estado,
                        p.total
                    })
                    .ToList();
            }
        }

        private void listar()
        {
            using (var db = new LabPizzeriaEntities())
            {
                dgvLista.DataSource = db.PEDIDO
                    .Where(p => p.estado_registro)
                    .OrderByDescending(p => p.fecha)
                    .Select(p => new
                    {
                        p.pedido_id,
                        Cliente = p.USUARIO.nombre,
                        Direccion = p.DIRECCION.calle,
                        Fecha = p.fecha,
                        Entregado = p.estado,
                        p.total
                    })
                    .ToList();
            }
        }

        private void limpiar()
{
    cboCliente.SelectedIndex   = -1;
    cboDireccion.DataSource    = null;
    cboPlatillo.SelectedIndex  = -1;
    txtCantidad.Clear();
    detalles.Clear();
    lblTotal.Text              = "Bs. 0.00";
    total                      = 0m;
    idPedido                   = 0;
    esNuevo                    = false;
        }
    }
}
