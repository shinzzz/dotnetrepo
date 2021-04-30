using System.ComponentModel.DataAnnotations;

namespace UsersAPI.Models {
    public class User {

        [Display (Name = "ID")]
        [Key]
        public int? id { get; set; }

        [Required]
        [Display (Name = "User Id")]
        public int? userId { get; set; }

        [Required]
        [Display (Name = "Title")]
        public string title { get; set; }

        [Required]
        [Display (Name = "Completed")]
        public bool completed { get; set; }
    }
}