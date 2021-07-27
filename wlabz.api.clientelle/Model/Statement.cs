using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WLabz.api.clientelle.Model
{

    /// <summary>
    /// Provides a statement for an accounting period...
    /// </summary>
    public class Statement
    {

        #region Properties...

        /// <summary>
        /// Unique Account Number
        /// </summary>
        public virtual long AccountNumber { get; set; }

        /// <summary>
        /// Date and Time of the statement
        /// </summary>
        public virtual DateTime StatementDate { get; set; }

        /// <summary>
        /// Closing Balance for the statement Period
        /// </summary>
        public virtual decimal ClosingBalance { get; set; }

        /// <summary>
        /// Total Debits for the sataement period
        /// </summary>
        public virtual decimal TotalDebits { get; set; }

        /// <summary>
        /// Total Credits for the statement period
        /// </summary>
        public virtual decimal TotalCredits { get; set; }


        #endregion

        #region Overrides...

        public override bool Equals(object obj)
        {
            if (obj is not Statement other) return false;
            if (ReferenceEquals(this, other)) return true;

            return this.AccountNumber == other.AccountNumber &&
                this.StatementDate == other.StatementDate;
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int hash = GetType().GetHashCode();
                hash = (hash * 31) ^ AccountNumber.GetHashCode();
                hash = (hash * 31) ^ StatementDate.GetHashCode();

                return hash;
            }
        }

        #endregion
    }
}
