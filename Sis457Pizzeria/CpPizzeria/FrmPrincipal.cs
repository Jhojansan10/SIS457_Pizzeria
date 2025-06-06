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
    public partial class FrmPrincipal : Form
    {
        public FrmPrincipal()
        {
            InitializeComponent();
        }

        private void FrmPrincipal_Load(object sender, EventArgs e)
        {
            aplicarPermisos();
            lblUsuario.Text = "Bienvenido: " + Util.usuario.nombre;
        }

        private void catalogosToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void reportesToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void menuStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }

        private void platillosToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new FrmPlatillo().ShowDialog();

        }

        private void categoríasToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new FrmCategoria().ShowDialog();
        }

        private void pedidosToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new FrmPedido().ShowDialog();
        }

        private void pagosToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new FrmPago().ShowDialog();
        }

        private void usuariosToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new FrmUsuario().ShowDialog();
        }

        private void repartidoresToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new FrmRepartidor().ShowDialog();
        }

        private void direccionesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new FrmDireccion().ShowDialog();
        }

        private void verReseñasToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new FrmResena().ShowDialog();
        }

        private void reporteDeVentasToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new FrmReporteVentas().ShowDialog();
        }

        private void pedidosPorUsuarioToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new FrmPedidosPorUsuario().ShowDialog();
        }

        private void platillosMásVendidosToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new FrmPlatillosMasVendidos().ShowDialog();
        }

        private USUARIO usuarioLogueado;

        public FrmPrincipal(USUARIO usuario)
        {
            InitializeComponent();
            usuarioLogueado = usuario;
        }

        private FrmLogin frmLogin;

        public FrmPrincipal(FrmLogin frmLogin)
        {
            InitializeComponent();
            this.frmLogin = frmLogin;
        }

        private void FrmPrincipal_FormClosing(object sender, FormClosingEventArgs e)
        {
            frmLogin.Show();
        }

        private void aplicarPermisos()
        {
            var rol = Util.usuario.rol;

            // Ejemplo: desactivar todo primero
            administraciónToolStripMenuItem.Visible = false;
            reportesToolStripMenuItem.Visible = false;
            reseñasToolStripMenuItem.Visible = false;

            // Activar según rol
            if (rol == "admin")
            {
                administraciónToolStripMenuItem.Visible = true;
                reportesToolStripMenuItem.Visible = true;
                reseñasToolStripMenuItem.Visible = true;
            }
            else if (rol == "cliente")
            {
                reseñasToolStripMenuItem.Visible = true;
            }
            else if (rol == "repartidor")
            {
                reportesToolStripMenuItem.Visible = true;
            }
        }

        private void lblTitulo_Click(object sender, EventArgs e)
        {

        }
    }
}
