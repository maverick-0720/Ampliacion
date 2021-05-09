using DataAccess.Crud;
using DataAccess.Dao;
using EntitiesPOJO;
using Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreAPI
{
    public class TraductorManager
    {
        private TraductorCrudFactory factory;

        public TraductorManager()
        {
            factory = new TraductorCrudFactory();
        }

        public void Create(Traductor traductor)
        {
            try
            {
                factory.Create(traductor);
            }
            catch(Exception ex)
            {
                ExecptionManager.GetInstance().Process(ex);
            }
        }

        public Traductor RetrieveById(Traductor traductor)
        {
            Traductor i = null;

            try
            {
                i = factory.Retrieve<Traductor>(traductor);
                if (i != null)
                {
                    factory.Update(i);
                }
            }
            catch (Exception ex)
            {
                ExecptionManager.GetInstance().Process(ex);
            }

            return i;
        }

        public Traductor TranslateWord(Traductor traductor)
        {
            Traductor i = null;

            try
            {
                i = factory.TranslateWord<Traductor>(traductor);
                if (i != null)
                {
                    factory.Update(i);
                }
            }
            catch (Exception ex)
            {
                ExecptionManager.GetInstance().Process(ex);
            }

            return i;
        }

        public List<Traductor> RetrieveAll()
        {
            return factory.RetrieveAllWords<Traductor>();
        }

        public List<Traductor> RetrieveHundredFamous()
        {
            return factory.RetrieveAll<Traductor>();
        }

        public List<Traductor> RetrieveByDictionary(Traductor traductor)
        {
            return factory.RetrieveDictionary<Traductor>(traductor);
        }

        public void Update(Traductor traductor)
        {
            factory.Update(traductor);
        }


    }
}
