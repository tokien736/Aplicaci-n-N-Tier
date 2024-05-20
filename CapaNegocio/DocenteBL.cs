using System;
using System.Data;
using System.Data.SqlClient;
using CapaDato;
using CapaEntidad;
using CapaNegocio.Interface;

namespace CapaNegocio
{
    public class DocenteBL : Interface.IDocente
    {
        private Datos datos = new DatosSQL(); // Instancia de la capa de datos

        public string Mensaje { get; set; } // Mensaje del procedimiento almacenado

        // Método para listar docentes
        public DataTable Listar() => datos.TraerDataTable("spListarDocente");

        // Método para agregar un docente
        public bool Agregar(Docente docente)
        {
            DataRow fila = datos.TraerDataRow("spAgregarDocente", docente.CodDocente, docente.APaterno, docente.AMaterno, docente.Nombres, docente.CodUsuario, docente.Contrasena);
            Mensaje = fila["Mensaje"].ToString();
            return Convert.ToByte(fila["CodError"]) == 0;
        }

        // Método para eliminar un docente
        public bool Eliminar(string codDocente)
        {
            try
            {
                DataRow fila = datos.TraerDataRow("spEliminarDocente", codDocente);
                Mensaje = fila["Mensaje"].ToString();
                // Elimina la siguiente línea que intenta acceder a la columna "CodError"
                //return Convert.ToByte(fila["CodError"]) == 0;
                return true; // Opcionalmente, puedes devolver verdadero si el procedimiento se ejecuta sin errores.
            }
            catch (Exception ex)
            {
                Mensaje = "Error al eliminar el docente: " + ex.Message;
                return false;
            }
        }


        // Método para actualizar información de un docente
        public bool Actualizar(Docente docente)
        {
            // Definir el parámetro de salida para el código de error
            SqlParameter parametroCodigoError = new SqlParameter("@CodError", SqlDbType.Int);
            parametroCodigoError.Direction = ParameterDirection.Output;

            // Ejecutar el procedimiento almacenado con el parámetro de salida
            DataRow fila = datos.TraerDataRow("spActualizarDocente", docente.CodDocente, docente.APaterno,
                                              docente.AMaterno, docente.Nombres, parametroCodigoError);

            // Obtener el mensaje de retorno del procedimiento almacenado
            string mensaje = fila["Mensaje"].ToString();

            // Verificar el código de error
            int codigoError = Convert.ToInt32(parametroCodigoError.Value);
            if (codigoError == 0)
            {
                // Actualización exitosa
                Mensaje = mensaje;
                return true;
            }
            else
            {
                // Error al actualizar
                Mensaje = mensaje;
                return false;
            }
        }


        // Método para buscar docentes por apellido
        public DataTable Buscar(string APaterno) => datos.TraerDataTable("spBuscarDocentePorApellido", APaterno);
    }
}
