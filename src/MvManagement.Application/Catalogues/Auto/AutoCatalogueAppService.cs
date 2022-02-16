using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Application.Services.Dto;
using Abp.Domain.Repositories;
using Catalogue.Auto;
using Microsoft.EntityFrameworkCore;

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

        private static string RemoveDiacritics(string s)
        {
            var normalizedString = s.Normalize(NormalizationForm.FormD);
            var stringBuilder = new StringBuilder();

            foreach (var c in normalizedString.Where(c => CharUnicodeInfo.GetUnicodeCategory(c) != UnicodeCategory.NonSpacingMark))
            {
                stringBuilder.Append(c);
            }

            return stringBuilder.ToString();
        }

        public async Task<ListResultDto<MakeAuto>> GetMakeByCategoryAsync(AutoTypeCategoryMake categorie, string q = "")
        {
            var marciQuery = from m in _makeAutoRepository.GetAll()
                join cm in _makeCategoryAutoRepository.GetAll() on m.Id equals cm.IdMake
                where cm.Category == categorie
                select m;

            var marci = await marciQuery.ToListAsync();

            var listaMarci = marci.Where(m => RemoveDiacritics(m.Name.ToLower()).Contains(q.ToLower())).ToList();

            listaMarci.Insert(0, new MakeAuto { Id = -1, Name = L("AltaMarca") });

            return new ListResultDto<MakeAuto>(listaMarci.ToList());
        }

        public async Task<ListResultDto<ModelAuto>> GetModelsAsync(int idMarca, string q = "")
        {
            var modele = await _modelAutoRepository.GetAll().Where(m => m.IdMake == idMarca).ToListAsync();
            var listaModele = modele.Where(m => RemoveDiacritics(m.Name.ToLower()).Contains(q.ToLower())).ToList();

            listaModele.Insert(0, new ModelAuto { Id = -1, IdMake = idMarca, Name = L("AltModel") });

            return new ListResultDto<ModelAuto>(listaModele.ToList());
        }
    }
}