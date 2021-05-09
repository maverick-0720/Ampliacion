using DataAccess.Dao;
using EntitiesPOJO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Mapper
{
    public class TraductorMapper : EntityMapper, ISqlStatements, IObjectMapper
    {
        private const string DB_COL_ID = "Id";
        private const string DB_COL_EXISTENCIA_PALABRA = "Palabra";
        private const string DB_COL_IDIOMA = "Nombre_idioma";
        private const string DB_COL_PO = "PO";
        private const string DB_COL_PT = "PT";
        private const string DB_COL_TRADUCCION = "Traduccion";
        private const string DB_COL_COD_IDIOMA = "Cod_Idioma";
        private const string DB_COL_COD_POPULARIDAD = "Popularidad";


        public BaseEntity BuildObject(Dictionary<string, object> row)
        {
            var traductor = new Traductor
            {
                Id = GetIntValue(row, DB_COL_ID),
                Cod_Idioma = GetIntValue(row, DB_COL_COD_IDIOMA),
                PO = GetStringValue(row, DB_COL_PO),
                PT = GetStringValue(row, DB_COL_PT),
                Popularidad = GetIntValue(row, DB_COL_COD_POPULARIDAD)
            };

            return traductor;
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
            var operation = new SqlOperation { ProcedureName = "sp_palabra_nueva" };

            var t = (Traductor)entity;

            operation.AddVarcharParam(DB_COL_EXISTENCIA_PALABRA, t.PO);
            operation.AddVarcharParam(DB_COL_TRADUCCION, t.PT);
            operation.AddIntParam(DB_COL_COD_IDIOMA, t.Cod_Idioma);

            return operation;
        }

        public SqlOperation TranslateWordsStatement(BaseEntity entity)
        {
            var operation = new SqlOperation { ProcedureName = "sp_traducir_palabra" };

            var t = (Traductor)entity;

            operation.AddVarcharParam(DB_COL_EXISTENCIA_PALABRA, t.PO);
            operation.AddIntParam(DB_COL_COD_IDIOMA, t.Cod_Idioma);

            return operation;
        }

        public SqlOperation GetDeleteStatement(BaseEntity entity)
        {
            throw new NotImplementedException();
        }

        public SqlOperation GetRetriveAllStatement()
        {
            var operation = new SqlOperation { ProcedureName = "sp_devolver_100_palabras_mas_populares" };

            return operation;
        }

        public SqlOperation GetAllTheWords()
        {
            var operation = new SqlOperation { ProcedureName = "sp_devolver_todas_las_palabras_disponibles" };

            return operation;
        }

        public SqlOperation GetRetriveStatement(BaseEntity entity)
        {
            var operation = new SqlOperation { ProcedureName = "sp_existencia_palabra" };

            var t = (Traductor)entity;

            operation.AddVarcharParam(DB_COL_EXISTENCIA_PALABRA, t.PO);

            return operation;
        }

        public SqlOperation GetRetrieveDictionaryByLanguaje(BaseEntity entity)
        {
            var operation = new SqlOperation { ProcedureName = "sp_devolver_diccionario_por_idioma" };

            var t = (Traductor)entity;

            operation.AddIntParam(DB_COL_COD_IDIOMA, t.Cod_Idioma);

            return operation;
        }

        public SqlOperation GetUpdateStatement(BaseEntity entity)
        {
            var operation = new SqlOperation { ProcedureName = "sp_modificar_popularidad" };

            var t = (Traductor)entity;

            operation.AddIntParam(DB_COL_COD_IDIOMA, t.Cod_Idioma);
            operation.AddVarcharParam(DB_COL_EXISTENCIA_PALABRA, t.PO);

            return operation;
        }
    }
}
