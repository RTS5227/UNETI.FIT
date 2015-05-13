using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Globalization;
using System.Web.Security;

namespace UNETI.FIT.Models.ViewModels
{
    public class LoginModel
    {
        [Required]
        [StringLength(100, ErrorMessage = "Nhập ít nhất {2} kí tự", MinimumLength = 5)]
        public string UserName { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "Nhập ít nhất {2} kí tự", MinimumLength = 5)]
        public string Password { get; set; }

        public bool RememberMe { get; set; }
    }
}
