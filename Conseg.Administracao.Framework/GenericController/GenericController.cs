using AutoMapper;
using Conseg.Administracao.Domain.Core;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Administracao.GenericController
{
    public class GenericController<TEntity, TModel> : Controller where TEntity : BaseEntity where TModel : BaseModel
    {
        private readonly IMapper _mapper;
        private readonly IRepository<TEntity> _repository;

        public GenericController(IRepository<TEntity> repository,
            IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        [HttpGet]
        public virtual ActionResult List()
        {
            Type type = typeof(TModel);
            return View(new BaseModel()
            {
                //Title = "Cidades",
                ModuleName = "",
                EntityName = type.Name.Replace("Model", ""),
                ModelName = type.Name
            });
        }

        [HttpGet]
        public virtual ActionResult Form(int? Id, string layout = "")
        {
            Type type = typeof(TModel);
            TModel model = (TModel)Activator.CreateInstance(typeof(TModel));

            //model.Title = "";
            model.ModuleName = "";
            model.EntityName = type.Name.Replace("Model", "");
            model.ModelName = type.Name;

            if (Id.HasValue)
            {
                var entity = _repository.GetById(Id);
                model = _mapper.Map<TEntity, TModel>(entity, model);
            }

            if (string.IsNullOrEmpty(layout))
            {
                return View(model);
            }
            else
            {
                return View("Form", layout, model);
            }
        }
    }
}