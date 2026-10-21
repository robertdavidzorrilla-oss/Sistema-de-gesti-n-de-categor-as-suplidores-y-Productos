using LogicaNegocio;
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
    public partial class UIConsultas : UserControl
    {
        private readonly ConsultasLinqService _linqService = new ConsultasLinqService();

        private ComboBox _cboConsultas;
        private DataGridView _dgvResultados;
        private Button _btnEjecutar;

        public UIConsultas()
        {
            InicializarComponentes();
        }

        private void InicializarComponentes()
        {
            this.BackColor = Color.FromArgb(241, 245, 249);

            Panel panelTop = new Panel
            {
                Location = new Point(20, 20),
                Size = new Size(990, 70),
                BackColor = Color.White
            };

            Label lblSel = new Label { Text = "Seleccione la Consulta Analítica:", Location = new Point(15, 12), AutoSize = true, Font = new Font("Segoe UI", 9, FontStyle.Bold) };

            _cboConsultas = new ComboBox
            {
                Location = new Point(15, 32),
                Width = 720,
                DropDownStyle = ComboBoxStyle.DropDownList,
                Font = new Font("Segoe UI", 10)
            };

            _cboConsultas.Items.AddRange(new object[] {
                "1. Top Suplidores con más productos y precio promedio",
                "2. Categorías cuyo valor total supera el promedio general",
                "3. Estadísticas de Precios (Mín, Máx, Promedio) por Categoría",
                "4. Suplidores activos sin productos registrados actualmente"
            });
            _cboConsultas.SelectedIndex = 0;

            _btnEjecutar = new Button
            {
                Text = "Ejecutar Consulta",
                Location = new Point(750, 28),
                Size = new Size(220, 32),
                BackColor = Color.FromArgb(16, 185, 129),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 9, FontStyle.Bold)
            };
            _btnEjecutar.Click += EjecutarConsulta_Click;

            panelTop.Controls.AddRange(new Control[] { lblSel, _cboConsultas, _btnEjecutar });

            _dgvResultados = new DataGridView
            {
                Location = new Point(20, 100),
                Size = new Size(990, 480),
                BackgroundColor = Color.White,
                ReadOnly = true,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
            };

            this.Controls.AddRange(new Control[] { panelTop, _dgvResultados });
        }

        private void EjecutarConsulta_Click(object sender, EventArgs e)
        {
            try
            {
                switch (_cboConsultas.SelectedIndex)
                {
                    case 0:
                        _dgvResultados.DataSource = _linqService.ObtenerTopSuplidoresConMasProductos();
                        break;
                    case 1:
                        _dgvResultados.DataSource = _linqService.ObtenerCategoriasConValorTotalAlto();
                        break;
                    case 2:
                        _dgvResultados.DataSource = _linqService.ObtenerEstadisticasPreciosPorCategoria();
                        break;
                    case 3:
                        _dgvResultados.DataSource = _linqService.ObtenerSuplidoresSinProductosActivos();
                        break;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al ejecutar consulta LINQ: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
