using EntitiesPOJO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Net.Http;
using System.Text.RegularExpressions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http.Headers;
using System.Web.Script.Serialization;

namespace Interfaz_de_traduccion
{
    class Program
    {
        public static HttpClient cliente { get; set; }

        public static List<Idioma> listaIdiomas = new List<Idioma>();

        public static Idioma idioma1 = new Idioma();

        public static Traductor t1 = new Traductor();

        public static string nombre = "";

        static void Main(string[] args)
        {

            menu();

            
        }

        private static void menu()
        {
            Boolean salir = false;
            string idioma = "";

            Console.WriteLine("==================================================================");
            Console.WriteLine(
                    "= " + " Ingrese su nombre\n" +
                            "==================================================================");
            nombre = Console.ReadLine();

            imprimir("Bienvenido al Poli-Bot " + nombre + "\n");

            while (!salir)
            {
                imprimir("Ingrese el idioma");
                idioma = Console.ReadLine();

                mainAsyncBuscarIdioma(idioma.ToLower()).Wait();

               if(idioma1.IdiomaNuevo != null)
                {


                    int opcion = 0;
                    string nuevaPalabra = "";
                    string resultante = "";
                    string[] coleccion = new string[] { };

                    imprimir("Seleccione 1 si es una palabra o seleccion 2 si es una frase");

                    opcion = Int32.Parse(Console.ReadLine());

                    if (opcion == 1)
                    {
                        //palabra
                        imprimir("Digite la palabra a traducir");
                        nuevaPalabra = Console.ReadLine();

                        mainAsyncTraducir(nuevaPalabra.ToLower()).Wait();

                        imprimir(t1.PT);

                    }
                    else
                    {
                        //frase
                        imprimir("Digite la frase a traducir");
                        nuevaPalabra = Console.ReadLine();
                        coleccion = nuevaPalabra.Split(' ');
                        string tmp = "";
                        foreach (string buscador in coleccion)
                        {
                            tmp = buscador;
                            mainAsyncBuscarPalabras(buscador.ToLower()).Wait();
                            if (t1 == null)
                            {
                                t1 = new Traductor();
                                imprimir("Su palabra " + tmp + " no está almacenada, por favor digite la traduccion, en el idioma de " + idioma1.IdiomaNuevo);
                                nuevaPalabra = Console.ReadLine();
                                t1.PO = tmp;
                                mainAsyncAgregarTraduccion(nuevaPalabra).Wait();
                                imprimir("Nueva traduccion guardada");
                            }
                            
                            if (t1.PO != null && t1.PT == null)
                            {
                                string nuevaTraduccion = "";

                                imprimir("Su palabra existe en nuestros registros");
                                imprimir("");
                                imprimir("Por favor ingrese la nueva traduccion de la palabra " + tmp +" en el idioma " + idioma1.IdiomaNuevo);
                                nuevaTraduccion = Console.ReadLine();
                                imprimir("");
                                imprimir("espere un momento...");
                                mainAsyncGuardarTraduccion(nuevaPalabra, nuevaTraduccion).Wait();
                                imprimir("Nueva traduccion guardada");
                            }

                            mainAsyncTraducir(tmp.ToLower()).Wait();

                            if (t1.PO != null && t1.PT != null){
                                resultante += " " + t1.PT;
                            }

                            

                        }

                        imprimir(resultante);


                    }
                }
               else
                {
                    string nuevaPalabra = "";
                    string tmp = "";

                    imprimir("Este idioma no existe, se agregará a la base de datos");
                    mainAsyncGuardarIdioma(idioma.ToLower()).Wait();
                    imprimir("Este idioma se guardo con exito");
                    mainAsyncBuscarIdioma(idioma.ToLower()).Wait();
                    imprimir("");
                    imprimir("Ahora, por favor escriba la palabra a traducir, primeramente buscaremos sí esta ya existe");

                    nuevaPalabra = Console.ReadLine();
                    tmp = nuevaPalabra;

                    mainAsyncBuscarPalabras(nuevaPalabra).Wait();

                    if(t1 == null)
                    {
                        t1 = new Traductor();
                        imprimir("Su palabra no está almacenada, por favor digite la traduccion, en el idioma de " + idioma1.IdiomaNuevo);
                        nuevaPalabra = Console.ReadLine();
                        t1.PO = tmp;
                        mainAsyncAgregarTraduccion(nuevaPalabra).Wait();
                        t1.PO = tmp;
                        imprimir("Nueva traduccion guardada");

                        mainAsyncTraducir(t1.PO.ToLower()).Wait();

                        imprimir(t1.PT);

                    }
                    else
                    {
                        string nuevaTraduccion = "";

                        imprimir("Su palabra existe en nuestros registros");
                        imprimir("");
                        imprimir("Por favor ingrese la nueva traduccion de la palabra en el idioma " + idioma1.IdiomaNuevo);
                        nuevaTraduccion = Console.ReadLine();
                        imprimir("");
                        imprimir("espere un momento...");
                        mainAsyncGuardarTraduccion(nuevaPalabra, nuevaTraduccion).Wait();
                        t1 = new Traductor();
                        t1.PO = nuevaPalabra;

                        mainAsyncTraducir(t1.PO.ToLower()).Wait();

                        imprimir(t1.PT);
                    }
                }

                imprimir("Desea seguir traduciendo? Digite 1 para seguir o 0 para salir");
                int codigo = Int32.Parse(Console.ReadLine());
                if (codigo == 1)
                {
                    salir = false;
                }
                else
                {
                    salir = true;
                }

            }
        }

