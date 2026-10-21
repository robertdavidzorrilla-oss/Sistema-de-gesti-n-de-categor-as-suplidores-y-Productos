using AccesoDatos;
using Entidades;
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
    public partial class UIProductos : UserControl
    {
        private readonly ProductoService _productoService = new ProductoService();
        private readonly CategoriaService _categoriaService = new CategoriaService();
        private readonly SuplidorService _suplidorService = new SuplidorService();
        private readonly ErrorProvider _errorProvider = new ErrorProvider();

        private DataGridView _dgvProductos;
        private TextBox _txtNombre;
        private TextBox _txtPrecio;
        private ComboBox _cboCategoria;
        private ComboBox _cboSuplidor;
        private Button _btnGuardar;
        private Button _btnEliminar;
        private Button _btnNuevo;

        private int _idProductoSeleccionado = 0;

        public UIProductos()
        {
            InicializarComponentes();
            CargarCombos();
            CargarGrilla();
        }

        private void InicializarComponentes()
        {
            this.BackColor = Color.FromArgb(241, 245, 249);

            Panel panelForm = new Panel
            {
                Location = new Point(20, 20),
                Size = new Size(350, 480),
                BackColor = Color.White,
                Padding = new Padding(15)
            };

            Label lblTitulo = new Label { Text = "Datos del Producto", Font = new Font("Segoe UI", 12, FontStyle.Bold), Location = new Point(15, 15), AutoSize = true };

            Label lblNom = new Label { Text = "Nombre Producto:", Location = new Point(15, 55), AutoSize = true };
            _txtNombre = new TextBox { Location = new Point(15, 75), Width = 300, Font = new Font("Segoe UI", 10) };

            Label lblPrec = new Label { Text = "Precio (RD$):", Location = new Point(15, 115), AutoSize = true };
            _txtPrecio = new TextBox { Location = new Point(15, 135), Width = 300, Font = new Font("Segoe UI", 10) };

            Label lblCat = new Label { Text = "Categoría:", Location = new Point(15, 175), AutoSize = true };
            _cboCategoria = new ComboBox { Location = new Point(15, 195), Width = 300, DropDownStyle = ComboBoxStyle.DropDownList, Font = new Font("Segoe UI", 10) };

            Label lblSup = new Label { Text = "Suplidor:", Location = new Point(15, 235), AutoSize = true };
            _cboSuplidor = new ComboBox { Location = new Point(15, 255), Width = 300, DropDownStyle = ComboBoxStyle.DropDownList, Font = new Font("Segoe UI", 10) };

            _btnGuardar = new Button { Text = "Guardar", Location = new Point(15, 320), Width = 300, Height = 40, BackColor = Color.FromArgb(16, 185, 129), ForeColor = Color.White, FlatStyle = FlatStyle.Flat, Font = new Font("Segoe UI", 10, FontStyle.Bold) };
            _btnNuevo = new Button { Text = "Limpiar / Nuevo", Location = new Point(15, 370), Width = 300, Height = 35, BackColor = Color.FromArgb(30, 41, 59), ForeColor = Color.White, FlatStyle = FlatStyle.Flat };

            _btnGuardar.Click += Guardar_Click;
            _btnNuevo.Click += (s, e) => LimpiarFormulario();

            panelForm.Controls.AddRange(new Control[] { lblTitulo, lblNom, _txtNombre, lblPrec, _txtPrecio, lblCat, _cboCategoria, lblSup, _cboSuplidor, _btnGuardar, _btnNuevo });

            _dgvProductos = new DataGridView
            {
                Location = new Point(390, 20),
                Size = new Size(620, 440),
                BackgroundColor = Color.White,
                ReadOnly = true,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
            };
            _dgvProductos.SelectionChanged += DgvProductos_SelectionChanged;

            _btnEliminar = new Button
            {
                Text = "Enviar a Papelera",
                Location = new Point(390, 470),
                Size = new Size(160, 40),
                BackColor = Color.FromArgb(225, 29, 72),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 9, FontStyle.Bold)
            };
            _btnEliminar.Click += Eliminar_Click;

            this.Controls.AddRange(new Control[] { panelForm, _dgvProductos, _btnEliminar });
        }

        private void CargarCombos()
        {
            try
            {
                _cboCategoria.DataSource = _categoriaService.ListarActivas();
                _cboCategoria.DisplayMember = "Nombre";
                _cboCategoria.ValueMember = "IdCategoria";

                _cboSuplidor.DataSource = _suplidorService.ListarActivos();
                _cboSuplidor.DisplayMember = "NombreEmpresa";
                _cboSuplidor.ValueMember = "IdSuplidor";
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar combos: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void CargarGrilla()
        {
            try
            {
                _dgvProductos.DataSource = _productoService.ListarActivos();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar productos: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void Guardar_Click(object sender, EventArgs e)
        {
            _errorProvider.Clear();

            if (!decimal.TryParse(_txtPrecio.Text, out decimal precio) || precio <= 0)
            {
                _errorProvider.SetError(_txtPrecio, "El precio debe ser mayor a 0");
                return;
            }

            if (decimal.Round(precio, 2) != precio)
            {
                _errorProvider.SetError(_txtPrecio, "Máximo 2 posiciones decimales");
                return;
            }

            Producto producto = new Producto
            {
                IdProducto = _idProductoSeleccionado,
                Nombre = _txtNombre.Text.Trim(),
                Precio = precio,
                IdCategoria = Convert.ToInt32(_cboCategoria.SelectedValue),
                IdSuplidor = Convert.ToInt32(_cboSuplidor.SelectedValue)
            };

            ResultadoOperacion<bool> resultado = _productoService.Guardar(producto);

            if (resultado.Exito)
            {
                MessageBox.Show(resultado.Mensaje, "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                LimpiarFormulario();
                CargarGrilla();
            }
            else
            {
                MessageBox.Show(resultado.Mensaje, "Atención", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void Eliminar_Click(object sender, EventArgs e)
        {
            if (_idProductoSeleccionado == 0) return;

            if (MessageBox.Show("¿Enviar producto a la papelera?", "Confirmación", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                ResultadoOperacion<bool> resultado = _productoService.Eliminar(_idProductoSeleccionado);
                MessageBox.Show(resultado.Mensaje, "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                LimpiarFormulario();
                CargarGrilla();
            }
        }

        private void DgvProductos_SelectionChanged(object sender, EventArgs e)
        {
            if (_dgvProductos.CurrentRow != null && _dgvProductos.CurrentRow.DataBoundItem is Producto prod)
            {
                _idProductoSeleccionado = prod.IdProducto;
                _txtNombre.Text = prod.Nombre;
                _txtPrecio.Text = prod.Precio.ToString("F2");
                _cboCategoria.SelectedValue = prod.IdCategoria;
                _cboSuplidor.SelectedValue = prod.IdSuplidor;
            }
        }

        private void LimpiarFormulario()
        {
            _idProductoSeleccionado = 0;
            _txtNombre.Clear();
            _txtPrecio.Clear();
            _errorProvider.Clear();
        }
    }
}
