using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

using MicroActive.Security.AuthorizationServer.Data;
using Microsoft.EntityFrameworkCore;
using MicroActive.Security.AuthorizationServer.Data.Models;

namespace MicroActive.Security.AuthorizationServer.Data.Repositories
{
	public class DataRepository<TObject> : IRepository<TObject>
		where TObject : class
	{
		#region Fields

		protected IdentityContext Context = null;

		#endregion

		#region Constructors and Destructors

		public DataRepository(DbContextOptions<IdentityContext> options)
		{
			this.Context = new IdentityContext(options);
			//this.Context.Database.Log = s => System.Diagnostics.Debug.WriteLine(s);
		}

		public DataRepository(IdentityContext context)
		{
			this.Context = context;
		}
		#endregion

		#region Properties

		protected DbSet<TObject> DbSet
		{
			get
			{
				return this.Context.Set<TObject>();
			}
		}

		#endregion

		#region Public Methods and Operators
		public virtual IQueryable<TObject> All(params string[] includes)
		{
			var all = this.DbSet.AsQueryable();
			foreach (var includeClause in includes)
			{
				all.Include(includeClause);
			}
			return all;
		}

		public bool Any(Expression<Func<TObject, bool>> predicate)
		{
			return this.DbSet.Any(predicate);
		}

		public bool Contains(Expression<Func<TObject, bool>> predicate)
		{
			return this.DbSet.Count(predicate) > 0;
		}

		public int Count(Expression<Func<TObject, bool>> predicate)
		{
			return this.DbSet.Count(predicate);
		}

		public virtual TObject Create(TObject TObject)
		{
			var newEntry = this.DbSet.Add(TObject);
			this.Context.SaveChanges();

			return newEntry.Entity;
		}

		public virtual void Delete(TObject TObject, bool batch = true)
		{
			this.DbSet.Remove(TObject);

			if (!batch)
			{
				Context.SaveChanges();
			}
		}

		public virtual int Delete(Expression<Func<TObject, bool>> predicate, bool batch = true)
		{
			IQueryable<TObject> objects = this.Filter(predicate);
			foreach (TObject obj in objects)
			{
				this.DbSet.Remove(obj);
			}

			if (!batch)
			{
				Context.SaveChanges();
			}

			return 0;
		}

		public virtual void Delete(object obj, bool batch = true)
		{
			var entity = this.Context.Entry(obj);
			entity.State = Microsoft.EntityFrameworkCore.EntityState.Deleted;
			if (!batch)
			{
				Context.SaveChanges();
			}
		}

		public void SaveBatch()
		{
			Context.SaveChanges();
		}

		public void Dispose()
		{
			if (this.Context != null)
			{
				this.Context.Dispose();
			}
		}

		public virtual IQueryable<TObject> Filter(Expression<Func<TObject, bool>> predicate, params string[] includes)
		{
			IQueryable<TObject> dbSet = this.DbSet;
			foreach (var includeClause in includes)
			{
				dbSet.Include(includeClause);
			}
			return dbSet.Where(predicate).AsQueryable();
		}

		public IQueryable<TObject> Filter<TKey>(
			Expression<Func<TObject, bool>> filter,
			out int total,
			int index = 0,
			int size = 50)
		{
			int skipCount = index * size;
			IQueryable<TObject> resetSet = filter != null
											   ? this.DbSet.Where(filter).AsQueryable()
											   : this.DbSet.AsQueryable();
			resetSet = skipCount == 0 ? resetSet.Take(size) : resetSet.Skip(skipCount).Take(size);
			total = resetSet.Count();
			return resetSet.AsQueryable();
		}

		public virtual TObject Find(params object[] keys)
		{
			return this.DbSet.Find(keys);
		}

		public virtual TObject Find(Expression<Func<TObject, bool>> predicate, params string[] includes)
		{
			// Add includes to query
			IQueryable<TObject> query = this.DbSet;
			query = includes.Aggregate(query, (current, includeClause) => current.Include(includeClause));

			return query.FirstOrDefault(predicate);
		}

		public int Update(TObject TObject)
		{
			//Add logging
			Console.WriteLine("Update begun");

			var entry = this.Context.Entry(TObject);
			this.DbSet.Attach(TObject);
			entry.State = EntityState.Modified;

			Console.WriteLine("Update end");

			return this.Context.SaveChanges();
		}

		#endregion
	}
}
