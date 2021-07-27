using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace wlabz.api.clientelle.Model
{
    public class Admin
    {

        /// <summary>
        /// Administrative Task Request List
        /// </summary>
        [System.Text.Json.Serialization.JsonConverter(typeof(System.Text.Json.Serialization.JsonStringEnumConverter))]
        public enum AdminRequests
        {
            SetupRepository = 1
        }

        /// <summary>
        /// Type of Administrive Request
        /// </summary>        
        public virtual AdminRequests AdminRequest { get; set; }
        

    }
}
