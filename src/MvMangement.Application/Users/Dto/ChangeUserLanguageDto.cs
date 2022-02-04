using System.ComponentModel.DataAnnotations;

namespace MvMangement.Users.Dto
{
    public class ChangeUserLanguageDto
    {
        [Required]
        public string LanguageName { get; set; }
    }
}