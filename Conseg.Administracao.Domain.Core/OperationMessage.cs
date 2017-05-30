using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Conseg.Administracao.Domain.Core
{
    public class OperationMessage
    {

        public OperationMessage(string description)
        {
            this.Description = description;
        }

        public OperationMessage(string description, MessageType messageType) : this(description)
        {
            MessageType = messageType;
        }

        public OperationMessage(string description, MessageType messageType, string code) : this(description, messageType)
        {
            Code = code;
        }

        public OperationMessage(string description, MessageType messageType, string code, int order) : this(description, messageType, code)
        {
            Order = order;
        }

        /// <summary>
        /// Gets or sets the order for this message.
        /// </summary>
        /// <value>
        /// The order for this message.
        /// </value>
        public int Order { get; set; } = 0;

        /// <summary>
        /// Gets or sets the code for errors.
        /// </summary>
        /// <value>
        /// The code for this error message.
        /// </value>
        public string Code { get; set; }

        public MessageType MessageType { get; set; } = MessageType.Info;

        /// <summary>
        /// Gets or sets the description for this error.
        /// </summary>
        /// <value>
        /// The description for this error.
        /// </value>
        public string Description { get; set; }

    }
}
