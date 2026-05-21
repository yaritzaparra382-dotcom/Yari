using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using TaskManager.Helpers;
using TaskManager.Interfaces;
using TaskManager.Models;
using TaskManager.Repository;
using TaskManager.Services;

namespace TaskManager.Forms
{
    public partial class MainForm : Form
    {
        private ITaskService _servicio;

        private ListView listaTareas;
        private Button btnAgregar;
        private Button btnEditar;
        private Button btnEliminar;
        private ComboBox cmbEstado;
        private ComboBox cmbPrioridad;

        public MainForm()
        {
            var repo = new JsonTaskRepository("data/tareas.json");
            _servicio = new TaskService(repo);
            ((TaskService)_servicio).TareasModificadas += (s, e) => CargarTareas();

            InicializarComponentes();
            CargarTareas();
        }

        private void InicializarComponentes()
        {
            this.Text = "Gestor de Tareas";
            this.Size = new Size(800, 520);
            this.StartPosition = FormStartPosition.CenterScreen;

            var lblTitulo = new Label
            {
                Text = "Mis Tareas",
                Font = new Font("Segoe UI", 15, FontStyle.Bold),
                Dock = DockStyle.Top,
                Height = 42,
                TextAlign = ContentAlignment.MiddleLeft,
                Padding = new Padding(10, 0, 0, 0)
            };

            var panelFiltros = new FlowLayoutPanel
            {
                Dock = DockStyle.Top,
                Height = 38,
                Padding = new Padding(6, 4, 0, 0)
            };

            cmbEstado = new ComboBox { Width = 145, DropDownStyle = ComboBoxStyle.DropDownList };
            cmbEstado.Items.AddRange(EnumTranslator.OpcionesEstado);
            cmbEstado.SelectedIndex = 0;
            cmbEstado.SelectedIndexChanged += (s, e) => CargarTareas();

            cmbPrioridad = new ComboBox { Width = 145, DropDownStyle = ComboBoxStyle.DropDownList };
            cmbPrioridad.Items.AddRange(EnumTranslator.OpcionesPrioridad);
            cmbPrioridad.SelectedIndex = 0;
            cmbPrioridad.SelectedIndexChanged += (s, e) => CargarTareas();

            panelFiltros.Controls.Add(new Label { Text = "Estado:", AutoSize = true, Padding = new Padding(0, 4, 4, 0) });
            panelFiltros.Controls.Add(cmbEstado);
            panelFiltros.Controls.Add(new Label { Text = "  Prioridad:", AutoSize = true, Padding = new Padding(0, 4, 4, 0) });
            panelFiltros.Controls.Add(cmbPrioridad);

            listaTareas = new ListView
            {
                Dock = DockStyle.Fill,
                View = View.Details,
                FullRowSelect = true,
                GridLines = true,
                Font = new Font("Segoe UI", 9)
            };
            listaTareas.Columns.Add("Título", 210);
            listaTareas.Columns.Add("Descripción", 195);
            listaTareas.Columns.Add("Prioridad", 85);
            listaTareas.Columns.Add("Estado", 105);
            listaTareas.Columns.Add("Vencimiento", 105);

            var panelBotones = new FlowLayoutPanel
            {
                Dock = DockStyle.Bottom,
                Height = 42,
                Padding = new Padding(6, 5, 6, 0),
                FlowDirection = FlowDirection.RightToLeft
            };

            btnEliminar = new Button { Text = "Eliminar",  Width = 88, Height = 30, BackColor = Color.FromArgb(210, 50, 60),  ForeColor = Color.White, FlatStyle = FlatStyle.Flat };
            btnEditar   = new Button { Text = "Editar",    Width = 88, Height = 30, BackColor = Color.FromArgb(240, 180, 0),   FlatStyle = FlatStyle.Flat };
            btnAgregar  = new Button { Text = "+ Agregar", Width = 95, Height = 30, BackColor = Color.FromArgb(40, 160, 70),   ForeColor = Color.White, FlatStyle = FlatStyle.Flat };

            btnAgregar.Click  += BtnAgregar_Click;
            btnEditar.Click   += BtnEditar_Click;
            btnEliminar.Click += BtnEliminar_Click;

            panelBotones.Controls.AddRange(new Control[] { btnEliminar, btnEditar, btnAgregar });

            this.Controls.Add(listaTareas);
            this.Controls.Add(panelFiltros);
            this.Controls.Add(lblTitulo);
            this.Controls.Add(panelBotones);
        }

        private void CargarTareas()
        {
            listaTareas.Items.Clear();

            List<TaskItem> tareas;

            if (cmbEstado.SelectedIndex > 0)
                tareas = _servicio.FiltrarPorEstado(EnumTranslator.AEstado(cmbEstado.SelectedItem.ToString()));
            else if (cmbPrioridad.SelectedIndex > 0)
                tareas = _servicio.FiltrarPorPrioridad(EnumTranslator.APrioridad(cmbPrioridad.SelectedItem.ToString()));
            else
                tareas = _servicio.ObtenerTareas();

            foreach (var t in tareas)
            {
                var item = new ListViewItem(t.Titulo);
                item.SubItems.Add(t.Descripcion);
                item.SubItems.Add(EnumTranslator.Traducir(t.Prioridad));
                item.SubItems.Add(EnumTranslator.Traducir(t.Estado));
                item.SubItems.Add(t.FechaVencimiento?.ToString("dd/MM/yyyy") ?? "-");
                item.Tag = t;

                item.BackColor = t.Prioridad switch
                {
                    TaskPriority.Alta  => Color.FromArgb(255, 218, 218),
                    TaskPriority.Media => Color.FromArgb(255, 246, 210),
                    _                  => Color.White
                };

                listaTareas.Items.Add(item);
            }
        }

        private void BtnAgregar_Click(object sender, EventArgs e)
        {
            var form = new TaskForm();
            if (form.ShowDialog() == DialogResult.OK)
                _servicio.CrearTarea(form.TituloTarea, form.DescripcionTarea, form.PrioridadTarea, form.FechaVencimiento);
        }

        private void BtnEditar_Click(object sender, EventArgs e)
        {
            if (listaTareas.SelectedItems.Count == 0)
            {
                MessageBox.Show("Selecciona una tarea primero.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var tarea = (TaskItem)listaTareas.SelectedItems[0].Tag;
            var form = new TaskForm(tarea);

            if (form.ShowDialog() == DialogResult.OK)
            {
                tarea.Titulo           = form.TituloTarea;
                tarea.Descripcion      = form.DescripcionTarea;
                tarea.Prioridad        = form.PrioridadTarea;
                tarea.Estado           = form.EstadoTarea;
                tarea.FechaVencimiento = form.FechaVencimiento;
                _servicio.ActualizarTarea(tarea);
            }
        }

        private void BtnEliminar_Click(object sender, EventArgs e)
        {
            if (listaTareas.SelectedItems.Count == 0)
            {
                MessageBox.Show("Selecciona una tarea primero.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var tarea = (TaskItem)listaTareas.SelectedItems[0].Tag;
            var respuesta = MessageBox.Show($"¿Eliminar \"{tarea.Titulo}\"?", "Confirmar", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (respuesta == DialogResult.Yes)
                _servicio.EliminarTarea(tarea.Id);
        }
    }
}
