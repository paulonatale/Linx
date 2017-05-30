using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Web;
using System.Web.Routing;
using System.Web.SessionState;
using Administracao.GenericController;

namespace Conseg.Administracao.Framework
{
    public class ControllerFactory : DefaultControllerFactory
    {
        private readonly IAssemblyService _assemblyService;

        public ControllerFactory(IAssemblyService assemblyService)
        {
            _assemblyService = assemblyService;
        }

        public override IController CreateController(RequestContext requestContext, string controllerName)
        {
            return base.CreateController(requestContext, controllerName);
        }
        protected override Type GetControllerType(RequestContext requestContext, string controllerName)
        {
            Type controllerType = base.GetControllerType(requestContext, controllerName);

            if (controllerType != null)
            {
                return controllerType;
            }

            Type entityType = _assemblyService.Entities.FirstOrDefault(x => x.Name == controllerName);

            if (entityType == null)
            {
                throw new Exception("Não existe entidade para a controller: " + controllerName);
            }

            Type modelType = _assemblyService.Models.FirstOrDefault(x => x.Name == controllerName + "Model");

            if (modelType == null)
            {
                throw new Exception("Não existe model para a controller: " + controllerName);
            }

            Type[] typeArgs = { entityType, modelType };

            Type d1 = typeof(GenericController<,>);
            Type constructed = d1.MakeGenericType(typeArgs);

            return constructed;

            //return typeof(GerenicController<,>);            
        }
        protected override IController GetControllerInstance(RequestContext requestContext, Type controllerType)
        {
            return base.GetControllerInstance(requestContext, controllerType);
        }
        protected override SessionStateBehavior GetControllerSessionBehavior(RequestContext requestContext, Type controllerType)
        {
            return base.GetControllerSessionBehavior(requestContext, controllerType);
        }
        public override void ReleaseController(IController controller)
        {
            base.ReleaseController(controller);
        }
    }
}
