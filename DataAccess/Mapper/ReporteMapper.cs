using DataAccess.Dao;
using EntitiesPOJO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Mapper
{
    public class ReporteMapper : EntityMapper, ISqlStatements, IObjectMapper
    {
        private const string DB_COL_USUARIO = "Usuario";
        private const string DB_COL_FRASE_SIN_TRADUCIR = "Frase_sin_traducir";
        private const string DB_COL_TRADUCCION = "Traduccion";
        private const string DB_COL_ID = "Id";
        private const string DB_COL_FECHA = "Fecha";

        public BaseEntity BuildObject(Dictionary<string, object> row)
        {
            var reportes = new Reporte
            {
                Id = GetIntValue(row, DB_COL_ID),
                Usuario = GetStringValue(row, DB_COL_USUARIO),
                Fecha = GetDateValue(row, DB_COL_FECHA),
                Frase_sin_traducir = GetStringValue(row, DB_COL_FRASE_SIN_TRADUCIR),
                Traduccion = GetStringValue(row, DB_COL_TRADUCCION)
            };

            return reportes;
        }

        public List<BaseEntity> BuildObjects(List<Dictionary<string, object>> lstRows)
        {
            var lstResults = new List<BaseEntity>();

            foreach (var row in lstRows)
            {
                var cliente = BuildObject(row);
                lstResults.Add(cliente);
            }

            return lstResults;
        }

        public SqlOperation GetCreateStatement(BaseEntity entity)
        {
            var operation = new SqlOperation { ProcedureName = "sp_generar_reporte_bitacora" };

            var r = (Reporte)entity;

            operation.AddVarcharParam(DB_COL_USUARIO, r.Usuario);
            operation.AddVarcharParam(DB_COL_FRASE_SIN_TRADUCIR, r.Frase_sin_traducir);
            operation.AddVarcharParam(DB_COL_TRADUCCION, r.Traduccion);

            return operation;
        }

        public SqlOperation GetDeleteStatement(BaseEntity entity)
        {
            throw new NotImplementedException();
        }

        public SqlOperation GetRetriveAllStatement()
        {
            var operation = new SqlOperation { ProcedureName = "sp_historico_traducciones" };

            return operation;
        }

        public SqlOperation GetRetriveStatement(BaseEntity entity)
        {
            throw new NotImplementedException();
        }

        public SqlOperation GetUpdateStatement(BaseEntity entity)
        {
            throw new NotImplementedException();
        }
    }
}
