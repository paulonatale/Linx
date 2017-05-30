using Conseg.Administracao.Domain.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http.ModelBinding;

namespace Conseg.Administracao.Framework
{
    public static class OperationResultExtentions
    {
        public static ModelStateDictionary ToModelState(this OperationResult operationResult)
        {
            ModelStateDictionary modelState = new ModelStateDictionary();

            var messages = operationResult.GetMessages
                .Where(x => x.MessageType == MessageType.Danger ||
                x.MessageType == MessageType.Error ||
                x.MessageType == MessageType.FatalError ||
                x.MessageType == MessageType.Warning);

            if (messages.Count() > 0)
            {
                foreach (OperationMessage message in messages.OrderBy(x => x.Order))
                {
                    modelState.AddModelError("", message.Description);
                }
            }

            return modelState;
        }
    }
}
