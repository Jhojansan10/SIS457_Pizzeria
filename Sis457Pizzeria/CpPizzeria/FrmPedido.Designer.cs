// FrmPedido.Designer.cs
using System.Windows.Forms;

namespace CpPizzeria
{
    partial class FrmPedido
    {
        private System.ComponentModel.IContainer components = null;

        // Encabezado
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtBuscar;
        private System.Windows.Forms.Button btnBuscar;

        // Listados
        private System.Windows.Forms.GroupBox gbxSerie;
        private System.Windows.Forms.DataGridView dgvLista;
        private System.Windows.Forms.DataGridView dgvDetalles;

        // Panel de acciones
        private System.Windows.Forms.Panel pnlAcciones;
        private System.Windows.Forms.Button btnNuevo;
        private System.Windows.Forms.Button btnAgregar;
        private System.Windows.Forms.Button btnEliminar;
        private System.Windows.Forms.Button btnGuardar;
        private System.Windows.Forms.Button btnCancelar;

        // Datos del pedido
        private System.Windows.Forms.GroupBox gbxDatos;

        // Cliente
        private System.Windows.Forms.Label lblNit;
        private System.Windows.Forms.ComboBox txtNit;
        private System.Windows.Forms.Label lblNombre;
        private System.Windows.Forms.TextBox txtNombre;
        private System.Windows.Forms.Button btnNuevoCliente;

        // Dirección y reparto
        private System.Windows.Forms.ComboBox cboDireccion;
        private System.Windows.Forms.RadioButton rbtnLocal;
        private System.Windows.Forms.RadioButton rbtnDelivery;
        private System.Windows.Forms.TextBox txtDireccionLibre;
        private System.Windows.Forms.ComboBox cboRepartidor;

        // Platillo
        private System.Windows.Forms.ComboBox cboPlatillo;
        private System.Windows.Forms.Label lblPrecioU;
        private System.Windows.Forms.TextBox txtPrecio;
        private System.Windows.Forms.TextBox txtCantidad;
        private System.Windows.Forms.Label lblTotal;

        // Pago
        private System.Windows.Forms.Label lblPago;
        private System.Windows.Forms.TextBox txtPago;
        private System.Windows.Forms.Label lblCambio;
        private System.Windows.Forms.TextBox txtCambio;

        // Estado / Fecha
        private System.Windows.Forms.CheckBox chkEntregado;
        private System.Windows.Forms.DateTimePicker dtpFecha;

        // justo debajo de las demás declaraciones de Label…
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;


