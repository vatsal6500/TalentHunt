using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace TalentHunt.ModelView
{
    public partial class planv
    {
        public int planid { get; set; }

        [Required(ErrorMessage = "*")]
        [DisplayName("Plan Type")]
        [DataType(DataType.Text)]
        [StringLength(20, MinimumLength = 2, ErrorMessage = "Invalid Talent")]
        [RegularExpression(@"^[a-zA-z ]*$", ErrorMessage = "Invalid Talent")]
        public string plantype { get; set; }

        [Required(ErrorMessage = "*")]
        [DisplayName("Duration")]
        [DataType(DataType.Text)]
        [RegularExpression(@"^[0-9:-]*$", ErrorMessage = "Invalid Duration")]
        public string duration { get; set; }

        [Required(ErrorMessage = "*")]
        [DisplayName("Price")]
        [DataType(DataType.Text)]
        [RegularExpression(@"^[0-9]*$", ErrorMessage = "Invalid Duration")]
        public int price { get; set; }

        [Required(ErrorMessage = "*")]
        [DisplayName("Max Events")]
        [DataType(DataType.Text)]
        [RegularExpression(@"^[0-9]*$", ErrorMessage = "Invalid Duration")]
        public int maxevents { get; set; }

        [Required(ErrorMessage = "*")]
        [DisplayName("Max Bids")]
        [DataType(DataType.Text)]
        [RegularExpression(@"^[0-9]*$", ErrorMessage = "Invalid Duration")]
        public int maxbids { get; set; }

        [Required(ErrorMessage = "*")]
        [DisplayName("Description")]
        [DataType(DataType.MultilineText)]
        public string description { get; set; }

        [Required(ErrorMessage = "*")]
        [DisplayName("Benifits")]
        public string benefits { get; set; }


    }
}