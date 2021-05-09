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
    public class IdiomaManager
    {
        private IdiomaCrudFactory factory;

        public IdiomaManager()
        {
            factory = new IdiomaCrudFactory();
        }

        public void Create(Idioma idioma)
        {
            try
            {
                var c = factory.Retrieve<Idioma>(idioma);

                if (c != null)
                {
                    throw new BussinessException(1);
                }
                else
                {
                    factory.Create(idioma);
                }
            }
            catch (Exception ex)
            {
                ExecptionManager.GetInstance().Process(ex);
            }
        }

        public List<Idioma> RetrieveAll()
        {
            return factory.RetrieveAll<Idioma>();
        }

        public Idioma RetrieveById(Idioma idioma)
        {
            Idioma i = null;

            try
            {
                i = factory.Retrieve<Idioma>(idioma);
                if (i == null)
                {
                    throw new BussinessException(2);
                }
                else
                {
                    factory.Update(i);
                }
            }
            catch(Exception ex)
            {
                ExecptionManager.GetInstance().Process(ex);
            }

            return i;
        }
    }
}
