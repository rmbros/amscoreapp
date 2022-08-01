namespace AmsApp.Helpers
{
    using System;
    using System.Collections.Generic;
    using System.Data.Common;
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.EntityFrameworkCore;

    public static class SqlQueryExtensions
    {
        public static IList<T> SqlQuery<T>(this DbContext db, Func<T> anonType, string sql, params object[] parameters)
            where T : class
            => SqlQuery<T>(db, sql, parameters);

        public static List<T> SqlQuery<T>(this DbContext db, string sql, params object[] parameters)
            where T : class
        {
            using var db2 = new ContextForQueryType<T>(db.Database.GetDbConnection());
            return db2.Set<T>().FromSqlRaw(sql, parameters).ToList();
        }

        public async static Task<List<T>> SqlQueryAsync<T>(this DbContext db, string sql, params object[] parameters)
            where T : class
        {
            using var db2 = new ContextForQueryType<T>(db.Database.GetDbConnection());
            return await db2.Set<T>().FromSqlRaw(sql, parameters).ToListAsync();
        }

        private sealed class ContextForQueryType<T> : DbContext
            where T : class
        {
            private readonly DbConnection connection;

            public ContextForQueryType(DbConnection connection)
            {
                this.connection = connection;
            }

            protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
            {
                optionsBuilder.UseSqlServer(this.connection, options => options.EnableRetryOnFailure());

                base.OnConfiguring(optionsBuilder);
            }

            protected override void OnModelCreating(ModelBuilder modelBuilder)
            {
                modelBuilder.Entity<T>().HasNoKey();
                base.OnModelCreating(modelBuilder);
            }
        }
    }
}
