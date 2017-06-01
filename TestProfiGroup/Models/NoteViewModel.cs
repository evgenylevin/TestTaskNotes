using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TestProfiGroup.Web.Models
{
    public class NoteViewModel
    {
        public Guid Id {get;set;}

        [Display(Name = "Title")]
        [Required(ErrorMessage = "Title shouldn't be empty")]
        [MaxLength(250, ErrorMessage = "Title maximum length is 250")]
        public string Title { get; set; }

        [Display(Name = "Text")]
        [MaxLength(1000, ErrorMessage = "Text maximum length is 1000")]
        public string Content { get; set; }
    }
}