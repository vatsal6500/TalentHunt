using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace TalentHunt.ModelView
{
    public partial class talentv
    {
        public int tid { get; set; }

        [Required(ErrorMessage = "*")]
        [DisplayName("Talents")]
        [StringLength(20, MinimumLength = 2, ErrorMessage = "Invalid Talent")]
        [RegularExpression(@"^[a-zA-z ]*$", ErrorMessage = "Invalid Talent")]
        public string ttype { get; set; }

    }
}