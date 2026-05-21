using System;
using System.Drawing;
using System.Windows.Forms;
using TaskManager.Helpers;
using TaskManager.Models;

namespace TaskManager.Forms
{
    public class TaskForm : Form
    {
        public string TituloTarea       => txtTitulo.Text.Trim();
        public string DescripcionTarea  => txtDescripcion.Text.Trim();
        public TaskPriority PrioridadTarea => EnumTranslator.APrioridad(cmbPrioridad.SelectedItem.ToString());
        public TaskStatus EstadoTarea      => EnumTranslator.AEstado(cmbEstado.SelectedItem.ToString());
        public DateTime? FechaVencimiento  => chkFecha.Checked ? (DateTime?)dtFecha.Value : null;

        private TextBox txtTitulo;
        private TextBox txtDescripcion;
        private ComboBox cmbPrioridad;
        private ComboBox cmbEstado;
        private DateTimePicker dtFecha;
        private CheckBox chkFecha;
        private Button btnGuardar;
        private Button btnCancelar;

        public TaskForm(TaskItem tarea = null)
        {
            this.Text = tarea == null ? "Nueva Tarea" : "Editar Tarea";
            this.Size = new Size(390, 320);
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.StartPosition = FormStartPosition.CenterParent;
            this.MaximizeBox = false;

            ConstruirLayout();

            if (tarea != null)
                CargarDatos(tarea);
        }

        private void ConstruirLayout()
        {
            var layout = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 2,
                RowCount = 7,
                Padding = new Padding(10),
                AutoSize = true
            };
            layout.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 105));
            layout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));

            txtTitulo      = new TextBox { Dock = DockStyle.Fill };
            txtDescripcion = new TextBox { Dock = DockStyle.Fill, Multiline = true, Height = 55 };

            cmbPrioridad = new ComboBox { Dock = DockStyle.Fill, DropDownStyle = ComboBoxStyle.DropDownList };
            cmbPrioridad.Items.AddRange(new[] { "Baja", "Media", "Alta" });
            cmbPrioridad.SelectedIndex = 1;

            cmbEstado = new ComboBox { Dock = DockStyle.Fill, DropDownStyle = ComboBoxStyle.DropDownList };
            cmbEstado.Items.AddRange(new[] { "Pendiente", "En progreso", "Completada" });
            cmbEstado.SelectedIndex = 0;

            chkFecha = new CheckBox { Text = "Tiene fecha de vencimiento", Dock = DockStyle.Fill };
            dtFecha  = new DateTimePicker { Dock = DockStyle.Fill, Enabled = false, Format = DateTimePickerFormat.Short };
            chkFecha.CheckedChanged += (s, e) => dtFecha.Enabled = chkFecha.Checked;

            btnGuardar  = new Button { Text = "Guardar",  DialogResult = DialogResult.OK,     Width = 88, Height = 28, BackColor = Color.FromArgb(40, 160, 70), ForeColor = Color.White, FlatStyle = FlatStyle.Flat };
            btnCancelar = new Button { Text = "Cancelar", DialogResult = DialogResult.Cancel,  Width = 88, Height = 28, FlatStyle = FlatStyle.Flat };

            btnGuardar.Click += (s, e) =>
            {
                if (string.IsNullOrWhiteSpace(txtTitulo.Text))
                {
                    MessageBox.Show("El título no puede estar vacío.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    this.DialogResult = DialogResult.None;
                }
            };

            AgregarFila(layout, 0, "Título:",      txtTitulo);
            AgregarFila(layout, 1, "Descripción:", txtDescripcion);
            AgregarFila(layout, 2, "Prioridad:",   cmbPrioridad);
            AgregarFila(layout, 3, "Estado:",      cmbEstado);
            AgregarFila(layout, 4, "",             chkFecha);
            AgregarFila(layout, 5, "Vencimiento:", dtFecha);

            var panelBotones = new FlowLayoutPanel { Dock = DockStyle.Fill, FlowDirection = FlowDirection.RightToLeft };
            panelBotones.Controls.AddRange(new Control[] { btnCancelar, btnGuardar });
            layout.Controls.Add(panelBotones, 0, 6);
            layout.SetColumnSpan(panelBotones, 2);

            this.Controls.Add(layout);
            this.AcceptButton = btnGuardar;
            this.CancelButton = btnCancelar;
        }

        private void AgregarFila(TableLayoutPanel layout, int fila, string etiqueta, Control control)
        {
            layout.Controls.Add(new Label
            {
                Text = etiqueta,
                Dock = DockStyle.Fill,
                TextAlign = System.Drawing.ContentAlignment.MiddleRight,
                Padding = new Padding(0, 3, 5, 0)
            }, 0, fila);
            layout.Controls.Add(control, 1, fila);
        }

        private void CargarDatos(TaskItem tarea)
        {
            txtTitulo.Text      = tarea.Titulo;
            txtDescripcion.Text = tarea.Descripcion;
            cmbPrioridad.SelectedItem = EnumTranslator.Traducir(tarea.Prioridad);
            cmbEstado.SelectedItem    = EnumTranslator.Traducir(tarea.Estado);

            if (tarea.FechaVencimiento.HasValue)
            {
                chkFecha.Checked = true;
                dtFecha.Value    = tarea.FechaVencimiento.Value;
            }
        }
    }
}
