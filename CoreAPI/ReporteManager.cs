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
    public class ReporteManager
    {
        private ReporteCrudFactory factory;

        public ReporteManager()
        {
            factory = new ReporteCrudFactory();
        }

        public void Create(Reporte reporte)
        {
            try
            {
                factory.Create(reporte);
            }
            catch(Exception ex)
            {
                ExecptionManager.GetInstance().Process(ex);
            }
        }

        public List<Reporte> RetrieveAll()
        {
            return factory.RetrieveAll<Reporte>();
        }
    }
}
