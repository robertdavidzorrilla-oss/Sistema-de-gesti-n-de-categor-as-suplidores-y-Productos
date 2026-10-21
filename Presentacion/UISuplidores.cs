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
    public partial class UISuplidores : UserControl
    {
        private readonly SuplidorService _suplidorService = new SuplidorService();
        private readonly ErrorProvider _errorProvider = new ErrorProvider();

        private DataGridView _dgvSuplidores;
        private TextBox _txtEmpresa;
        private TextBox _txtContacto;
        private TextBox _txtTelefono;
        private TextBox _txtEmail;
        private Button _btnGuardar;
        private Button _btnEliminar;
        private Button _btnNuevo;

        private int _idSuplidorSeleccionado = 0;

        public UISuplidores()
        {
            InicializarComponentes();
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

            Label lblTitulo = new Label { Text = "Datos del Suplidor", Font = new Font("Segoe UI", 12, FontStyle.Bold), Location = new Point(15, 15), AutoSize = true };

            Label lblEmp = new Label { Text = "Empresa:", Location = new Point(15, 55), AutoSize = true };
            _txtEmpresa = new TextBox { Location = new Point(15, 75), Width = 300, Font = new Font("Segoe UI", 10) };

            Label lblCont = new Label { Text = "Contacto:", Location = new Point(15, 115), AutoSize = true };
            _txtContacto = new TextBox { Location = new Point(15, 135), Width = 300, Font = new Font("Segoe UI", 10) };

            Label lblTel = new Label { Text = "Teléfono (RD):", Location = new Point(15, 175), AutoSize = true };
            _txtTelefono = new TextBox { Location = new Point(15, 195), Width = 300, Font = new Font("Segoe UI", 10) };

            Label lblEmail = new Label { Text = "Email:", Location = new Point(15, 235), AutoSize = true };
            _txtEmail = new TextBox { Location = new Point(15, 255), Width = 300, Font = new Font("Segoe UI", 10) };

            _btnGuardar = new Button { Text = "Guardar", Location = new Point(15, 320), Width = 300, Height = 40, BackColor = Color.FromArgb(16, 185, 129), ForeColor = Color.White, FlatStyle = FlatStyle.Flat, Font = new Font("Segoe UI", 10, FontStyle.Bold) };
            _btnNuevo = new Button { Text = "Limpiar / Nuevo", Location = new Point(15, 370), Width = 300, Height = 35, BackColor = Color.FromArgb(30, 41, 59), ForeColor = Color.White, FlatStyle = FlatStyle.Flat };

            _btnGuardar.Click += Guardar_Click;
            _btnNuevo.Click += (s, e) => LimpiarFormulario();

            panelForm.Controls.AddRange(new Control[] { lblTitulo, lblEmp, _txtEmpresa, lblCont, _txtContacto, lblTel, _txtTelefono, lblEmail, _txtEmail, _btnGuardar, _btnNuevo });

            _dgvSuplidores = new DataGridView
            {
                Location = new Point(390, 20),
                Size = new Size(620, 440),
                BackgroundColor = Color.White,
                ReadOnly = true,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
            };
            _dgvSuplidores.SelectionChanged += DgvSuplidores_SelectionChanged;

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

            this.Controls.AddRange(new Control[] { panelForm, _dgvSuplidores, _btnEliminar });
        }

        private void CargarGrilla()
        {
            try
            {
                _dgvSuplidores.DataSource = _suplidorService.ListarActivos();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar suplidores: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void Guardar_Click(object sender, EventArgs e)
        {
            _errorProvider.Clear();

            if (!ValidadorRegex.EsTelefonoRDValido(_txtTelefono.Text))
            {
                _errorProvider.SetError(_txtTelefono, "Ingrese un número válido de R.D. (ej. 809-555-0101)");
                return;
            }

            if (!ValidadorRegex.EsEmailValido(_txtEmail.Text))
            {
                _errorProvider.SetError(_txtEmail, "Ingrese un correo válido");
                return;
            }

            Suplidor suplidor = new Suplidor
            {
                IdSuplidor = _idSuplidorSeleccionado,
                NombreEmpresa = _txtEmpresa.Text.Trim(),
                NombreContacto = _txtContacto.Text.Trim(),
                Telefono = _txtTelefono.Text.Trim(),
                Email = _txtEmail.Text.Trim()
            };

            ResultadoOperacion<bool> resultado = _suplidorService.Guardar(suplidor);

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
            if (_idSuplidorSeleccionado == 0) return;

            if (MessageBox.Show("¿Enviar suplidor a la papelera?", "Confirmación", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                ResultadoOperacion<bool> resultado = _suplidorService.Eliminar(_idSuplidorSeleccionado);
                MessageBox.Show(resultado.Mensaje, resultado.Exito ? "Éxito" : "Atención", MessageBoxButtons.OK, resultado.Exito ? MessageBoxIcon.Information : MessageBoxIcon.Warning);
                LimpiarFormulario();
                CargarGrilla();
            }
        }

        private void DgvSuplidores_SelectionChanged(object sender, EventArgs e)
        {
            if (_dgvSuplidores.CurrentRow != null && _dgvSuplidores.CurrentRow.DataBoundItem is Suplidor sup)
            {
                _idSuplidorSeleccionado = sup.IdSuplidor;
                _txtEmpresa.Text = sup.NombreEmpresa;
                _txtContacto.Text = sup.NombreContacto;
                _txtTelefono.Text = sup.Telefono;
                _txtEmail.Text = sup.Email;
            }
        }

        private void LimpiarFormulario()
        {
            _idSuplidorSeleccionado = 0;
            _txtEmpresa.Clear();
            _txtContacto.Clear();
            _txtTelefono.Clear();
            _txtEmail.Clear();
            _errorProvider.Clear();
        }
    }
}
