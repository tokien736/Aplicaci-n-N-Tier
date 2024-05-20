using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using CapaEntidad; // llamar al mapeado objeto relacional
using System.Data; // llamar a los buffer de memoria: Tablas con registros

namespace CapaNegocio.Interface
{
    interface IDocente
    {
        // Declarar los metodos de la clase alumno
        DataTable Listar();
        bool Agregar(Docente docente);
        bool Eliminar(string codAlumno);
        bool Actualizar(Docente docente);
        DataTable Buscar(string codAlumno);
    }
}
