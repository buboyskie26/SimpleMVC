using System.ComponentModel.DataAnnotations;

namespace ThurstonMVC.Models.ViewModel
{
    public class EditEmployeeViewModel
    {
        public int Id { get; set; }
      /*  public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public DateTime DateOfBirth { get; set; }*/
        [StringLength(15, ErrorMessage = "First Name must not be exceed of 15 characters.")]
        [RegularExpression("^[a-zA-Z ]*$", ErrorMessage = "First Name must contain only alphabetical characters.")]
        [Required(ErrorMessage = "First Name is required.")]
        public string FirstName { get; set; }

        [StringLength(15, ErrorMessage = "Last Name must not be exceed of 15 characters.")]
        [RegularExpression("^[a-zA-Z ]*$", ErrorMessage = "Last Name must contain only alphabetical characters.")]
        [Required(ErrorMessage = "Last Name is required.")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Email address is required.")]
        [RegularExpression(@"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$", ErrorMessage = "Invalid Email address")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Date of Birth is required.")]
        [DataType(DataType.Date)]
        [Display(Name = "Date of Birth")]
        public DateTime DateOfBirth { get; set; }
    }
}
