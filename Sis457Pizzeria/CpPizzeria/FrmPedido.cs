// FrmPedido.cs
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

        // Lookup CI → usuario_id
        private Dictionary<string, int> _clientesByNit;
        private int _usuarioActual = 0;  // 0 = cliente nuevo

        public FrmPedido()
        {
            InitializeComponent();

            // Eventos cliente
            txtNit.SelectedIndexChanged += txtNit_SelectedIndexChanged;
            txtNit.Leave += txtNit_SelectedIndexChanged;
            btnNuevoCliente.Click += btnNuevoCliente_Click;

            // Resto eventos
            cboPlatillo.SelectedIndexChanged += cboPlatillo_SelectedIndexChanged;
            rbtnLocal.CheckedChanged += EntregaMode_Changed;
            rbtnDelivery.CheckedChanged += EntregaMode_Changed;
            txtPago.TextChanged += txtPago_TextChanged;
            btnAgregar.Click += btnAgregar_Click;
            btnGuardar.Click += btnGuardar_Click;
            btnCancelar.Click += btnCancelar_Click;
            btnBuscar.Click += btnBuscar_Click;
            btnNuevo.Click += btnNuevo_Click;
            btnEliminar.Click += btnEliminar_Click;
        }

        private void FrmPedido_Load(object sender, EventArgs e)
        {
            // Leemos la fecha una sola vez
            DateTime ahora = DateTime.Now;
            // Fijamos Value primero (quedará dentro del rango por defecto)
            dtpFecha.Value = ahora;
            // Luego reducimos el MaxDate a ese mismo instante
            dtpFecha.MaxDate = ahora;

            cargarClientes();
            cargarDirecciones();
            cargarPlatillos();
            cargarRepartidores();
            listarPedidos();
            limpiar();
        }

        #region Carga de datos

        private void cargarClientes()
        {
            using (var db = new LabPizzeriaEntities())
            {
                var list = db.USUARIO
                             .Where(u => u.estado && u.rol == "Cliente")
                             .Select(u => new { u.usuario_id, u.ci, u.nombre })
                             .ToList();

                _clientesByNit = list.ToDictionary(x => x.ci, x => x.usuario_id);
                txtNit.DataSource = list.Select(x => x.ci).ToList();
            }

            txtNit.SelectedIndex = -1;
            txtNit.Text = "";
            txtNombre.Clear();
            _usuarioActual = 0;
        }

        private void cargarDirecciones()
        {
            using (var db = new LabPizzeriaEntities())
            {
                var lista = db.DIRECCION
                              .Where(d => d.estado)
                              .Select(d => new { d.direccion_id, d.direccion })
                              .ToList();
                cboDireccion.DataSource = lista;
                cboDireccion.ValueMember = "direccion_id";
                cboDireccion.DisplayMember = "direccion";
                cboDireccion.SelectedIndex = -1;
            }
        }

        private void cargarPlatillos()
        {
            using (var db = new LabPizzeriaEntities())
            {
                var pls = db.PLATILLO
                            .Where(p => p.estado)
                            .Select(p => new { p.platillo_id, p.nombre, p.precio })
                            .ToList();

                cboPlatillo.ValueMember = "platillo_id";
                cboPlatillo.DisplayMember = "nombre";
                cboPlatillo.DataSource = pls;
                cboPlatillo.SelectedIndex = -1;
            }
        }

        private void cargarRepartidores()
        {
            using (var db = new LabPizzeriaEntities())
            {
                var reps = db.USUARIO
                             .Where(u => u.estado && u.rol == "Repartidor")
                             .Select(u => new { u.usuario_id, u.nombre })
                             .ToList();

                cboRepartidor.ValueMember = "usuario_id";
                cboRepartidor.DisplayMember = "nombre";
                cboRepartidor.DataSource = reps;
                cboRepartidor.SelectedIndex = -1;
            }
        }

        #endregion

        #region Eventos UI

        private void txtNit_SelectedIndexChanged(object sender, EventArgs e)
        {
            var ci = txtNit.Text.Trim();
            if (_clientesByNit.TryGetValue(ci, out var uid))
            {
                using (var db = new LabPizzeriaEntities())
                    txtNombre.Text = db.USUARIO.Find(uid).nombre;
                _usuarioActual = uid;
            }
            else
            {
                txtNombre.Clear();
                _usuarioActual = 0;
            }
        }

        private void cboPlatillo_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cboPlatillo.SelectedIndex >= 0 && cboPlatillo.SelectedValue != null)
            {
                int pid = Convert.ToInt32(cboPlatillo.SelectedValue);
                using (var db = new LabPizzeriaEntities())
                {
                    var p = db.PLATILLO.Find(pid);
                    txtPrecio.Text = p.precio.ToString("N2");
                }
            }
            else
            {
                txtPrecio.Clear();
            }
        }

        private void EntregaMode_Changed(object sender, EventArgs e)
        {
            bool esLocal = rbtnLocal.Checked;
            cboDireccion.Enabled = !esLocal;
            cboRepartidor.Enabled = !esLocal;
            txtDireccionLibre.Enabled = !esLocal;

            if (esLocal)
            {
                txtDireccionLibre.Text = "En Local";
                cboDireccion.SelectedIndex = -1;
            }
            else
            {
                txtDireccionLibre.Clear();
            }
        }

        private void btnNuevoCliente_Click(object sender, EventArgs e)
        {
            MessageBox.Show(
                "Para crear un cliente, introduce su NIT y Nombre y guarda el pedido.",
                "Info", MessageBoxButtons.OK, MessageBoxIcon.Information
            );
        }

        private void btnAgregar_Click(object sender, EventArgs e)
        {
            if (cboPlatillo.SelectedIndex == -1 ||
               !int.TryParse(txtCantidad.Text, out int qty) ||
                qty <= 0) return;

            int pid = Convert.ToInt32(cboPlatillo.SelectedValue);
            using (var db = new LabPizzeriaEntities())
            {
                var p = db.PLATILLO.Find(pid);
                var ex = detalles.FirstOrDefault(d => d.platillo_id == pid);
                if (ex != null) ex.cantidad += qty;
                else detalles.Add(new DETALLE_PEDIDO { platillo_id = pid, cantidad = qty, PLATILLO = p });
            }
            actualizarDetalle();
            txtCantidad.Clear();
            cboPlatillo.SelectedIndex = -1;
        }

        private void actualizarDetalle()
        {
            dgvDetalles.DataSource = detalles
                .Select(d => new {
                    Platillo = d.PLATILLO.nombre,
                    Cantidad = d.cantidad,
                    Subtotal = d.cantidad * d.PLATILLO.precio
                })
                .ToList();
            total = detalles.Sum(d => d.cantidad * d.PLATILLO.precio);
            lblTotal.Text = total.ToString("N2");
        }

        private void txtPago_TextChanged(object sender, EventArgs e)
        {
            if (decimal.TryParse(txtPago.Text, out var pago) && pago >= total)
                txtCambio.Text = (pago - total).ToString("N2");
            else
                txtCambio.Text = "0.00";
        }

        private void btnGuardar_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtNit.Text) ||
                string.IsNullOrWhiteSpace(txtNombre.Text) ||
               (!rbtnLocal.Checked && string.IsNullOrWhiteSpace(cboDireccion.Text)) ||
                detalles.Count == 0)
            {
                MessageBox.Show(
                    "Complete NIT, Nombre, Dirección (si aplica) y agregue al menos un platillo.",
                    "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning
                );
                return;
            }

            // 1) Crear o reutilizar cliente
            int usuarioId;
            if (_usuarioActual != 0)
            {
                usuarioId = _usuarioActual;
            }
            else
            {
                using (var db = new LabPizzeriaEntities())
                {
                    var u = new USUARIO
                    {
                        ci = txtNit.Text.Trim(),
                        nombre = txtNombre.Text.Trim(),
                        rol = "Cliente",
                        estado = true,
                        fecha_registro = DateTime.Now,
                        contraseña = ""
                    };
                    db.USUARIO.Add(u);
                    db.SaveChanges();
                    usuarioId = u.usuario_id;
                }
            }

            // 2) Crear pedido
            var pedido = new PEDIDO
            {
                usuario_id = usuarioId,
                fecha = dtpFecha.Value,
                total = total,
                estado = chkEntregado.Checked ? "Entregado" : "Pendiente",
                estado_registro = true,
                repartidor_id = rbtnLocal.Checked
                                 ? (int?)null
                                 : Convert.ToInt32(cboRepartidor.SelectedValue)
            };

            // 3) Dirección: Local o Delivery (existente o nueva)
            if (rbtnLocal.Checked)
            {
                using (var db = new LabPizzeriaEntities())
                {
                    var dir = new DIRECCION
                    {
                        direccion = txtDireccionLibre.Text.Trim(),
                        estado = true,
                        fecha_registro = DateTime.Now
                    };
                    db.DIRECCION.Add(dir);
                    db.SaveChanges();
                    pedido.direccion_id = dir.direccion_id;
                }
            }
            else
            {
                // Delivery: si seleccionó una existente
                if (cboDireccion.SelectedIndex >= 0)
                {
                    pedido.direccion_id = Convert.ToInt32(cboDireccion.SelectedValue);
                }
                // si tecleó una nueva
                else
                {
                    using (var db = new LabPizzeriaEntities())
                    {
                        var dir = new DIRECCION
                        {
                            direccion = cboDireccion.Text.Trim(),
                            estado = true,
                            fecha_registro = DateTime.Now
                        };
                        db.DIRECCION.Add(dir);
                        db.SaveChanges();
                        pedido.direccion_id = dir.direccion_id;
                    }
                }
            }

            // 4) Insertar pedido + detalles
            PedidoCln.insertar(pedido, detalles);
            MessageBox.Show("Pedido guardado.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);
            listarPedidos();
            limpiar();
        }

        private void listarPedidos()
        {
            dgvLista.DataSource = PedidoCln.listar();
        }

        private void btnCancelar_Click(object sender, EventArgs e) => limpiar();
        private void btnBuscar_Click(object sender, EventArgs e) => listarPedidos();
        private void btnNuevo_Click(object sender, EventArgs e) { limpiar(); esNuevo = true; }
        private void btnEliminar_Click(object sender, EventArgs e)
        {
            if (idPedido == 0)
            {
                MessageBox.Show("Seleccione un pedido para eliminar", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (MessageBox.Show("¿Seguro de eliminar este pedido?", "Confirmar", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                using (var db = new LabPizzeriaEntities())
                {
                    var p = db.PEDIDO.Find(idPedido);
                    p.estado_registro = false;
                    db.SaveChanges();
                }
                listarPedidos();
                limpiar();
            }
        }

        private void limpiar()
        {
            esNuevo = false;
            idPedido = 0;
            detalles.Clear();
            dgvDetalles.DataSource = null;

            // Cliente
            txtNit.Text = "";
            txtNit.SelectedIndex = -1;
            txtNombre.Clear();
            _usuarioActual = 0;

            // Dirección / repartidor
            cboDireccion.SelectedIndex = -1;
            cboRepartidor.SelectedIndex = -1;
            rbtnLocal.Checked = true;
            txtDireccionLibre.Clear();

            // Platillos
            cboPlatillo.SelectedIndex = -1;
            txtPrecio.Clear();
            txtCantidad.Clear();
            lblTotal.Text = "0.00";

            // Pago
            txtPago.Clear();
            txtCambio.Text = "0.00";

            // Estado
            chkEntregado.Checked = false;
        }

        #endregion
    }
}
