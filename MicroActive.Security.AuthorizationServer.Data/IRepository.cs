using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Linq.Expressions;

namespace MicroActive.Security.AuthorizationServer.Data.Repositories
{
	public interface IRepository<T> : IDisposable
		where T : class
	{
		#region Public Methods and Operators

		IQueryable<T> All(params string[] includes);

		bool Any(Expression<Func<T, bool>> predicate);

		bool Contains(Expression<Func<T, bool>> predicate);

		int Count(Expression<Func<T, bool>> predicate);

		T Create(T t);

		void Delete(T t, bool batch = true);

		int Delete(Expression<Func<T, bool>> predicate, bool batch = true);

		IQueryable<T> Filter(Expression<Func<T, bool>> predicate, params string[] includeStrings);

		IQueryable<T> Filter<TKey>(Expression<Func<T, bool>> filter, out int total, int index = 0, int size = 50);

		T Find(params object[] keys);

		T Find(Expression<Func<T, bool>> predicate, params string[] includes);

		int Update(T t);

		void SaveBatch();

//		List<T> ExecuteQuery(string sqlQuery, params SqlParameter[] sqlParams);

		#endregion
	}
}
