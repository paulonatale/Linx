using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Conseg.Administracao.Domain.Core
{
    public class OperationResult
    {

        public object Entity = null;
        private List<OperationMessage> _messages = new List<OperationMessage>();

        public bool Succeeded
        {
            get
            {
                return 0 == _messages.Count(x => x.MessageType == MessageType.Error ||
                x.MessageType == MessageType.Danger || x.MessageType == MessageType.FatalError);
            }
        }

        public void Add(OperationMessage message)
        {
            _messages.Add(message);
        }

        public void AddToEnd(OperationMessage message)
        {
            message.Order = _messages.Count + 1;

            _messages.Add(message);
        }

        public List<OperationMessage> GetMessages
        {
            get
            {
                return _messages;
            }
        }

        /// <summary>
        /// Converts the value of the current <see cref="OperationResult"/> object to its equivalent string representation.
        /// </summary>
        /// <returns>A string representation of the current <see cref="OperationResult"/> object.</returns>
        /// <remarks>
        /// If the operation was successful the ToString() will return "Succeeded" otherwise it returned 
        /// "Error : " followed by a comma delimited list of error codes from its <see cref="Errors"/> collection, if any.
        /// </remarks>
        public override string ToString()
        {
            if (Succeeded)
            {
                return "Succeeded";
            }
            else
            {
                StringBuilder stringBuilder = new StringBuilder();
                bool first = true;
                if (_messages.Count > 0)
                {
                    foreach (var error in _messages.Where(x => x.MessageType == MessageType.Error ||
                    x.MessageType == MessageType.Danger || x.MessageType == MessageType.FatalError))
                    {
                        stringBuilder.Append(first ? "Error: " : ", ");
                        stringBuilder.Append(error.Code + " : " + error.Description);
                    }
                }
                return stringBuilder.ToString();
            }
        }




    }

}
