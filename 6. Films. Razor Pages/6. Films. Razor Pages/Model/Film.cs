using _6._Films._Razor_Pages.Validation;
using System.ComponentModel.DataAnnotations;

namespace _6._Films._Razor_Pages.Model
{
    public class Film
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Имя обязательно")]
        [Display(Name = "Название")]
        public string? Name { get; set; }

        [Display(Name = "Постер")]
        public string? Poster { get; set; }

        [Required(ErrorMessage = "Жанр обязателен")]
        [Display(Name = "Жанр")]
        public string Genres { get; set; }

        [Required(ErrorMessage = "Режиссер обязателен")]
        [DirectorValidation("Жюлия Дюкорно", "Селин Сьямма", "Дэмиен Шазелл", "Ари Астер", "Роберт Эггерс", ErrorMessage = "Этот режиссер запрещен!")]
        [Display(Name = "Режиссер")]
        public string? Director { get; set; }

        [Range(1895, 2024, ErrorMessage = "Год фильма должен быть в диапозоне 1895-2024")]
        [Required(ErrorMessage = "Год обязателен")]
        [Display(Name = "Год")]
        public int Year { get; set; }

        [Required(ErrorMessage = "Описание обязательно")]
        [Display(Name = "Описание")]
        public string? Description { get; set; }
    }
}
