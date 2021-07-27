using System;
using System.Collections.Generic;
using System.Text;
namespace WLabz.Repository
{
    /// <summary>
    /// Configuration settings for Fluent Hibernate...
    /// </summary>
    public class FluentHibernateSettings
    {
      
        /// <summary>
        /// Datastore connection string. Currently setup for MSSQL...
        /// </summary>
        public string ConnectionString { get; set; }

        /// <summary>
        /// String representation of the type of FluentConfiguration CurrentSessionContext...
        /// </summary>
        public string CurrentSessionContext { get; set; }

        /// <summary>
        /// Default Schema to be used...
        /// </summary>
        public string DefaultSchema { get; set; }
    }
}
