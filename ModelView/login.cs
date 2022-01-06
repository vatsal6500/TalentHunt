using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace TalentHunt.ModelView
{
    public partial class login
    {
        [Required(ErrorMessage = "*")]
        [DisplayName("User Name")]
        [DataType(DataType.Text)]
        public string username { get; set; }

        [Required(ErrorMessage = "*")]
        [DisplayName("Password")]
        [DataType(DataType.Password)]
        public string password { get; set; }

        public string expert { get; set; }

        public string production { get; set; }

        public bool rememberme { get; set; }
    }
}