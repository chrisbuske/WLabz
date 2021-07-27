using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WLabz.api.clientelle.Model
{
    /// <summary>
    /// Entity of Type Organisation
    /// </summary>
    public class Organisation
    {
        /// <summary>
        /// Unique Entity ID
        /// </summary>
        public virtual Guid EntityID { get; set; }
        /// <summary>
        /// Registration Number
        /// </summary>
        public virtual string RegistrationNamber { get; set; }
        /// <summary>
        /// Country of Issue
        /// </summary>
        public virtual string CountryOfIssue { get; set; }
        /// <summary>
        /// Organisations Name
        /// </summary>
        public virtual string OrganisationName { get; set; }
    }
}
