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
    public partial class FrmPago : Form
    {
        private bool esNuevo = false;
        private int idPago = 0;

        public FrmPago()
        {
            InitializeComponent();
        }

        private void FrmPago_Load(object sender, EventArgs e)
        {
            cargarPedidos();
            cargarMetodos();
            listar();
        }

        private void cargarPedidos()
        {
            using (var db = new LabPizzeriaEntities())
            {
                var pedidos = db.PEDIDO
                    .Where(p => p.estado == "true")
                    .Select(p => new
                    {
                        p.pedido_id,
                        cliente = p.USUARIO.nombre
                    })
                    .ToList();

                cboPedido.DataSource = pedidos;
                cboPedido.DisplayMember = "cliente";
                cboPedido.ValueMember = "pedido_id";
                cboPedido.SelectedIndex = -1;
            }
        }

        private void cargarMetodos()
        {
            cboMetodoPago.Items.Clear();
            cboMetodoPago.Items.Add("Efectivo");
            cboMetodoPago.Items.Add("QR");
            cboMetodoPago.Items.Add("Tarjeta");
            cboMetodoPago.SelectedIndex = -1;
        }

        private void listar()
        {
            using (var db = new LabPizzeriaEntities())
            {
                var pagos = db.PAGO
                    .OrderByDescending(p => p.fecha_pago)
                    .Select(p => new
                    {
                        p.pago_id,
                        Cliente = p.PEDIDO.USUARIO.nombre,
                        Pedido = p.pedido_id,
                        Monto = p.monto_pagado,
                        Método = p.metodo,
                        Fecha = p.fecha_pago
                    })
                    .ToList();

                dgvPagos.DataSource = pagos;
            }
        }

        private void limpiarFormulario()
        {
            cboPedido.SelectedIndex = -1;
            txtMontoPagado.Clear();
            cboMetodoPago.SelectedIndex = -1;
            dtpFechaPago.Value = DateTime.Today;
            idPago = 0;
            esNuevo = false;
        }

        private void btnNuevo_Click(object sender, EventArgs e)
        {
            limpiarFormulario();
            esNuevo = true;
            cboPedido.Focus();
        }

        private void btnGuardar_Click(object sender, EventArgs e)
        {
            if (cboPedido.SelectedIndex == -1 ||
        string.IsNullOrWhiteSpace(txtMontoPagado.Text) ||
        cboMetodoPago.SelectedIndex == -1)
            {
                MessageBox.Show("Complete todos los campos obligatorios.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            decimal monto;
            if (!decimal.TryParse(txtMontoPagado.Text.Trim(), out monto))
            {
                MessageBox.Show("El monto ingresado no es válido.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            var pago = new PAGO
            {
                pedido_id = Convert.ToInt32(cboPedido.SelectedValue),
                monto_pagado = monto,
                metodo = cboMetodoPago.SelectedItem.ToString(),
                fecha_pago = dtpFechaPago.Value
            };

            using (var db = new LabPizzeriaEntities())
            {
                if (esNuevo)
                {
                    db.PAGO.Add(pago);
                }
                else
                {
                    pago.pago_id = idPago;
                    var original = db.PAGO.Find(idPago);
                    original.pedido_id = pago.pedido_id;
                    original.monto_pagado = pago.monto_pagado;
                    original.metodo = pago.metodo;
                    original.fecha_pago = pago.fecha_pago;
                }

                db.SaveChanges();
            }

            MessageBox.Show("Pago guardado correctamente.", "::: PIZZERIA :::", MessageBoxButtons.OK, MessageBoxIcon.Information);
            listar();
            limpiarFormulario();
        }

        private void btnEditar_Click(object sender, EventArgs e)
        {
            if (idPago == 0)
            {
                MessageBox.Show("Seleccione un pago para editar.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            esNuevo = false;
        }

        private void dgvPagos_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow fila = dgvPagos.Rows[e.RowIndex];

                idPago = Convert.ToInt32(dgvPagos.Rows[e.RowIndex].Cells["pago_id"].Value);
                cboPedido.Text = dgvPagos.Rows[e.RowIndex].Cells["Cliente"].Value.ToString();
                txtMontoPagado.Text = dgvPagos.Rows[e.RowIndex].Cells["Monto"].Value.ToString();
                cboMetodoPago.Text = dgvPagos.Rows[e.RowIndex].Cells["Método"].Value.ToString();
                dtpFechaPago.Value = Convert.ToDateTime(dgvPagos.Rows[e.RowIndex].Cells["Fecha"].Value);
            }
        }

        private void btnEliminar_Click(object sender, EventArgs e)
        {
            if (idPago == 0)
            {
                MessageBox.Show("Seleccione un pago para eliminar.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (MessageBox.Show("¿Está seguro de eliminar este pago?", "::: PIZZERIA :::", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                try
                {
                    ClnPizzeria.PagoCln.eliminar(idPago);
                    MessageBox.Show("Pago eliminado correctamente", "::: PIZZERIA :::", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    listar();
                    limpiarFormulario();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error al eliminar el pago:\n{ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            limpiarFormulario();
        }

        private void btnBuscar_Click(object sender, EventArgs e)
        {
            string criterio = txtBuscar.Text.Trim().ToLower();

            using (var db = new LabPizzeriaEntities())
            {
                var pagos = db.PAGO
                    .Where(p =>
                        p.PEDIDO.USUARIO.nombre.ToLower().Contains(criterio) ||
                        p.pedido_id.ToString().Contains(criterio) ||
                        p.metodo.ToLower().Contains(criterio))
                    .OrderByDescending(p => p.fecha_pago)
                    .Select(p => new
                    {
                        p.pago_id,
                        Cliente = p.PEDIDO.USUARIO.nombre,
                        Pedido = p.pedido_id,
                        Monto = p.monto_pagado,
                        Método = p.metodo,
                        Fecha = p.fecha_pago
                    })
                    .ToList();

                dgvPagos.DataSource = pagos;
            }
        }
    }
}