        private static void startCliente()
        {
            cliente = new HttpClient
            {
                BaseAddress = new Uri("https://localhost:44349/api/")
            };
            cliente.DefaultRequestHeaders.Accept.Clear();
            var acceptJson = new MediaTypeWithQualityHeaderValue("application/json");
            cliente.DefaultRequestHeaders.Accept.Add(acceptJson);
        }

        public static void listarIdiomas()
        {
            Console.WriteLine("Los idiomas registrados son: ");
            mainAsyncListarIdioma().Wait();
            foreach (var element in listaIdiomas)
            {
                Console.WriteLine(element.IdiomaNuevo);
            }
        }

        public static async Task mainAsyncListarIdioma()
        {
            startCliente();
            listaIdiomas = await GetListarIdiomaAsync();
        }

        public static async Task mainAsyncBuscarIdioma(string lengua)
        {
            startCliente();
            idioma1 = await GetIdiomaAsync(lengua);
        }

        public static async Task mainAsyncTraducir(string palabra)
        {
            startCliente();
            t1 = await GetPalabraTraducidaAsync(palabra);
        }

        public static async Task mainAsyncGuardarTraduccion(string palabra, string traduccion)
        {
            startCliente();
            t1 = await GuardarPalabraAsync(palabra,traduccion);
        }

        public static async Task mainAsyncGuardarIdioma(string palabra)
        {
            startCliente();
            idioma1 = await GuardarIdiomaAsync(palabra);
        }

        public static async Task mainAsyncBuscarPalabras(string palabra)
        {
            startCliente();
            t1 = await BuscarPalabrasAsync(palabra);
        }

        public static async Task mainAsyncAgregarTraduccion(string palabra)
        {
            startCliente();
            t1 = await AgregarTraduccion(palabra);
        }

        private static async Task<List<Idioma>> GetListarIdiomaAsync()
        {
            using (HttpResponseMessage response = await cliente.GetAsync("https://localhost:44349/api/idioma/Get"))

            {

                if (response.IsSuccessStatusCode)
                {

                    //Dictionary<string, Idioma> operationResult1 = await response.Content.ReadAsAsync<Dictionary<string, Idioma>>();
                    var json = await response.Content.ReadAsStringAsync();
                    var data = JsonConvert.DeserializeObject<List<Idioma>>(json);
                    return data;
                }
                else
                {
                    throw new Exception(response.ReasonPhrase);
                }
            }
        }

        private static async Task<Idioma> GetIdiomaAsync(string idioma)
        {
            HttpResponseMessage response = await cliente.GetAsync("https://localhost:44349/api/idioma/Get?idioma="+idioma.ToLower());
            //get data as Json string 
            string data = await response.Content.ReadAsStringAsync();
            //use JavaScriptSerializer from System.Web.Script.Serialization
            JavaScriptSerializer JSserializer = new JavaScriptSerializer();
            //deserialize to your class
            Idioma valor = JSserializer.Deserialize<Idioma>(data);

            return valor;
        }