        /// <summary> 
        /// Limpiar recursos 
        /// </summary>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Código generado por el Diseñador

        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmPedido));
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.txtBuscar = new System.Windows.Forms.TextBox();
            this.btnBuscar = new System.Windows.Forms.Button();
            this.gbxSerie = new System.Windows.Forms.GroupBox();
            this.dgvLista = new System.Windows.Forms.DataGridView();
            this.dgvDetalles = new System.Windows.Forms.DataGridView();
            this.pnlAcciones = new System.Windows.Forms.Panel();
            this.btnNuevo = new System.Windows.Forms.Button();
            this.btnAgregar = new System.Windows.Forms.Button();
            this.btnEliminar = new System.Windows.Forms.Button();
            this.btnGuardar = new System.Windows.Forms.Button();
            this.btnCancelar = new System.Windows.Forms.Button();
            this.gbxDatos = new System.Windows.Forms.GroupBox();
            this.lblNit = new System.Windows.Forms.Label();
            this.txtNit = new System.Windows.Forms.ComboBox();
            this.lblNombre = new System.Windows.Forms.Label();
            this.txtNombre = new System.Windows.Forms.TextBox();
            this.btnNuevoCliente = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.cboDireccion = new System.Windows.Forms.ComboBox();
            this.rbtnLocal = new System.Windows.Forms.RadioButton();
            this.rbtnDelivery = new System.Windows.Forms.RadioButton();
            this.label4 = new System.Windows.Forms.Label();
            this.cboRepartidor = new System.Windows.Forms.ComboBox();
            this.label5 = new System.Windows.Forms.Label();
            this.cboPlatillo = new System.Windows.Forms.ComboBox();
            this.lblPrecioU = new System.Windows.Forms.Label();
            this.txtPrecio = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.txtCantidad = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.lblTotal = new System.Windows.Forms.Label();
            this.lblPago = new System.Windows.Forms.Label();
            this.txtPago = new System.Windows.Forms.TextBox();
            this.lblCambio = new System.Windows.Forms.Label();
            this.txtCambio = new System.Windows.Forms.TextBox();
            this.chkEntregado = new System.Windows.Forms.CheckBox();
            this.dtpFecha = new System.Windows.Forms.DateTimePicker();
            this.txtDireccionLibre = new System.Windows.Forms.TextBox();
            this.gbxSerie.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvLista)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvDetalles)).BeginInit();
            this.pnlAcciones.SuspendLayout();
            this.gbxDatos.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Book Antiqua", 27.75F, System.Drawing.FontStyle.Bold);
            this.label1.Location = new System.Drawing.Point(255, 19);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(405, 44);
            this.label1.TabIndex = 0;
            this.label1.Text = "PIZZERIA - PEDIDOS";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Book Antiqua", 12F, System.Drawing.FontStyle.Bold);
            this.label2.Location = new System.Drawing.Point(69, 92);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(242, 21);
            this.label2.TabIndex = 1;
            this.label2.Text = "Buscar por ID, Cliente o Estado:";
            // 
            // txtBuscar
            // 
            this.txtBuscar.Location = new System.Drawing.Point(384, 94);
            this.txtBuscar.Name = "txtBuscar";
            this.txtBuscar.Size = new System.Drawing.Size(331, 20);
            this.txtBuscar.TabIndex = 2;
            // 
            // btnBuscar
            // 
            this.btnBuscar.Image = ((System.Drawing.Image)(resources.GetObject("btnBuscar.Image")));
            this.btnBuscar.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnBuscar.Location = new System.Drawing.Point(729, 89);
            this.btnBuscar.Name = "btnBuscar";
            this.btnBuscar.Size = new System.Drawing.Size(94, 38);
            this.btnBuscar.TabIndex = 3;
            this.btnBuscar.Text = "Buscar";
            this.btnBuscar.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnBuscar.UseVisualStyleBackColor = true;
            // 
            // gbxSerie
            // 
            this.gbxSerie.Controls.Add(this.dgvLista);
            this.gbxSerie.Controls.Add(this.dgvDetalles);
            this.gbxSerie.Font = new System.Drawing.Font("Book Antiqua", 12F, System.Drawing.FontStyle.Bold);
            this.gbxSerie.Location = new System.Drawing.Point(49, 129);
            this.gbxSerie.Name = "gbxSerie";
            this.gbxSerie.Size = new System.Drawing.Size(789, 260);
            this.gbxSerie.TabIndex = 4;
            this.gbxSerie.TabStop = false;
            this.gbxSerie.Text = "Lista de Pedidos y Detalles";
            // 
            // dgvLista
            // 
            this.dgvLista.AllowUserToAddRows = false;
            this.dgvLista.AllowUserToDeleteRows = false;
            this.dgvLista.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dgvLista.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
            this.dgvLista.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllHeaders;
            this.dgvLista.Location = new System.Drawing.Point(10, 25);
            this.dgvLista.Name = "dgvLista";
            this.dgvLista.ReadOnly = true;
            this.dgvLista.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.AutoSizeToAllHeaders;
            this.dgvLista.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect;
            this.dgvLista.Size = new System.Drawing.Size(764, 120);
            this.dgvLista.TabIndex = 0;
            // 
            // dgvDetalles
            // 
            this.dgvDetalles.AllowUserToAddRows = false;
            this.dgvDetalles.AllowUserToDeleteRows = false;
            this.dgvDetalles.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
            this.dgvDetalles.Location = new System.Drawing.Point(254, 164);
            this.dgvDetalles.Name = "dgvDetalles";
            this.dgvDetalles.ReadOnly = true;
            this.dgvDetalles.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvDetalles.Size = new System.Drawing.Size(260, 90);
            this.dgvDetalles.TabIndex = 1;
            // 
            // pnlAcciones
            // 
            this.pnlAcciones.Controls.Add(this.btnNuevo);
            this.pnlAcciones.Controls.Add(this.btnAgregar);
            this.pnlAcciones.Controls.Add(this.btnEliminar);
            this.pnlAcciones.Controls.Add(this.btnGuardar);
            this.pnlAcciones.Controls.Add(this.btnCancelar);
            this.pnlAcciones.Location = new System.Drawing.Point(88, 395);
            this.pnlAcciones.Name = "pnlAcciones";
            this.pnlAcciones.Size = new System.Drawing.Size(710, 50);
            this.pnlAcciones.TabIndex = 5;
            // 
            // btnNuevo
            // 
            this.btnNuevo.Image = ((System.Drawing.Image)(resources.GetObject("btnNuevo.Image")));
            this.btnNuevo.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnNuevo.Location = new System.Drawing.Point(68, 2);
            this.btnNuevo.Name = "btnNuevo";
            this.btnNuevo.Size = new System.Drawing.Size(90, 45);
            this.btnNuevo.TabIndex = 0;
            this.btnNuevo.Text = "Nuevo";
            this.btnNuevo.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnNuevo.UseVisualStyleBackColor = true;
            // 
            // btnAgregar
            // 
            this.btnAgregar.Image = ((System.Drawing.Image)(resources.GetObject("btnAgregar.Image")));
            this.btnAgregar.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnAgregar.Location = new System.Drawing.Point(178, 2);
            this.btnAgregar.Name = "btnAgregar";
            this.btnAgregar.Size = new System.Drawing.Size(100, 45);
            this.btnAgregar.TabIndex = 1;
            this.btnAgregar.Text = "Agregar";
            this.btnAgregar.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnAgregar.UseVisualStyleBackColor = true;
            // 
            // btnEliminar
            // 
            this.btnEliminar.Image = ((System.Drawing.Image)(resources.GetObject("btnEliminar.Image")));
            this.btnEliminar.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnEliminar.Location = new System.Drawing.Point(298, 2);
            this.btnEliminar.Name = "btnEliminar";
            this.btnEliminar.Size = new System.Drawing.Size(95, 45);
            this.btnEliminar.TabIndex = 2;
            this.btnEliminar.Text = "Eliminar";
            this.btnEliminar.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnEliminar.UseVisualStyleBackColor = true;
            // 
            // btnGuardar
            // 
            this.btnGuardar.Image = ((System.Drawing.Image)(resources.GetObject("btnGuardar.Image")));
            this.btnGuardar.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnGuardar.Location = new System.Drawing.Point(418, 2);
            this.btnGuardar.Name = "btnGuardar";
            this.btnGuardar.Size = new System.Drawing.Size(95, 45);
            this.btnGuardar.TabIndex = 3;
            this.btnGuardar.Text = "Guardar";
            this.btnGuardar.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnGuardar.UseVisualStyleBackColor = true;
            // 
            // btnCancelar
            // 
            this.btnCancelar.Image = ((System.Drawing.Image)(resources.GetObject("btnCancelar.Image")));
            this.btnCancelar.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnCancelar.Location = new System.Drawing.Point(538, 2);
            this.btnCancelar.Name = "btnCancelar";
            this.btnCancelar.Size = new System.Drawing.Size(95, 45);
            this.btnCancelar.TabIndex = 4;
            this.btnCancelar.Text = "Cancelar";
            this.btnCancelar.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnCancelar.UseVisualStyleBackColor = true;
            // 
            // gbxDatos
            // 
            this.gbxDatos.Controls.Add(this.lblNit);
            this.gbxDatos.Controls.Add(this.txtNit);
            this.gbxDatos.Controls.Add(this.lblNombre);
            this.gbxDatos.Controls.Add(this.txtNombre);
            this.gbxDatos.Controls.Add(this.btnNuevoCliente);
            this.gbxDatos.Controls.Add(this.label3);
            this.gbxDatos.Controls.Add(this.cboDireccion);
            this.gbxDatos.Controls.Add(this.rbtnLocal);
            this.gbxDatos.Controls.Add(this.rbtnDelivery);
            this.gbxDatos.Controls.Add(this.label4);
            this.gbxDatos.Controls.Add(this.cboRepartidor);
            this.gbxDatos.Controls.Add(this.label5);
            this.gbxDatos.Controls.Add(this.cboPlatillo);
            this.gbxDatos.Controls.Add(this.lblPrecioU);
            this.gbxDatos.Controls.Add(this.txtPrecio);
            this.gbxDatos.Controls.Add(this.label6);
            this.gbxDatos.Controls.Add(this.txtCantidad);
            this.gbxDatos.Controls.Add(this.label7);
            this.gbxDatos.Controls.Add(this.lblTotal);
            this.gbxDatos.Controls.Add(this.lblPago);
            this.gbxDatos.Controls.Add(this.txtPago);
            this.gbxDatos.Controls.Add(this.lblCambio);
            this.gbxDatos.Controls.Add(this.txtCambio);
            this.gbxDatos.Controls.Add(this.chkEntregado);
            this.gbxDatos.Controls.Add(this.dtpFecha);
            this.gbxDatos.Font = new System.Drawing.Font("Book Antiqua", 9F, System.Drawing.FontStyle.Bold);
            this.gbxDatos.Location = new System.Drawing.Point(49, 459);
            this.gbxDatos.Name = "gbxDatos";
            this.gbxDatos.Size = new System.Drawing.Size(789, 200);
            this.gbxDatos.TabIndex = 6;
            this.gbxDatos.TabStop = false;
            this.gbxDatos.Text = "Datos del Pedido";
            // 
            // lblNit
            // 
            this.lblNit.AutoSize = true;
            this.lblNit.Location = new System.Drawing.Point(47, 33);
            this.lblNit.Name = "lblNit";
            this.lblNit.Size = new System.Drawing.Size(34, 17);
            this.lblNit.TabIndex = 0;
            this.lblNit.Text = "NIT:";
            // 
            // txtNit
            // 
            this.txtNit.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.txtNit.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.txtNit.Location = new System.Drawing.Point(90, 27);
            this.txtNit.Name = "txtNit";
            this.txtNit.Size = new System.Drawing.Size(180, 25);
            this.txtNit.TabIndex = 1;
            // 
            // lblNombre
            // 
            this.lblNombre.AutoSize = true;
            this.lblNombre.Location = new System.Drawing.Point(24, 65);
            this.lblNombre.Name = "lblNombre";
            this.lblNombre.Size = new System.Drawing.Size(57, 17);
            this.lblNombre.TabIndex = 2;
            this.lblNombre.Text = "Nombre:";
            // 
            // txtNombre
            // 
            this.txtNombre.Location = new System.Drawing.Point(90, 62);
            this.txtNombre.Name = "txtNombre";
            this.txtNombre.Size = new System.Drawing.Size(200, 22);
            this.txtNombre.TabIndex = 3;
            // 
            // btnNuevoCliente
            // 
            this.btnNuevoCliente.Location = new System.Drawing.Point(265, 65);
            this.btnNuevoCliente.Name = "btnNuevoCliente";
            this.btnNuevoCliente.Size = new System.Drawing.Size(16, 18);
            this.btnNuevoCliente.TabIndex = 4;
            this.btnNuevoCliente.Text = "+";
            this.btnNuevoCliente.UseVisualStyleBackColor = true;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(17, 138);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(64, 17);
            this.label3.TabIndex = 21;
            this.label3.Text = "Dirección:";
            // 
            // cboDireccion
            // 
            this.cboDireccion.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.cboDireccion.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cboDireccion.Location = new System.Drawing.Point(90, 132);
            this.cboDireccion.Name = "cboDireccion";
            this.cboDireccion.Size = new System.Drawing.Size(180, 25);
            this.cboDireccion.TabIndex = 7;
            // 
            // rbtnLocal
            // 
            this.rbtnLocal.AutoSize = true;
            this.rbtnLocal.Location = new System.Drawing.Point(90, 101);
            this.rbtnLocal.Name = "rbtnLocal";
            this.rbtnLocal.Size = new System.Drawing.Size(72, 21);
            this.rbtnLocal.TabIndex = 5;
            this.rbtnLocal.Text = "En Local";
            this.rbtnLocal.UseVisualStyleBackColor = true;
            // 
            // rbtnDelivery
            // 
            this.rbtnDelivery.AutoSize = true;
            this.rbtnDelivery.Location = new System.Drawing.Point(170, 101);
            this.rbtnDelivery.Name = "rbtnDelivery";
            this.rbtnDelivery.Size = new System.Drawing.Size(75, 21);
            this.rbtnDelivery.TabIndex = 6;
            this.rbtnDelivery.Text = "Delivery";
            this.rbtnDelivery.UseVisualStyleBackColor = true;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(10, 170);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(71, 17);
            this.label4.TabIndex = 22;
            this.label4.Text = "Repartidor:";
            // 
            // cboRepartidor
            // 
            this.cboRepartidor.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboRepartidor.Location = new System.Drawing.Point(90, 167);
            this.cboRepartidor.Name = "cboRepartidor";
            this.cboRepartidor.Size = new System.Drawing.Size(180, 25);
            this.cboRepartidor.TabIndex = 9;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(306, 33);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(178, 17);
            this.label5.TabIndex = 23;
            this.label5.Text = "Seleccionar Platillo o Producto:";
            // 
            // cboPlatillo
            // 
            this.cboPlatillo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboPlatillo.Location = new System.Drawing.Point(486, 27);
            this.cboPlatillo.Name = "cboPlatillo";
            this.cboPlatillo.Size = new System.Drawing.Size(180, 25);
            this.cboPlatillo.TabIndex = 10;
            // 
            // lblPrecioU
            // 
            this.lblPrecioU.AutoSize = true;
            this.lblPrecioU.Location = new System.Drawing.Point(419, 65);
            this.lblPrecioU.Name = "lblPrecioU";
            this.lblPrecioU.Size = new System.Drawing.Size(61, 17);
            this.lblPrecioU.TabIndex = 11;
            this.lblPrecioU.Text = "Precio Bs:";
            // 
            // txtPrecio
            // 
            this.txtPrecio.Location = new System.Drawing.Point(486, 62);
            this.txtPrecio.Name = "txtPrecio";
            this.txtPrecio.ReadOnly = true;
            this.txtPrecio.Size = new System.Drawing.Size(90, 22);
            this.txtPrecio.TabIndex = 12;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(419, 105);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(61, 17);
            this.label6.TabIndex = 24;
            this.label6.Text = "Cantidad:";
            // 
            // txtCantidad
            // 
            this.txtCantidad.Location = new System.Drawing.Point(486, 100);
            this.txtCantidad.Name = "txtCantidad";
            this.txtCantidad.Size = new System.Drawing.Size(60, 22);
            this.txtCantidad.TabIndex = 13;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(601, 143);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(81, 17);
            this.label7.TabIndex = 25;
            this.label7.Text = "Total Pedido:";
            // 
            // lblTotal
            // 
            this.lblTotal.AutoSize = true;
            this.lblTotal.Location = new System.Drawing.Point(698, 143);
            this.lblTotal.Name = "lblTotal";
            this.lblTotal.Size = new System.Drawing.Size(29, 17);
            this.lblTotal.TabIndex = 14;
            this.lblTotal.Text = "0.00";
            // 
            // lblPago
            // 
            this.lblPago.AutoSize = true;
            this.lblPago.Location = new System.Drawing.Point(644, 117);
            this.lblPago.Name = "lblPago";
            this.lblPago.Size = new System.Drawing.Size(38, 17);
            this.lblPago.TabIndex = 15;
            this.lblPago.Text = "Pago:";
            // 
            // txtPago
            // 
            this.txtPago.Location = new System.Drawing.Point(694, 114);
            this.txtPago.Name = "txtPago";
            this.txtPago.Size = new System.Drawing.Size(80, 22);
            this.txtPago.TabIndex = 16;
            // 
            // lblCambio
            // 
            this.lblCambio.AutoSize = true;
            this.lblCambio.Location = new System.Drawing.Point(627, 169);
            this.lblCambio.Name = "lblCambio";
            this.lblCambio.Size = new System.Drawing.Size(55, 17);
            this.lblCambio.TabIndex = 17;
            this.lblCambio.Text = "Cambio:";
            // 
            // txtCambio
            // 
            this.txtCambio.Location = new System.Drawing.Point(694, 166);
            this.txtCambio.Name = "txtCambio";
            this.txtCambio.ReadOnly = true;
            this.txtCambio.Size = new System.Drawing.Size(80, 22);
            this.txtCambio.TabIndex = 18;
            // 
            // chkEntregado
            // 
            this.chkEntregado.AutoSize = true;
            this.chkEntregado.Location = new System.Drawing.Point(596, 35);
            this.chkEntregado.Name = "chkEntregado";
            this.chkEntregado.Size = new System.Drawing.Size(15, 14);
            this.chkEntregado.TabIndex = 19;
            // 
            // dtpFecha
            // 
            this.dtpFecha.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpFecha.Location = new System.Drawing.Point(426, 143);
            this.dtpFecha.Name = "dtpFecha";
            this.dtpFecha.Size = new System.Drawing.Size(120, 22);
            this.dtpFecha.TabIndex = 20;
            // 
            // txtDireccionLibre
            // 
            this.txtDireccionLibre.Location = new System.Drawing.Point(675, 30);
            this.txtDireccionLibre.Name = "txtDireccionLibre";
            this.txtDireccionLibre.Size = new System.Drawing.Size(156, 20);
            this.txtDireccionLibre.TabIndex = 8;
            // 
            // FrmPedido
            // 
            this.BackgroundImage = global::CpPizzeria.Properties.Resources._798_7989994_sobrepizza_2_fondos_para_menus_de_pizzerias;
            this.ClientSize = new System.Drawing.Size(886, 680);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.txtBuscar);
            this.Controls.Add(this.btnBuscar);
            this.Controls.Add(this.gbxSerie);
            this.Controls.Add(this.pnlAcciones);
            this.Controls.Add(this.gbxDatos);
            this.Name = "FrmPedido";
            this.Text = "Pizzeria - Pedidos";
            this.Load += new System.EventHandler(this.FrmPedido_Load);
            this.gbxSerie.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvLista)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvDetalles)).EndInit();
            this.pnlAcciones.ResumeLayout(false);
            this.gbxDatos.ResumeLayout(false);
            this.gbxDatos.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
    }
}
