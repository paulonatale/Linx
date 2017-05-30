using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Conseg.Administracao.Domain.Core
{
    public interface IAuditableEntity
    {
        /// <summary>
        /// Gets or sets the date and time of instance creation
        /// </summary>
        DateTime CreatedOn { get; set; }

        /// <summary>
        /// Gets or sets the userid of instance creation
        /// </summary>
        int CreatedBy { get; set; }

        /// <summary>
        /// Gets or sets the date and time of instance creation
        /// </summary>
        DateTime UpdatedOn { get; set; }

        /// <summary>
        /// Gets or sets the userid of instance creation
        /// </summary>
        DateTime UpdatedBy { get; set; }
    }
}
