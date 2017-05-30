using Conseg.Administracao.Domain.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autofac;

namespace Conseg.Administracao.Framework
{
    public class LoadApplicationStartupTask : IStartupTask
    {
        public int Order { get { return -1; } }
                  

        public void Execute(IContainer container)
        {
            var assemblyService = container.Resolve<IAssemblyService>();

            var _appEntity = container.Resolve<IRepository<AppEntity>>();
            var _appModel = container.Resolve<IRepository<AppModel>>();

            foreach (var entityType in assemblyService.Entities)
            {

                if (_appEntity.Table.Count(x => x.Name == entityType.Name) > 0)
                {
                    continue;
                }

                AppEntity entity = new AppEntity();

                entity.Name = entityType.Name;
                entity.TypeFullname = entityType.AssemblyQualifiedName;
                entity.Namespace = entityType.Namespace;
                entity.Assembly = entityType.Assembly.FullName;

                _appEntity.Insert(entity);
            }

            foreach (var modelType in assemblyService.Models)
            {
                if (_appModel.Table.Count(x => x.Name == modelType.Name) > 0)
                {
                    continue;
                }

                AppModel model = new AppModel();

                model.Name = modelType.Name;
                model.TypeFullname = modelType.AssemblyQualifiedName;
                model.Namespace = modelType.Namespace;
                model.Assembly = modelType.Assembly.FullName;

                _appModel.Insert(model);

            }



        }

    }
}
