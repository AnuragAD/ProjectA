//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ProjectA_MVC.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public partial class Executive_Tbl
    {
        public int Id { get; set; }

        [Required(ErrorMessage ="This Field is required")]
        public string Name { get; set; }

        [Required(ErrorMessage = "This Field is required")]
        public Nullable<int> Age { get; set; }

        [Required(ErrorMessage = "This Field is required")]
        public string Phone { get; set; }

        [Required(ErrorMessage = "This Field is required")]
        public string Password { get; set; }

        [Required(ErrorMessage = "This Field is required")]
        [Compare("Password", ErrorMessage = "Password Do not Matched")]
        public string ConfirmPassword { get; set; }

        [Required(ErrorMessage = "This Field is required")]
     
        public string Email { get; set; }
    }
}
