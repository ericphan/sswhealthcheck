using System;
using System.Data.Entity;
using System.Web;

namespace SSW.Framework.Data.EF
{
    public interface IDbContextManager : IDisposable
    {
        bool HasContext { get; }
        DbContext Context { get; }
    }

    public interface IDbContextManager<T> : IDbContextManager
    {
    }

    public class DbContextManager<T> : IDbContextManager<T> 
        where T : DbContext
    {
        private static log4net.ILog log =
            log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        IDbContextFactory<T> _factory;

        public DbContextManager(IDbContextFactory<T> factory)
        {
            _factory = factory;
        }

        T _context = default(T);
        public DbContext Context
        {
            get
            {
                if (_context == null)
                {
                    var requestId = HttpContext.Current != null ? HttpContext.Current.GetHashCode() : 0;
                    log.Debug(requestId+" context manager "+typeof(T).Name+" hash: "+this.GetHashCode()+" building context ");
                    _context = _factory.Build();
                }
                return _context as DbContext;
            }
        }

        public bool HasContext
        {
            get
            {
                return _context != null;
            }
        }

        public void Dispose()
        {
            if (HasContext)
            {
                var requestId = HttpContext.Current != null ? HttpContext.Current.GetHashCode() : 0;
                log.Debug(requestId+" context manager " + typeof(T).Name + " hash: " + this.GetHashCode() + " disposing context ");
                _context.Dispose();
                _context = null;
            }
        }
    }
}
