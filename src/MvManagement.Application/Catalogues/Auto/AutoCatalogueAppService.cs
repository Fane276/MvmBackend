using Abp.Domain.Repositories;
using Catalogue.Auto;

namespace MvManagement.Catalogues.Auto
{
    public class AutoCatalogueAppService : MvManagementAppServiceBase, IAutoCatalogueAppService
    {
        private readonly IRepository<MakeAuto> _makeAutoRepository;
        private readonly IRepository<ModelAuto> _modelAutoRepository;
        private readonly IRepository<MakeCategoryAuto> _makeCategoryAutoRepository;

        public AutoCatalogueAppService(
            IRepository<MakeAuto> makeAutoRepository, 
            IRepository<ModelAuto> modelAutoRepository,
            IRepository<MakeCategoryAuto> makeCategoryAutoRepository)
        {
            _makeAutoRepository = makeAutoRepository;
            _modelAutoRepository = modelAutoRepository;
            _makeCategoryAutoRepository = makeCategoryAutoRepository;
        }
    }
}