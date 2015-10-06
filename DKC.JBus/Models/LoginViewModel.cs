using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace DKC.JBus.Models
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "กรุณาใส่รหัสผู้ใช้")]
        public string Username { get; set; }

        [Required(ErrorMessage = "กรุณาใส่รหัสผ่าน")]
        public string Password { get; set; }
    }
}