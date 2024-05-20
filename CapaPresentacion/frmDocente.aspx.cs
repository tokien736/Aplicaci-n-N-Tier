using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CapaEntidad;
using CapaNegocio;

namespace CapaPresentacion
{
    public partial class frmDocente : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Listar(); // Mostrar todos los docentes al cargar la página por primera vez
            }
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

            // Si las contraseñas coinciden y no están vacías, entonces agregar el docente
            Docente docente = new Docente()
            {
                CodDocente = txtCodDocente.Text.Trim(),
                APaterno = txtAPaterno.Text.Trim(),
                AMaterno = txtAMaterno.Text.Trim(),
                Nombres = txtNombres.Text.Trim(),
                CodUsuario = txtCodUsuario.Text.Trim(),
                Contrasena = contrasena // Usar la contraseña obtenida anteriormente
            };

            DocenteBL docenteBL = new DocenteBL();
            if (docenteBL.Agregar(docente))
            {
                Listar();
                lblMensaje.Text = "Docente agregado correctamente.";
            }
            else
            {
                lblMensaje.Text = docenteBL.Mensaje; // Mostrar mensaje de error si no se puede agregar
            }
        }

        protected void btnEliminar_Click(object sender, EventArgs e)
        {
            string codDocente = txtCodDocente.Text.Trim();
            DocenteBL docenteBL = new DocenteBL();
            if (docenteBL.Eliminar(codDocente))
            {
                Listar();
                lblMensaje.Text = docenteBL.Mensaje;
            }
        }

        protected void btnActualizar_Click(object sender, EventArgs e)
        {
            Docente docente = new Docente()
            {
                CodDocente = txtCodDocente.Text.Trim(),
                APaterno = txtAPaterno.Text.Trim(),
                AMaterno = txtAMaterno.Text.Trim(),
                Nombres = txtNombres.Text.Trim()
            };

            DocenteBL docenteBL = new DocenteBL();
            if (docenteBL.Actualizar(docente))
            {
                Listar();
                lblMensaje.Text = docenteBL.Mensaje;
            }
        }

        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            string apaterno = txtAPaternoBuscar.Text.Trim();
            DataTable resultadoBusqueda = new DocenteBL().Buscar(apaterno);

            if (resultadoBusqueda != null && resultadoBusqueda.Rows.Count > 0)
            {
                gvDocente.DataSource = resultadoBusqueda;
                gvDocente.DataBind();
                lblMensaje.Text = "";
            }
            else
            {
                lblMensaje.Text = "No se encontraron docentes con el apellido proporcionado.";
                gvDocente.DataSource = null;
                gvDocente.DataBind();
            }
        }

        protected void btnVerTodos_Click(object sender, EventArgs e)
        {
            Listar();
            lblMensaje.Text = "";
        }

        protected void gvDocente_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (gvDocente.SelectedRow != null)
            {
                GridViewRow row = gvDocente.SelectedRow;

                txtCodDocente.Text = row.Cells[1].Text;
                txtAPaterno.Text = row.Cells[2].Text;
                txtAMaterno.Text = row.Cells[3].Text;
                txtNombres.Text = row.Cells[4].Text;

                btnActualizar.Visible = true;
                txtCodDocente.Enabled = false;
            }
        }

        private void Listar()
        {
            DocenteBL docenteBL = new DocenteBL();
            gvDocente.DataSource = docenteBL.Listar();
            gvDocente.DataBind();
            btnActualizar.Visible = false;
            LimpiarFormulario();
        }

        private void LimpiarFormulario()
        {
            txtCodDocente.Text = "";
            txtAPaterno.Text = "";
            txtAMaterno.Text = "";
            txtNombres.Text = "";
            txtCodUsuario.Text = "";
            txtContrasena.Text = "";
        }
    }
}
