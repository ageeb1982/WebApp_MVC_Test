using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebApp_Test.Models
{
    public class article
    {
 
         [HiddenInput(DisplayValue = false)]
         [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Int64 Id { get; set; }



        [Display(Name = "Article Name")]
        [RegularExpression(@"^[ (\p{L})?(\p{M})?-]*$", ErrorMessage = "Please Use letters only")]
        [Required(ErrorMessage = "Please Input Article Name")]
        [MaxLength(125)]
        public string Name { set; get; }

        [Display(Name = "Article body")]
        [Required(ErrorMessage = "please Input Article body ")]
        [RegularExpression(@"^[ \n\r(\p{L})?(\p{M})?-]*$", ErrorMessage = "Please Use letters only")]
        [MaxLength(1000)]
        [DataType(DataType.MultilineText)]
        public string Body { set; get; }


        [Display(Name = "Article Time")]

        public DateTime AddTime { set; get; }


    }
}