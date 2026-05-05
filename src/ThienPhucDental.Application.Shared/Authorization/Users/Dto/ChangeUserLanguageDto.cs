using System.ComponentModel.DataAnnotations;

namespace ThienPhucDental.Authorization.Users.Dto
{
    public class ChangeUserLanguageDto
    {
        [Required]
        public string LanguageName { get; set; }
    }
}
