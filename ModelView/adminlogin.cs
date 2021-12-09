using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace TalentHunt.ModelView
{
    public partial class adminlogin
    {
        [Required(ErrorMessage = "*")]
        [DisplayName("User Name")]
        [RegularExpression(@"^[a-zA-z@_.0-9 ]*$", ErrorMessage = "Invalid Username")]
        public string username { get; set; }

        [Required(ErrorMessage = "*")]
        [DisplayName("Password")]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^\da-zA-Z]).{8,15}$", ErrorMessage = "Invalid password format")]
        [DataType(DataType.Password)]
        public string password { get; set; }
    }
}