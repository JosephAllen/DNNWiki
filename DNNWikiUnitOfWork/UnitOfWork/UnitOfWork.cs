using DotNetNuke.Data;
using DotNetNuke.Entities.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DotNetNuke.Modules.DNNWikiUnitOfWork.UnitOfWork
{
    public class UnitOfWork : IDisposable
    {
        private IDataContext _context;
        private bool disposed = false;
        private UserInfo _currentUser = null;

        #region properties

        internal IDataContext Context
        {
            get
            {
                return _context;
            }
        }

        internal UserInfo CurrentUser
        {
            get
            {
                return _currentUser;
            }
        }

        #endregion properties

        #region ctor

        public UnitOfWork()
        {
            _context = DataContext.Instance();

            if (HttpContext.Current.User.Identity.IsAuthenticated)
                _currentUser = UserController.GetCurrentUserInfo();
        }

        #endregion ctor

        #region transaction

        /// <summary>
        /// Starts a new transaction
        /// </summary>
        public void BeginTransaction()
        {
            _context.BeginTransaction();
        }

        /// <summary>
        /// Commits a transaction
        /// </summary>
        public void CommitTransaction()
        {
            _context.Commit();
        }

        /// <summary>
        /// Rollsback a transaction
        /// </summary>
        public void RollbackTransaction()
        {
            _context.RollbackTransaction();
        }

        #endregion transaction

        #region IDisposable Members

        public void Dispose()
        {
            Dispose(true);

            // This object will be cleaned up by the Dispose method. Therefore, you should call
            // GC.SupressFinalize to take this object off the finalization queue and prevent
            // finalization code for this object from executing a second time.
            GC.SuppressFinalize(this);
        }

        private void Dispose(bool disposing)
        {
            // Check to see if Dispose has already been called.
            if (!this.disposed)
            {
                // If disposing equals true, dispose all managed and unmanaged resources.
                if (disposing)
                {
                    // Clean up all managed resources
                    if (_context != null)
                    {
                        (this._context as IDisposable).Dispose();
                        _context = null;
                    }
                    _currentUser = null;
                }

                // Clean up all native resources

                // Note disposing has been done.
                disposed = true;
            }
        }

        ~UnitOfWork()
        {
            Dispose(false);
        }

        #endregion IDisposable Members
    }
}