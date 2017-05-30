using Conseg.Administracao.Domain.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Conseg.Administracao.Domain.Core
{
    public class AuditableEntity : BaseEntity, IAuditableEntity
    {
        /// <summary>
        /// Gets or sets the date and time of instance creation
        /// </summary>
        public DateTime CreatedOn { get; set; }

        /// <summary>
        /// Gets or sets the userid of instance creation
        /// </summary>
        public int CreatedBy { get; set; }

        /// <summary>
        /// Gets or sets the date and time of instance creation
        /// </summary>
        public DateTime UpdatedOn { get; set; }

        /// <summary>
        /// Gets or sets the userid of instance creation
        /// </summary>
        public DateTime UpdatedBy { get; set; }
    }
}
