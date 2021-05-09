using EntitiesPOJO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using System.Threading.Tasks;
using System.IO;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using Exceptions;

namespace Exceptions
{
    public class ExecptionManager
    {
        public string PATH = "";
        public string logsNames = "";


        private static ExecptionManager instance;

        private static Dictionary<int, string> messages = new Dictionary<int, string>();

        public ExecptionManager()
        {

            PATH = ConfigurationManager.AppSettings.Get("LOG_PATH");
            logsNames = ConfigurationManager.AppSettings.Get("JSON_LOGS_NAMES");
            LoadMessages();

        }

        public static ExecptionManager GetInstance()
        {
            if (instance == null)
            {
                instance = new ExecptionManager();

            }


            return instance;
        }

        public void Process(Exception ex)
        {

            var bex = new BussinessException();


            if (ex.GetType() == typeof(BussinessException))
            {
                bex = (BussinessException)ex;
                bex.ExceptionDetails = GetMessage(bex).Message;
            }
            else
            {
                bex = new BussinessException(0, ex);
            }

            ProcessBussinesException(bex);

        }

        private void ProcessBussinesException(BussinessException bex)
        {
            var today = DateTime.Now.ToString("yyyyMMdd_hh");
            var logName = PATH + today + "_" + "log.txt";

            var message = bex.ExceptionDetails + "\n" + bex.StackTrace + "\n";

            //if (bex.InnerException!=null)
            //    message += bex.InnerException.Message + "\n" + bex.InnerException.StackTrace;

            using (StreamWriter w = File.AppendText(logName))
            {
                Log(message, w);
            }

            bex.AppMessage = GetMessage(bex);

            throw bex;

        }

        public ApplicationMessage GetMessage(BussinessException bex)
        {

            var appMessage = new ApplicationMessage
            {
                Message = "Message not found!"
            };

            if (messages.ContainsKey(bex.ExceptionId))
            {
                appMessage.Id = bex.ExceptionId;
                appMessage.Message = messages[bex.ExceptionId];
            }


            return appMessage;

        }

        private void LoadMessages()
        {

            //Read the JSON file
            using (StreamReader r = new StreamReader(logsNames))
            {
                var json = r.ReadToEnd();

                List<ApplicationMessage> list = JsonConvert.DeserializeObject<List<ApplicationMessage>>(json);
                foreach (var appMessage in list)
                {
                    messages.Add(appMessage.Id, appMessage.Message);
                }
            }
        }

        private void Log(string logMessage, TextWriter w)
        {
            w.Write("\r\nLog Entry : ");
            w.WriteLine("{0} {1}", DateTime.Now.ToLongTimeString(),
                DateTime.Now.ToLongDateString());
            w.WriteLine("  :");
            w.WriteLine("  :{0}", logMessage);
            w.WriteLine("-------------------------------");
        }
    }
}
