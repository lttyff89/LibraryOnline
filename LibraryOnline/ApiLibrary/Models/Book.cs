using System;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;


namespace LibraryApi.Models
{
    public class Book
    {
      public int Id { get; set; }

        [Required, MaxLength(255)]
        public string Title { get; set; }

        [Required, MaxLength(100)]
        public string Author { get; set; }

        [Required,MaxLength(255)]
        public string ISBN { get; set; }

        [Required]
        // Esta es la propiedad real que vas a usar
        public string PunlishedDate { get; set; }

        [Required]
        [MaxLength(500)]
        public string Genre { get; set; }

    }
}
