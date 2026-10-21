using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Presentacion
{
    public partial class MenuPrincipal : Form
    {
        
       private Panel _panelLateral;
        private Panel _panelSuperior;
        private Panel _panelContenido;
        private Label _lblTituloModulo;

        private Button _btnDashboard;
        private Button _btnCategorias;
        private Button _btnSuplidores;
        private Button _btnProductos;
        private Button _btnConsultasLinq;
        private Button _btnPapelera;

        public MenuPrincipal()
        {
            InicializarComponentesCodigo();
            NavegarA(new UIDashboard(), "Dashboard General");
        }

        private void InicializarComponentesCodigo()
        {
            this.Text = "Sistema de Gestión de Inventario";
            this.Size = new Size(1280, 768);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.MinimumSize = new Size(1024, 600);

            //Panel Lateral Izquierdo
            _panelLateral = new Panel
            {
                Dock = DockStyle.Left,
                Width = 230,
                BackColor = Color.FromArgb(15, 23, 42) 
            };

            Label lblLogo = new Label
            {
                Text = "INVENTARIO",
                ForeColor = Color.FromArgb(16, 185, 129), 
                Font = new Font("Segoe UI", 14, FontStyle.Bold),
                Dock = DockStyle.Top,
                Height = 70,
                TextAlign = ContentAlignment.MiddleCenter
            };
            _panelLateral.Controls.Add(lblLogo);

            // Botones del Menú Lateral
            _btnDashboard = CrearBotonMenu("  Dashboard", 80);
            _btnCategorias = CrearBotonMenu("  Categorías", 135);
            _btnSuplidores = CrearBotonMenu("  Suplidores", 190);
            _btnProductos = CrearBotonMenu("  Productos", 245);
            _btnConsultasLinq = CrearBotonMenu("  Consultas", 300);
            _btnPapelera = CrearBotonMenu("  Papelera", 355);

            _btnDashboard.Click += (s, e) => NavegarA(new UIDashboard(), "Dashboard General");
            _btnCategorias.Click += (s, e) => NavegarA(new UICategorias(), "Gestión de Categorías");
            _btnSuplidores.Click += (s, e) => NavegarA(new UISuplidores(), "Gestión de Suplidores");
            _btnProductos.Click += (s, e) => NavegarA(new UIProductos(), "Gestión de Productos");
            _btnConsultasLinq.Click += (s, e) => NavegarA(new UIConsultas(), "Consultas Analíticas");
            _btnPapelera.Click += (s, e) => NavegarA(new UIPapelera(), "Papelera de Reciclaje");

            _panelLateral.Controls.AddRange(new Control[] {
                _btnPapelera, _btnConsultasLinq, _btnProductos, _btnSuplidores, _btnCategorias, _btnDashboard
            });

            // Panel Superior
            _panelSuperior = new Panel
            {
                Dock = DockStyle.Top,
                Height = 70,
                BackColor = Color.White
            };

            _lblTituloModulo = new Label
            {
                Text = "Dashboard",
                Font = new Font("Segoe UI", 16, FontStyle.Bold),
                ForeColor = Color.FromArgb(30, 41, 59),
                Location = new Point(25, 20),
                AutoSize = true
            };
            _panelSuperior.Controls.Add(_lblTituloModulo);

            //Panel Contenido Derecho
            _panelContenido = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = Color.FromArgb(241, 245, 249)
            };

            this.Controls.Add(_panelContenido);
            this.Controls.Add(_panelSuperior);
            this.Controls.Add(_panelLateral);
        }

        private Button CrearBotonMenu(string texto, int posicionY)
        {
            Button btn = new Button
            {
                Text = texto,
                Location = new Point(10, posicionY),
                Size = new Size(210, 45),
                FlatStyle = FlatStyle.Flat,
                ForeColor = Color.FromArgb(226, 232, 240),
                Font = new Font("Segoe UI", 10, FontStyle.Regular),
                TextAlign = ContentAlignment.MiddleLeft,
                Cursor = Cursors.Hand
            };
            btn.FlatAppearance.BorderSize = 0;
            btn.FlatAppearance.MouseOverBackColor = Color.FromArgb(30, 41, 59);

            return btn;
        }

        private void NavegarA(UserControl uc, string titulo)
        {
            _lblTituloModulo.Text = titulo;
            _panelContenido.Controls.Clear();
            uc.Dock = DockStyle.Fill;
            _panelContenido.Controls.Add(uc);
        }
    }
}
