    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;
    using System.Web;
    using System.Web.DynamicData;
    using System.Web.UI;
    using System.Web.UI.WebControls;

    using CapaEntidad;
    using CapaNegocio;

    namespace CapaPresentacion
    {

        public partial class frmAlumno : System.Web.UI.Page
        {

            private void Listar()
            {
                AlumnoBL alumnoBL = new AlumnoBL();
                gvAlumno.DataSource = alumnoBL.Listar();
                gvAlumno.DataBind();
                btnActualizar.Visible = false;
            }

            protected void Page_Load(object sender, EventArgs e)
            {
                if (!Page.IsPostBack)
                    Listar();
            }

        protected void btnAgregar_Click(object sender, EventArgs e)
        {
            // Obtener los valores de las cajas de texto
            string contrasena = txtContrasena.Text.Trim();
            string confirmarContrasena = txtConfirmarContrasena.Text.Trim();

            // Verificar si alguna de las contraseñas está vacía
            if (string.IsNullOrEmpty(contrasena) || string.IsNullOrEmpty(confirmarContrasena))
            {
                lblMensaje.Text = "Por favor, complete ambos campos de contraseña.";
                return;
            }

            // Verificar si las contraseñas son iguales
            if (contrasena != confirmarContrasena)
            {
                lblMensaje.Text = "Las contraseñas no coinciden. Por favor, inténtelo de nuevo.";
                return;
            }

            // Si las contraseñas coinciden y no están vacías, entonces agregar el alumno
            Alumno alumno = new Alumno();
            alumno.CodAlumno = txtCodAlumno.Text.Trim();
            alumno.APaterno = txtAPaterno.Text.Trim();
            alumno.AMaterno = txtAMaterno.Text.Trim();
            alumno.Nombres = txtNombres.Text.Trim();
            alumno.CodUsuario = txtCodUsuario.Text.Trim();
            alumno.Contrasena = contrasena; // Usar la contraseña obtenida anteriormente
            alumno.CodEscuela = txtCodEscuela.Text.Trim();

            AlumnoBL alumnoBL = new AlumnoBL();
            if (alumnoBL.Agregar(alumno))
            {
                Listar();
                lblMensaje.Text = "Alumno agregado correctamente.";
            }
            else
            {
                lblMensaje.Text = alumnoBL.Mensaje; // Mostrar mensaje de error si no se puede agregar
            }
        }


        protected void btnEliminar_Click(object sender, EventArgs e)
            {
                string codAlumno = txtCodAlumno.Text.Trim();
                AlumnoBL alumnoBL = new AlumnoBL();
                if (alumnoBL.Eliminar(codAlumno))
                    Listar();
                lblMensaje.Text = alumnoBL.Mensaje;
            }

            protected void btnBuscar_Click(object sender, EventArgs e)
            {
                // Obtener el apellido a buscar
                string apellido = txtAPaternoBuscar.Text.Trim();


                // Llamar al método de búsqueda
                DataTable resultadoBusqueda = new CapaNegocio.AlumnoBL().Buscar(apellido);

                // Verificar si se encontraron resultados
                if (resultadoBusqueda != null && resultadoBusqueda.Rows.Count > 0)
                {
                    // Mostrar los resultados en el GridView
                    gvAlumno.DataSource = resultadoBusqueda;
                    gvAlumno.DataBind();
                    lblMensaje.Text = ""; // Limpiar mensaje de error si lo hay
                }
                else
                {
                    // Mostrar mensaje de error
                    lblMensaje.Text = "No se encontraron alumnos con el apellido proporcionado.";
                    // Limpiar el GridView
                    gvAlumno.DataSource = null;
                    gvAlumno.DataBind();
                }
            }


            protected void btnActualizar_Click(object sender, EventArgs e)
            {
                // Verificar si hay una fila seleccionada
                if (gvAlumno.SelectedRow != null)
                {
                    // Obtener los valores de las cajas de texto
                    string codAlumno = txtCodAlumno.Text.Trim();
                    string apaterno = txtAPaterno.Text.Trim();
                    string amaterno = txtAMaterno.Text.Trim();
                    string nombres = txtNombres.Text.Trim();
                    string codUsuario = txtCodUsuario.Text.Trim();
                    string codEscuela = txtCodEscuela.Text.Trim();

                    // Crear un objeto Alumno con los datos actualizados
                    Alumno alumno = new Alumno()
                    {
                        CodAlumno = codAlumno,
                        APaterno = apaterno,
                        AMaterno = amaterno,
                        Nombres = nombres,
                        CodUsuario = codUsuario,
                        CodEscuela = codEscuela
                    };

                    // Llamar al método de la capa de negocio para actualizar el alumno
                    AlumnoBL alumnoBL = new AlumnoBL();
                    bool actualizado = alumnoBL.Actualizar(alumno);

                    if (actualizado)
                    {
                        // Actualizar el GridView
                        gvAlumno.DataSource = alumnoBL.Listar();
                        gvAlumno.DataBind();

                        // Mostrar mensaje de éxito
                        lblMensaje.Text = "Alumno actualizado correctamente.";

                        // Limpiar el formulario
                        LimpiarFormulario();
                    }
                    else
                    {
                        // Mostrar mensaje de error
                        lblMensaje.Text = "Error al actualizar el alumno.";
                    }
                }

                // Limpiar el mensaje de error
                lblMensaje.Text = "";
            }


            protected void gvAlumno_SelectedIndexChanged(object sender, EventArgs e)
            {
                // Verificar si hay una fila seleccionada
                if (gvAlumno.SelectedRow != null)
                {
                    // Obtener la fila seleccionada
                    GridViewRow row = gvAlumno.SelectedRow;

                    // Obtener los valores de las celdas de la fila seleccionada
                    string codAlumno = row.Cells[1].Text;
                    string apaterno = row.Cells[2].Text;
                    string amaterno = row.Cells[3].Text;
                    string nombres = row.Cells[4].Text;
                    string codEscuela = row.Cells[6].Text;

                    // Asignar los valores a las cajas de texto
                    txtCodAlumno.Text = codAlumno;
                    txtAPaterno.Text = apaterno;
                    txtAMaterno.Text = amaterno;
                    txtNombres.Text = nombres;
                    txtCodEscuela.Text = codEscuela;

                    // Habilitar el botón de "Actualizar"
                    btnActualizar.Visible = true;
                    // Deshabilitar la edición del campo txtCodAlumno
                    txtCodAlumno.Enabled = false;


                }
            }


            protected void btnVerTodos_Click(object sender, EventArgs e)
            {
                // Llamar al método para listar todos los alumnos
                Listar();

                // Limpiar el mensaje de error
                lblMensaje.Text = "";

                // Limpiar los campos del formulario
                LimpiarFormulario();

                // Ocultar el botón de "Actualizar"
                btnActualizar.Visible = false;
            }





            private void LimpiarFormulario()
            {
                // Limpiar los campos del formulario
                txtCodAlumno.Text = "";
                txtAPaterno.Text = "";
                txtAMaterno.Text = "";
                txtNombres.Text = "";
                txtCodUsuario.Text = "";
                txtContrasena.Text = "";
                txtCodEscuela.Text = "";
            }

        }
    }