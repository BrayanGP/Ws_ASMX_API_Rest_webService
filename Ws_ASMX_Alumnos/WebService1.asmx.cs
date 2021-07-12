using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Script.Services;
using System.Web.Services;

namespace Ws_ASMX_Alumnos
{
    /// <summary>
    /// Descripción breve de WebService1
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // Para permitir que se llame a este servicio web desde un script, usando ASP.NET AJAX, quite la marca de comentario de la línea siguiente. 
    [System.Web.Script.Services.ScriptService]
    public class WebService1 : System.Web.Services.WebService
    {
        private string _cnnString;
        public WebService1()
        {
            _cnnString = ConfigurationManager.ConnectionStrings["TareaAlumnos"].ConnectionString;
        }

        [WebMethod]
        public string HelloWorld()
        {
            return "Hola a todos JEJE";
        }
        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string retornoJSON(string alumno)
        {
            string mensaje;
            Alumno alumnoPrueba = new Alumno();
            try
            {
                alumnoPrueba = JsonConvert.DeserializeObject<Alumno>(alumno);
                mensaje = "Jalo";
            }
            catch
            {
                mensaje = $"No jalo al recbir el objeto: {alumnoPrueba.id} ";
            }

            string respuesta;

            respuesta = JsonConvert.SerializeObject(
                new
                {
                    mensajeEnviado = mensaje

                  
                }
                ); ;


            return respuesta;
        }


        [WebMethod]
        public List<Alumno> consultarAlumnoJson()
        {
            List<Alumno> lstAlumnoJson = new List<Alumno>();
            string query = $"select id,nombre,idEstadoOrigen from alumnos";
            try
            {
                using(SqlConnection con = new SqlConnection(_cnnString))
                {
                    SqlCommand comando = new SqlCommand(query,con);
                    comando.CommandType = System.Data.CommandType.Text;
                    con.Open();
                    SqlDataReader leer = comando.ExecuteReader();
                    while(leer.Read())
                    {
                       Alumno objAlumno = new Alumno();
                        objAlumno.id = Convert.ToInt32(leer["id"]);
                        objAlumno.nombre = Convert.ToString(leer["nombre"]);
                        objAlumno.ciudad = Convert.ToString(leer["idEstadoOrigen"]);
                        lstAlumnoJson.Add(objAlumno);

                    }
                    con.Close();
                }

               
            }
            catch(Exception ex)
            {

                throw new Exception("Error al Consultar", ex);
            
            }
            return lstAlumnoJson;
        }
        [WebMethod]
        public string mostrarLista()
        {
            string cadenaJson;
            string textoJson = "";
            List<Alumno> mostrarLista = consultarAlumnoJson();

            foreach (var ls in mostrarLista)
            {
                Alumno varAlumno = new Alumno
                {
                    id = ls.id,
                    nombre = ls.nombre,
                    ciudad = ls.ciudad
                };

                cadenaJson = JsonConvert.SerializeObject(varAlumno);
                textoJson = $"{textoJson}  {cadenaJson} \n";


            }
            string respuesta;
            respuesta = JsonConvert.SerializeObject(
             new
             {
                 mensajeEnviado =textoJson
        }
             ); 
            
            return respuesta;


        }
        [WebMethod]
        public string JSONParametros(int id, string nombre, string ciudad)
        {
            string mensaje;
            try
            {
                mensaje = "Exito";
            }
            catch
            {
                mensaje = "No Exito";
            }

         
            string respuesta;
            respuesta = JsonConvert.SerializeObject(
             new
             {
                 estatus = mensaje
             }
             );

            

            return respuesta;
        }
        [WebMethod]
        public Alumno JSONUnParametros(Alumno nombre)
        
        {
            string mensaje;
            try
            {
                mensaje = "Exito";
            }
            catch
            {
                mensaje = "No Exito";
            }


            string respuesta;
            respuesta = JsonConvert.SerializeObject(
             new
             {
                 estatus = mensaje
             }
             );


            Alumno n = new Alumno();
            n.id = nombre.id;
            n.nombre = nombre.nombre;
            n.ciudad = nombre.ciudad;

            
            return n;
        }


    }
}
