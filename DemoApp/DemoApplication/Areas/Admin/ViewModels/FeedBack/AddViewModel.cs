﻿using DemoApplication.Contracts.User;
using System.ComponentModel.DataAnnotations;

namespace DemoApplication.Areas.Admin.ViewModels.FeedBack
{
    public class AddViewModel
    {
        [Required]
        public IFormFile ProfilePhoto { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string LastName { get; set; }

        [Required]
        public string Content { get; set; }

        [Required]
        public MockRole Role { get; set; }
       
    }
}
