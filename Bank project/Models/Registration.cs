using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Bank_project.Models
{
    public class Registration
    {
        [Key]
        public int userid { get; set; }
        [Required(ErrorMessage = "Name is required")]
        [StringLength(10, MinimumLength = 4, ErrorMessage = " name should be between 3 and 30 characters")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Email is required")]

        [EmailAddress(ErrorMessage = "Enter valid Email address")]
        public string Email { get; set; }

        [Display(Name = "Date of Birth")]

        [DataType(DataType.Date)]
        [Required(ErrorMessage = "Date of birth Required")]
        public DateTime? DOB { get; set; }
        [Required(ErrorMessage = "Gender is required")]
        public string Gender { get; set; }
        [Required]
        [MaxLength(12)]
        [MinLength(1)]
        [RegularExpression("^[0-9]*$", ErrorMessage = "Account number must be numeric")]
        [Display(Name = "Account Number")]
        //public string accountnumber { get; set; }
        public string accountnumber { get; set; }
        //public IPagedList accountnumber { get; set; }
        //[Required(ErrorMessage ="Enter Account ID")]

        // public int? accountid { get; set; }

        [Required(ErrorMessage = "Account Type is Required")]
        [Display(Name = "Account Type ID")]
        public string acctype { get; set; }

        [Display(Name = "Account Type")]
        public string acctypename { get; set; }
        [Required]
        [DataType(DataType.PhoneNumber)]
        [Display(Name = "Mobile Number")]
        [RegularExpression(@"^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$", ErrorMessage = "Not a valid phone number")]
        public string mobile { get; set; }

        [Required(ErrorMessage = "Enter Aadhar Number")]
        [RegularExpression("([1-9][0-9]*)", ErrorMessage = "Enter only numeric number")]
        [Display(Name = "Aadhar")]
        public string aadhar { get; set; }
        [Display(Name = "Bank Balance")]

        public float bank_balance { get; set; }


        public string role { get; set; }
        [Required(ErrorMessage = "Role is Required")]
        public string roleid { get; set; }


        public string password { get; set; }
        [Display(Name = "Account Creation Date")]
        public DateTime? AccountCreationDate { get; set; }

        public string accountcreatedby { get; set; }

        public bool EmailVerification { get; set; }
        //[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string ActivationCode { get; set; }
        public string otp { get; set; }
        public ICollection<bankbal> bankbal { get; set; }
        public ICollection<Transactions> Transaction { get; set; }
        public ICollection<logindetails> logindetails { get; set; }
        [NotMapped]
        public ICollection<roles> roles { get; set; }
        public bool isActive { get; set; }




    }
}