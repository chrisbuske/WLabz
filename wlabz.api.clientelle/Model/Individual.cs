using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WLabz.api.clientelle.Model
{
    /// <summary>
    /// Entity information associated to an individual Entity
    /// </summary>
    public class Individual
    {

        /// <summary>
        /// Types of gender 
        /// </summary>
        [System.Text.Json.Serialization.JsonConverter(typeof(System.Text.Json.Serialization.JsonStringEnumConverter))]
        public enum GenderTypes : byte
        {
            Male = 1,
            Female = 2
        }

        /// <summary>
        /// Unique Entity ID
        /// </summary>
        public virtual Guid EntityID { get; set; }
        /// <summary>
        /// Identification Number
        /// </summary>
        public virtual string IdentityNumber { get; set; }
        /// <summary>
        /// Country of Issue
        /// </summary>
        public virtual string CountryOfIssue { get; set; }
        /// <summary>
        /// Individuals First Name
        /// </summary>
        public virtual string FirstName { get; set; }
        /// <summary>
        /// Individuals Middle Names
        /// </summary>
        public virtual string MiddleNames { get; set; }
        /// <summary>
        /// Individuals Surname
        /// </summary>
        public virtual string Surname { get; set; }
        /// <summary>
        /// Individuals Date Of Birth
        /// </summary>
        public virtual DateTime DOB { get; set; }
        /// <summary>
        /// Gender
        /// </summary>
        public virtual GenderTypes Gender { get; set; }
    }
}
