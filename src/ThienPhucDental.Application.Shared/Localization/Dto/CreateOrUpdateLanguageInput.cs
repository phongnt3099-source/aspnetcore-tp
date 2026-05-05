using System.ComponentModel.DataAnnotations;

namespace ThienPhucDental.Localization.Dto
{
    public class CreateOrUpdateLanguageInput
    {
        [Required]
        public ApplicationLanguageEditDto Language { get; set; }
    }
}