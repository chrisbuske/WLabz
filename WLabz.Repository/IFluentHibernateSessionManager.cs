using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;

namespace WLabz.Repository
{
    /// <summary>
    /// Interface for FleuntHibernate Session Management...
    /// </summary>
    public interface IFluentHibernateSessionManager
    {
        NHibernate.ISession OpenSession(Assembly assembly, bool buildSchema = false);
    }
}
