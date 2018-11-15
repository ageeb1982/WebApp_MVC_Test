using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebApp_Test.Models
{
    /// <summary>
    /// كلاس المواضيع
    /// </summary>
    public class article
    {
 /// <summary>
 /// الرقم التسلسلي للموضوع
 /// </summary>
         [HiddenInput(DisplayValue = false)]
         [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Int64 Id { get; set; }

        /// <summary>
        /// اسم الموضوع
        /// </summary>

        [Display(Name = "Article Name")]
        [RegularExpression(@"^[ (\p{L})?(\p{M})?-]*$", ErrorMessage = "Please Use letters only")]
        [Required(ErrorMessage = "Please Input Article Name")]
        [MaxLength(125)]
        public string Name { set; get; }

        /// <summary>
        /// الموضوع كامل
        /// </summary>
        [Display(Name = "Article body")]
        [Required(ErrorMessage = "please Input Article body ")]
        [RegularExpression(@"^[ \n\r(\p{L})?(\p{M})?-]*$", ErrorMessage = "Please Use letters only")]
        [MaxLength(1000)]
        [DataType(DataType.MultilineText)]
        public string Body { set; get; }

        /// <summary>
        /// وقت إنشاء الموضوع
        /// </summary>
        [Display(Name = "Article Time")]

        public DateTime AddTime { set; get; }


    }
}