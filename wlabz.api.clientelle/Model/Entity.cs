using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WLabz.api.clientelle.Model
{
    /// <summary>
    /// The entity class provides the base entity to which clientelle information and accounts are associated...
    /// </summary>
    public class Entity
    {
        /// <summary>
        /// Types of account owner entities 
        /// </summary>
        [System.Text.Json.Serialization.JsonConverter(typeof(System.Text.Json.Serialization.JsonStringEnumConverter))]
        public enum EntityTypes
        {
            /// <summary>
            /// Individual Human
            /// </summary>
            Individual = 1,
            /// <summary>
            /// Organisation or Multi-Party
            /// </summary>
            Organisation = 2
        }

        /// <summary>
        /// Unique Entity ID
        /// </summary>
        public virtual Guid EntityID { get; set; }
        /// <summary>
        /// Type of Entity represented
        /// </summary>        
        public virtual EntityTypes EntityType { get; set; }

        /// <summary>
        /// List of associate accounts...
        /// </summary>
        public virtual IList<Account> Accounts { get; set; }

        /// <summary>
        /// Personal Information  for Individual EntityTypes
        /// </summary>
        public virtual Individual Individual { get; set; }

        /// <summary>
        /// Organisational Information for Organisation EntityTypes
        /// </summary>
        public virtual Organisation Organisation { get; set; }

        public Entity()
        {
            this.Accounts = new List<Account>();
        }

    }
}
