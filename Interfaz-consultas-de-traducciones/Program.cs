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

namespace Interfaz_consultas_de_traducciones
{
    class Program
    {
        public static HttpClient cliente { get; set; }

        public static List<Idioma> listaIdiomas = new List<Idioma>();

        public static List<Traductor> listaTraductor = new List<Traductor>();

        public static Idioma idioma1 = new Idioma();

        public static Traductor t1 = new Traductor();


        static void Main(string[] args)
        {
            menu();
        }

        private static void menu()
        {
            Boolean salir = false;
            int opcion; //Guardaremos la opcion del usuario
            while (!salir)
            {
                Console.WriteLine("==================================================================");
                Console.WriteLine(
                        "= " + " 1. Idiomas disponibles\n" +
                        "= " + " 2. Idioma mas popular\n" +
                        "= " + " 3. 100 palabras mas populares\n" +
                        "= " + " 4. Diccionario segun idioma solicitado\n" +
                        "= " + " 5. Salir\n" +
                                "==================================================================");
                Console.WriteLine("   Seleccione una de las opciones");
                opcion = Int32.Parse(Console.ReadLine());
                switch (opcion)
                {
                    case 1:
                        listarIdiomas();
                        break;
                    case 2:
                        IdiomaPopular();
                        break;
                    case 3:
                        palabrasMasPopulares();
                        break;
                    case 4:
                        diccionarioIdioma();
                        break;
                    case 5:
                        Console.WriteLine("Gracias por preferirnos");
                        salir = true;
                        break;
                }
            }
        }

        private static void palabrasDisponiblesXIdioma()
        {
            //string palabra = "";
            //string hilera = "";
            //List<Traductor> palabrasDisponiblesIdiomas = new List<Traductor>();
            //Traductor traductor = new Traductor();
            //int contador = 0;

            //mainAsyncListarTraductor().Wait();
            //mainAsyncListarIdioma().Wait();

            //Console.WriteLine("Estas son las palabras disponibles por idioma");
            //Console.WriteLine("");

            //foreach (var buscardor in listaTraductor)
            //{

            //    if (palabrasDisponiblesIdiomas.Find(palabra) != null)
            //    {
            //        palabra = buscardor.PO;
            //        palabrasDisponiblesIdiomas.Add("Palabra: ", palabra);

            //        foreach (var ayudante in listaIdiomas)
            //        {
            //            if (!(palabrasDisponiblesIdiomas.ContainsValue(ayudante.IdiomaNuevo)))
            //            {

            //                mainAsyncTraducir(palabra, ayudante.Id).Wait();

            //                if (t1 != null)
            //                {
            //                    if ((t1.Id != 0) && t1.Popularidad > 0)
            //                    {
            //                        if (t1.PO != null && t1.PT != null)
            //                        {
            //                            hilera += ayudante.IdiomaNuevo + ",";
            //                            contador++;
            //                        }
            //                    }
            //                }
            //            }

                        
            //        }
            //    }
               
            //}

            //foreach (var encontrador in palabrasDisponiblesIdiomas)
            //{
            //    Console.WriteLine(encontrador.Key + " : " + encontrador.Value);
            //}

        }

        private static void diccionarioIdioma()
        {
            string idioma = "";
            int id = 0;
            bool exito = false;
            Console.WriteLine("Digite el idioma");
            idioma = Console.ReadLine();

            mainAsyncListarIdioma().Wait();

            foreach (var element in listaIdiomas)
            {
                if (element.IdiomaNuevo.ToLower().Equals(idioma.ToLower()))
                {
                    exito = true;
                    id = element.Id;
                    break;
                }

            }

            if (exito)
            {
                mainAsyncListarTraductor().Wait();
                foreach (var element in listaTraductor)
                {
                    if (element.Cod_Idioma == id)
                    {
                        Console.WriteLine("Palabras en: " + idioma);
                        Console.WriteLine("");
                        Console.WriteLine(element.PT);
                    }

                }
            }
            else
            {
                Console.WriteLine("Este idioma no existe, por favor ir a la consola del bot de traductor para registrar el idioma y sus traducciones");
            }

        }

        private static async Task mainAsyncListarTraductor()
        {
            startCliente();
            listaTraductor = await GetListarPalabras();
        }

        private static async Task<List<Traductor>> GetListarPalabras()
        {
            using (HttpResponseMessage response = await cliente.GetAsync("https://localhost:44349/api/traductor/RetrieveAll"))

            {

                if (response.IsSuccessStatusCode)
                {

                    //Dictionary<string, Idioma> operationResult1 = await response.Content.ReadAsAsync<Dictionary<string, Idioma>>();
                    var json = await response.Content.ReadAsStringAsync();
                    var data = JsonConvert.DeserializeObject<List<Traductor>>(json);
                    return data;
                }
                else
                {
                    throw new Exception(response.ReasonPhrase);
                }
            }
        }

        private static void palabrasMasPopulares()
        {
            Console.WriteLine("Las 100 palabras más populares son: ");
            mainAsyncListarPalabrasImportantes().Wait();
            foreach (var element in listaTraductor)
            {
                Console.WriteLine(element.Id + " " + element.PO);
            }
        }

        private static async Task mainAsyncListarPalabrasImportantes()
        {
            startCliente();
            listaTraductor = await GetListarPalabrasImportantes();
        }

        public static async Task mainAsyncTraducir(string palabra, int id)
        {
            startCliente();
            t1 = await GetPalabraTraducidaAsync(palabra, id);
        }

        private static async Task<Traductor> GetPalabraTraducidaAsync(string palabra, int id)
        {
            Traductor traductor = new Traductor();
            traductor.Cod_Idioma = id;
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

        private static async Task<List<Traductor>> GetListarPalabrasImportantes()
        {
            using (HttpResponseMessage response = await cliente.GetAsync("https://localhost:44349/api/HundredFamous/RetrieveHundredFamous"))

            {

                if (response.IsSuccessStatusCode)
                {

                    //Dictionary<string, Idioma> operationResult1 = await response.Content.ReadAsAsync<Dictionary<string, Idioma>>();
                    var json = await response.Content.ReadAsStringAsync();
                    var data = JsonConvert.DeserializeObject<List<Traductor>>(json);
                    return data;
                }
                else
                {
                    throw new Exception(response.ReasonPhrase);
                }
            }
        }

        private static void IdiomaPopular()
        {
            Console.WriteLine("El idioma más popular es: ");
            int mayor = 0;
            string nombre = "";
            mainAsyncListarIdioma().Wait();
            foreach (var element in listaIdiomas)
            {
                if (element.Popularidad > mayor)
                {
                    mayor = element.Popularidad;
                    nombre = element.IdiomaNuevo;
                }
            }
            Console.WriteLine(nombre);
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
    }
}
