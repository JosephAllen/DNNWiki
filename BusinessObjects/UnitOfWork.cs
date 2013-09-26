#region Copyright

//
// DotNetNuke� - http://www.dotnetnuke.com Copyright (c) 2002-2013 by DotNetNuke Corporation
//
// Permission is hereby granted, free of charge, to any person obtaining a copy of this software and
// associated documentation files (the "Software"), to deal in the Software without restriction,
// including without limitation the rights to use, copy, modify, merge, publish, distribute,
// sublicense, and/or sell copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in all copies or
// substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT
// NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
// NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM,
// DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.

#endregion Copyright

using DotNetNuke.Data;
using DotNetNuke.Entities.Users;
using System;
using System.Web;

namespace DotNetNuke.Wiki.BusinessObjects
{
    public class UnitOfWork : IDisposable
    {
        #region Variables

        private IDataContext _context;
        private bool disposed = false;
        private UserInfo _currentUser = null;

        #endregion Variables

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