        private static async Task<Traductor> GetPalabraTraducidaAsync(string palabra)
        {
            Traductor traductor = new Traductor();
            traductor.Cod_Idioma = idioma1.Id;
            traductor.PO = palabra;

            var json = JsonConvert.SerializeObject(traductor);
            var container = new StringContent(json, Encoding.UTF8, "application/json");

            HttpResponseMessage response = await cliente.PostAsync("https://localhost:44349/api/TranslateWord/TranslateWord", container);
            //get data as Json string 
            string data = await response.Content.ReadAsStringAsync();
            //use JavaScriptSerializer from System.Web.Script.Serialization
            JavaScriptSerializer JSserializer = new JavaScriptSerializer();
            //deserialize to your class
            Traductor valor = JSserializer.Deserialize<Traductor>(data);

            return valor;
        }

        private static async Task<Idioma> GuardarIdiomaAsync(string palabra)
        {
            Idioma idioma = new Idioma();
            idioma.IdiomaNuevo = palabra;


            var json = JsonConvert.SerializeObject(idioma);
            var container = new StringContent(json, Encoding.UTF8, "application/json");

            HttpResponseMessage response = await cliente.PostAsync("https://localhost:44349/api/idioma/post", container);
            //get data as Json string 
            string data = await response.Content.ReadAsStringAsync();
            //use JavaScriptSerializer from System.Web.Script.Serialization
            JavaScriptSerializer JSserializer = new JavaScriptSerializer();
            //deserialize to your class
            Idioma valor = JSserializer.Deserialize<Idioma>(data);

            return valor;
        }

        private static async Task<Traductor> GuardarPalabraAsync(string palabra, string traduccion)
        {
            Traductor traductor = new Traductor();
            traductor.PO = palabra;
            traductor.PT = traduccion;
            traductor.Cod_Idioma = idioma1.Id;


            var json = JsonConvert.SerializeObject(traductor);
            var container = new StringContent(json, Encoding.UTF8, "application/json");

            HttpResponseMessage response = await cliente.PostAsync("https://localhost:44349/api/traductor/post", container);
            //get data as Json string 
            string data = await response.Content.ReadAsStringAsync();
            //use JavaScriptSerializer from System.Web.Script.Serialization
            JavaScriptSerializer JSserializer = new JavaScriptSerializer();
            //deserialize to your class
            Traductor valor = JSserializer.Deserialize<Traductor>(data);

            return valor;
        }

        private static async Task<Traductor> BuscarPalabrasAsync(string palabra)
        {
            t1.PO = palabra;

            var json = JsonConvert.SerializeObject(t1);
            var container = new StringContent(json, Encoding.UTF8, "application/json");

            HttpResponseMessage response = await cliente.PutAsync("https://localhost:44349/api/traductor/GetWord",container);
            //get data as Json string 
            string data = await response.Content.ReadAsStringAsync();
            //use JavaScriptSerializer from System.Web.Script.Serialization
            JavaScriptSerializer JSserializer = new JavaScriptSerializer();
            //deserialize to your class
            Traductor valor = JSserializer.Deserialize<Traductor>(data);

            return valor;
        }

        private static async Task<Traductor> AgregarTraduccion(string palabra)
        {
            t1.PO = t1.PO;
            t1.PT = palabra;
            t1.Cod_Idioma = idioma1.Id;

            var json = JsonConvert.SerializeObject(t1);
            var container = new StringContent(json, Encoding.UTF8, "application/json");

            HttpResponseMessage response = await cliente.PostAsync("https://localhost:44349/api/traductor/post", container);
            //get data as Json string 
            string data = await response.Content.ReadAsStringAsync();
            //use JavaScriptSerializer from System.Web.Script.Serialization
            JavaScriptSerializer JSserializer = new JavaScriptSerializer();
            //deserialize to your class
            Traductor valor = JSserializer.Deserialize<Traductor>(data);

            return valor;
        }

        private static void imprimir(string v)
        {
            Console.WriteLine(v);
        }
    }
}
