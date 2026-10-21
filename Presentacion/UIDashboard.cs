using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using LogicaNegocio;

namespace Presentacion
{
    public partial class UIDashboard : UserControl
    {
        private readonly ConsultasLinqService _linqService = new ConsultasLinqService();

        public UIDashboard()
        {
            InicializarVista();
            CargarDatosDashboard();
        }

        private void InicializarVista()
        {
            this.BackColor = Color.FromArgb(241, 245, 249);
            this.Padding = new Padding(25);
        }

        private void CargarDatosDashboard()
        {
            this.Controls.Clear();

            try
            {
                ResumenDashboardDTO resumen = _linqService.ObtenerResumenDashboard();

                Panel card1 = CrearTarjeta("Total Productos Activos", resumen.TotalProductosActivos.ToString(), Color.FromArgb(16, 185, 129), 20, 20);
                Panel card2 = CrearTarjeta("Precio Promedio General", $"RD$ {resumen.PrecioPromedioGeneral:N2}", Color.FromArgb(30, 41, 59), 280, 20);
                Panel card3 = CrearTarjeta("Categoría Más Poblada", resumen.CategoriaMasPoblada, Color.FromArgb(15, 23, 42), 540, 20);
                Panel card4 = CrearTarjeta("Suplidores Con Productos", resumen.TotalSuplidoresConProductos.ToString(), Color.FromArgb(5, 150, 105), 800, 20);

                this.Controls.AddRange(new Control[] { card1, card2, card3, card4 });
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar el dashboard: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private Panel CrearTarjeta(string titulo, string valor, Color colorEncabezado, int x, int y)
        {
            Panel card = new Panel
            {
                Location = new Point(x, y),
                Size = new Size(240, 130),
                BackColor = Color.White
            };

            Panel headerBar = new Panel
            {
                Dock = DockStyle.Top,
                Height = 6,
                BackColor = colorEncabezado
            };

            Label lblTitulo = new Label
            {
                Text = titulo,
                Font = new Font("Segoe UI", 9, FontStyle.Bold),
                ForeColor = Color.FromArgb(100, 116, 139),
                Location = new Point(15, 20),
                AutoSize = true
            };

            Label lblValor = new Label
            {
                Text = valor,
                Font = new Font("Segoe UI", 16, FontStyle.Bold),
                ForeColor = Color.FromArgb(30, 41, 59),
                Location = new Point(15, 55),
                AutoSize = true
            };

            card.Controls.Add(lblValor);
            card.Controls.Add(lblTitulo);
            card.Controls.Add(headerBar);

            return card;
        }
    }
}
