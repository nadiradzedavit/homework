using System;
using System.ComponentModel.DataAnnotations;

namespace MovieTracker.Models
{
    public class Movie
    {
        public int Id { get; set; }

        [Required]
        [StringLength(200)]
        [Display(Name = "სათაური")]
        public string Title { get; set; }

        [Required]
        [StringLength(100)]
        [Display(Name = "რეჟისორი")]
        public string Director { get; set; }

        [Required]
        [StringLength(50)]
        [Display(Name = "ჟანრი")]
        public string Genre { get; set; }

        [Required]
        [Range(1888, 2100)]
        [Display(Name = "გამოსვლის წელი")]
        public int ReleaseYear { get; set; }

        [Required]
        [Range(1, 10)]
        [Display(Name = "შეფასება")]
        public decimal Rating { get; set; }

        [Required]
        [StringLength(20)]
        [Display(Name = "სტატუსი")]
        public string WatchStatus { get; set; }

        [Required]
        [StringLength(50)]
        [Display(Name = "პლატფორმა")]
        public string Platform { get; set; }

        [Required]
        [Range(1, 1000)]
        [Display(Name = "ხანგრძლივობა (წუთი)")]
        public int Duration { get; set; }

        [Required]
        [StringLength(50)]
        [Display(Name = "ენა")]
        public string Language { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "ნანახის თარიღი")]
        public DateTime? WatchedDate { get; set; }

        [StringLength(1000)]
        [Display(Name = "შენიშვნები")]
        public string Notes { get; set; }
    }
}
