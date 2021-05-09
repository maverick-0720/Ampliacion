using DataAccess.Dao;
using DataAccess.Mapper;
using EntitiesPOJO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Crud
{
    public class TraductorCrudFactory : CrudFactory
    {
        TraductorMapper mapper;

        public TraductorCrudFactory() : base()
        {
            mapper = new TraductorMapper();
            dao = SqlDao.GetInstance();
        }

        public override void Create(BaseEntity entity)
        {
            var cliente = (Traductor)entity;
            var sqlOperation = mapper.GetCreateStatement(cliente);
            dao.ExecuteProcedure(sqlOperation);
        }

        public T TranslateWord<T>(BaseEntity entity)
        {
            var sqlOperation = mapper.TranslateWordsStatement(entity);
            var lstResult = dao.ExecuteQueryProcedure(sqlOperation);
            var dic = new Dictionary<string, object>();
            if (lstResult.Count > 0)
            {
                dic = lstResult[0];
                var objs = mapper.BuildObject(dic);
                return (T)Convert.ChangeType(objs, typeof(T));
            }

            return default(T);
        }

        public override void Delete(BaseEntity entity)
        {
            throw new NotImplementedException();
        }

        public override T Retrieve<T>(BaseEntity entity)
        {
            var sqlOperation = mapper.GetRetriveStatement(entity);
            var lstResult = dao.ExecuteQueryProcedure(sqlOperation);
            var dic = new Dictionary<string, object>();
            if (lstResult.Count > 0)
            {
                dic = lstResult[0];
                var objs = mapper.BuildObject(dic);
                return (T)Convert.ChangeType(objs, typeof(T));
            }

            return default(T);
        }

        public List<T> RetrieveAllWords<T>()
        {
            var lstCliente = new List<T>();

            var lstResult = dao.ExecuteQueryProcedure(mapper.GetAllTheWords());
            var dic = new Dictionary<string, object>();
            if (lstResult.Count > 0)
            {
                var objs = mapper.BuildObjects(lstResult);
                foreach (var c in objs)
                {
                    lstCliente.Add((T)Convert.ChangeType(c, typeof(T)));
                }
            }

            return lstCliente;
        }

        public List<T> RetrieveDictionary<T>(BaseEntity entity)
        {
            var lstCliente = new List<T>();

            var lstResult = dao.ExecuteQueryProcedure(mapper.GetRetrieveDictionaryByLanguaje(entity));
            var dic = new Dictionary<string, object>();
            if (lstResult.Count > 0)
            {
                var objs = mapper.BuildObjects(lstResult);
                foreach (var c in objs)
                {
                    lstCliente.Add((T)Convert.ChangeType(c, typeof(T)));
                }
            }

            return lstCliente;
        }

        public override List<T> RetrieveAll<T>()
        {
            var lstCliente = new List<T>();

            var lstResult = dao.ExecuteQueryProcedure(mapper.GetRetriveAllStatement());
            var dic = new Dictionary<string, object>();
            if (lstResult.Count > 0)
            {
                var objs = mapper.BuildObjects(lstResult);
                foreach (var c in objs)
                {
                    lstCliente.Add((T)Convert.ChangeType(c, typeof(T)));
                }
            }

            return lstCliente;
        }

        public override void Update(BaseEntity entity)
        {
            var cliente = (Traductor)entity;
            dao.ExecuteProcedure(mapper.GetUpdateStatement(cliente));
        }
    }
}
