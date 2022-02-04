using System.ComponentModel.DataAnnotations;

namespace MvManagement.Users.Dto
{
    public class ChangeUserLanguageDto
    {
        [Required]
        public string LanguageName { get; set; }
    }
}