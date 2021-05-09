using DataAccess.Dao;
using EntitiesPOJO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Mapper
{
    public class IdiomaMapper : EntityMapper, ISqlStatements, IObjectMapper
    {
        private const string DB_COL_IDIOMA = "Nombre_idioma";
        private const string DB_COL_ID_IDIOMA = "Id";
        private const string DB_COL_IDIOMAS = "IdiomaNuevo";
        private const string DB_COL_POPULARIDAD = "Popularidad";


        public BaseEntity BuildObject(Dictionary<string, object> row)
        {
            var idioma = new Idioma
            {
                Id = GetIntValue(row,DB_COL_ID_IDIOMA),
                IdiomaNuevo = GetStringValue(row,DB_COL_IDIOMAS),
                Popularidad = GetIntValue(row,DB_COL_POPULARIDAD)
            };

            return idioma;
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
            var operation = new SqlOperation { ProcedureName = "sp_registrar_idioma" };

            var i = (Idioma)entity;
            operation.AddVarcharParam(DB_COL_IDIOMA, i.IdiomaNuevo);

            return operation;
        }

        public SqlOperation GetDeleteStatement(BaseEntity entity)
        {
            throw new NotImplementedException();
        }

        public SqlOperation GetRetriveAllStatement()
        {
            var operation = new SqlOperation { ProcedureName = "sp_devolver_todos_los_idiomas" };

            return operation;
        }

        public SqlOperation GetRetriveStatement(BaseEntity entity)
        {
            var operation = new SqlOperation { ProcedureName = "sp_existencia_idioma" };

            var i = (Idioma)entity;
            operation.AddVarcharParam(DB_COL_IDIOMA,i.IdiomaNuevo);

            return operation;
        }

        public SqlOperation GetUpdateStatement(BaseEntity entity)
        {
            var operation = new SqlOperation { ProcedureName = "sp_modificar_popularidad_idioma" };

            var i = (Idioma)entity;
            operation.AddVarcharParam(DB_COL_IDIOMA, i.IdiomaNuevo);

            return operation;
        }
    }
}
