using System;
using System.ComponentModel.DataAnnotations;

namespace LibraryApp.Models
{
    public class Book_Send
    {
        public int Id { get; set; }
    
        public string Title { get; set; }
       
        public string Author { get; set; }
        public string ISBN { get; set; }
      
        public DateTime PunlishedDate { get; set; }
        public string Genre { get; set; }
    }
}
