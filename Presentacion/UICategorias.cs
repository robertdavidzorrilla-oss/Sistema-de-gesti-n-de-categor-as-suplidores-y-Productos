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
    public partial class UICategorias : UserControl
    {

        private readonly CategoriaService _categoriaService = new CategoriaService();
        private readonly ErrorProvider _errorProvider = new ErrorProvider();

        private DataGridView _dgvCategorias;
        private TextBox _txtNombre;
        private TextBox _txtDescripcion;
        private Label _lblRutaImagen;
        private Button _btnCargarImagen;
        private Button _btnGuardar;
        private Button _btnEliminar;
        private Button _btnNuevo;

        private int _idCategoriaSeleccionada = 0;
        private string _rutaImagenSeleccionada = string.Empty;

        public UICategorias()
        {
            InicializarComponentes();
            CargarGrilla();
        }

        private void InicializarComponentes()
        {
            this.BackColor = Color.FromArgb(241, 245, 249);

            // Panel de Formulario
            Panel panelForm = new Panel
            {
                Location = new Point(20, 20),
                Size = new Size(350, 500),
                BackColor = Color.White,
                Padding = new Padding(15)
            };

            Label lblTitulo = new Label { Text = "Datos de Categoría", Font = new Font("Segoe UI", 12, FontStyle.Bold), Location = new Point(15, 15), AutoSize = true };

            Label lblNom = new Label { Text = "Nombre:", Location = new Point(15, 55), AutoSize = true };
            _txtNombre = new TextBox { Location = new Point(15, 75), Width = 300, Font = new Font("Segoe UI", 10) };

            Label lblDesc = new Label { Text = "Descripción:", Location = new Point(15, 115), AutoSize = true };
            _txtDescripcion = new TextBox { Location = new Point(15, 135), Width = 300, Height = 80, Multiline = true, Font = new Font("Segoe UI", 10) };

            Label lblImg = new Label { Text = "Imagen (opcional, máx 2MB):", Location = new Point(15, 230), AutoSize = true };
            _btnCargarImagen = new Button { Text = "Seleccionar", Location = new Point(15, 250), Width = 150,AutoSize = true, BackColor = Color.FromArgb(30, 41, 59), ForeColor = Color.White, FlatStyle = FlatStyle.Flat };
            _lblRutaImagen = new Label { Text = "Sin archivo", Location = new Point(170, 255), AutoSize = true, ForeColor = Color.Gray };

            _btnCargarImagen.Click += SeleccionarImagen_Click;

            _btnGuardar = new Button { Text = "Guardar", Location = new Point(15, 310), Width = 300, Height = 40, BackColor = Color.FromArgb(16, 185, 129), ForeColor = Color.White, FlatStyle = FlatStyle.Flat, Font = new Font("Segoe UI", 10, FontStyle.Bold) };
            _btnNuevo = new Button { Text = "Limpiar / Nuevo", Location = new Point(15, 360), Width = 300, Height = 35, BackColor = Color.FromArgb(30, 41, 59), ForeColor = Color.White, FlatStyle = FlatStyle.Flat };

            _btnGuardar.Click += Guardar_Click;
            _btnNuevo.Click += (s, e) => LimpiarFormulario();

            panelForm.Controls.AddRange(new Control[] { lblTitulo, lblNom, _txtNombre, lblDesc, _txtDescripcion, lblImg, _btnCargarImagen, _lblRutaImagen, _btnGuardar, _btnNuevo });

            // DataGridView
            _dgvCategorias = new DataGridView
            {
                Location = new Point(390, 20),
                Size = new Size(620, 450),
                BackgroundColor = Color.White,
                ReadOnly = true,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
            };
            _dgvCategorias.SelectionChanged += DgvCategorias_SelectionChanged;

            _btnEliminar = new Button
            {
                Text = "Enviar a Papelera",
                Location = new Point(390, 480),
                Size = new Size(160, 40),
                BackColor = Color.FromArgb(225, 29, 72),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 9, FontStyle.Bold)
            };
            _btnEliminar.Click += Eliminar_Click;

            this.Controls.AddRange(new Control[] { panelForm, _dgvCategorias, _btnEliminar });
        }

        private void CargarGrilla()
        {
            try
            {
                _dgvCategorias.DataSource = _categoriaService.ListarActivas();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar categorías: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void SeleccionarImagen_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog ofd = new OpenFileDialog())
            {
                ofd.Filter = "Archivos de Imagen|*.jpg;*.jpeg;*.png";
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    _rutaImagenSeleccionada = ofd.FileName;
                    _lblRutaImagen.Text = System.IO.Path.GetFileName(_rutaImagenSeleccionada);
                }
            }
        }

        private void Guardar_Click(object sender, EventArgs e)
        {
            _errorProvider.Clear();

            if (string.IsNullOrWhiteSpace(_txtNombre.Text))
            {
                _errorProvider.SetError(_txtNombre, "El nombre es obligatorio");
                return;
            }

            Categoria categoria = new Categoria
            {
                IdCategoria = _idCategoriaSeleccionada,
                Nombre = _txtNombre.Text.Trim(),
                Descripcion = _txtDescripcion.Text.Trim()
            };

            ResultadoOperacion<bool> resultado = _categoriaService.GuardarConImagen(categoria, _rutaImagenSeleccionada);

            if (resultado.Exito)
            {
                MessageBox.Show(resultado.Mensaje, "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                LimpiarFormulario();
                CargarGrilla();
            }
            else
            {
                MessageBox.Show(resultado.Mensaje, "Validación / Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void Eliminar_Click(object sender, EventArgs e)
        {
            if (_idCategoriaSeleccionada == 0)
            {
                MessageBox.Show("Seleccione una categoría de la tabla.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (MessageBox.Show("¿Desea enviar esta categoría a la papelera?", "Confirmar", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                ResultadoOperacion<bool> resultado = _categoriaService.Eliminar(_idCategoriaSeleccionada);
                MessageBox.Show(resultado.Mensaje, resultado.Exito ? "Éxito" : "Atención", MessageBoxButtons.OK, resultado.Exito ? MessageBoxIcon.Information : MessageBoxIcon.Warning);
                LimpiarFormulario();
                CargarGrilla();
            }
        }

        private void DgvCategorias_SelectionChanged(object sender, EventArgs e)
        {
            if (_dgvCategorias.CurrentRow != null && _dgvCategorias.CurrentRow.DataBoundItem is Categoria cat)
            {
                _idCategoriaSeleccionada = cat.IdCategoria;
                _txtNombre.Text = cat.Nombre;
                _txtDescripcion.Text = cat.Descripcion;
            }
        }

        private void LimpiarFormulario()
        {
            _idCategoriaSeleccionada = 0;
            _txtNombre.Clear();
            _txtDescripcion.Clear();
            _rutaImagenSeleccionada = string.Empty;
            _lblRutaImagen.Text = "Sin archivo";
            _errorProvider.Clear();
        }

        private void UICategorias_Load(object sender, EventArgs e)
        {

        }
    }
}
