using System;
using System.ComponentModel.DataAnnotations;

namespace LibraryApp.Models
{
    public class Book
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "El Título es obligatorio.")]
        [StringLength(200, ErrorMessage = "El Título no puede superar los 200 caracteres.")]
        public string Title { get; set; }

        [Required(ErrorMessage = "El Autor es obligatorio.")]
        public string Author { get; set; }

        [Required(ErrorMessage = "El ISBN es obligatorio.")]
        [RegularExpression(@"^\d{13}$", ErrorMessage = "El ISBN debe tener 13 dígitos.")]
        public string ISBN { get; set; }

        [Required(ErrorMessage = "La fecha de publicación es obligatoria.")]
        public string PunlishedDate { get; set; }

        [Required(ErrorMessage = "El genero del libro es obligatorio.")]
        public string Genre { get; set; }
    }
}
