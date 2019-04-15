using System.ComponentModel.DataAnnotations;

namespace StudentExerciseMVC.Models
{
    public class Exercise
    {
        public int Id { get; set; }

        [Required]
        [Display(Name = "Exercise Name")]
        public string Name { get; set; }

        [Required]
        public string Language { get; set; }
    }
}
