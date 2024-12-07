/*
 *      @Author: yaile
 */

using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;

namespace ITrack.Data.ViewModels
{
    public class EditUserViewModel
    {
        public string Id { get; set; }

        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required]
        [Display(Name = "Username")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Select the role")]
        [Display(Name = "Role")]
        public string SelectedRole { get; set; }

        [ValidateNever]
        public IEnumerable<IdentityRole> Roles { get; set; }
    }
}
