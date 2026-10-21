using AccesoDatos;
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
    public partial class UIPapelera : UserControl
    {
        private readonly CategoriaService _categoriaService = new CategoriaService();
        private readonly SuplidorService _suplidorService = new SuplidorService();
        private readonly ProductoService _productoService = new ProductoService();

        private ComboBox _cboEntidad;
        private DataGridView _dgvPapelera;
        private Button _btnRestaurar;

        public UIPapelera()
        {
            InicializarComponentes();
            CargarPapelera();
        }

        private void InicializarComponentes()
        {
            this.BackColor = Color.FromArgb(241, 245, 249);

            Panel panelTop = new Panel
            {
                Location = new Point(20, 20),
                Size = new Size(990, 60),
                BackColor = Color.White
            };

            Label lblSel = new Label { Text = "Ver Registros Eliminados de:  ", Location = new Point(15, 20), AutoSize = true, Font = new Font("Segoe UI", 9, FontStyle.Bold) };

            _cboEntidad = new ComboBox
            {
                Location = new Point(240, 16),
                Width = 250,
                DropDownStyle = ComboBoxStyle.DropDownList,
                Font = new Font("Segoe UI", 10)
            };
            _cboEntidad.Items.AddRange(new object[] { "Categorías", "Suplidores", "Productos" });
            _cboEntidad.SelectedIndex = 0;
            _cboEntidad.SelectedIndexChanged += (s, e) => CargarPapelera();

            panelTop.Controls.AddRange(new Control[] { lblSel, _cboEntidad });

            _dgvPapelera = new DataGridView
            {
                Location = new Point(20, 90),
                Size = new Size(990, 420),
                BackgroundColor = Color.White,
                ReadOnly = true,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
            };

            _btnRestaurar = new Button
            {
                Text = "Restaurar Registro Seleccionado",
                Location = new Point(20, 525),
                Size = new Size(280, 42),
                BackColor = Color.FromArgb(16, 185, 129),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 10, FontStyle.Bold)
            };
            _btnRestaurar.Click += Restaurar_Click;

            this.Controls.AddRange(new Control[] { panelTop, _dgvPapelera, _btnRestaurar });
        }

        private void CargarPapelera()
        {
            try
            {
                switch (_cboEntidad.SelectedIndex)
                {
                    case 0:
                        _dgvPapelera.DataSource = _categoriaService.ListarPapelera();
                        break;
                    case 1:
                        _dgvPapelera.DataSource = _suplidorService.ListarPapelera();
                        break;
                    case 2:
                        _dgvPapelera.DataSource = _productoService.ListarPapelera();
                        break;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar la papelera: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void Restaurar_Click(object sender, EventArgs e)
        {
            if (_dgvPapelera.CurrentRow == null)
            {
                MessageBox.Show("Seleccione un registro para restaurar.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                ResultadoOperacion<bool> resultado = new ResultadoOperacion<bool>(false, "Operación no iniciada");

                switch (_cboEntidad.SelectedIndex)
                {
                    case 0: 
                        int idCat = Convert.ToInt32(_dgvPapelera.CurrentRow.Cells["IdCategoria"].Value);
                        resultado = _categoriaService.Restaurar(idCat);
                        break;
                    case 1: 
                        int idSup = Convert.ToInt32(_dgvPapelera.CurrentRow.Cells["IdSuplidor"].Value);
                        resultado = _suplidorService.Restaurar(idSup);
                        break;
                    case 2: 
                        int idProd = Convert.ToInt32(_dgvPapelera.CurrentRow.Cells["IdProducto"].Value);
                        resultado = _productoService.Restaurar(idProd);
                        break;
                }

                MessageBox.Show(resultado.Mensaje, "Restauración", MessageBoxButtons.OK, resultado.Exito ? MessageBoxIcon.Information : MessageBoxIcon.Warning);
                CargarPapelera();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error durante la restauración: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void UIPapelera_Load(object sender, EventArgs e)
        {

        }
    }
}
