using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace shop_app.Models
{
    public class Product
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Required(ErrorMessage = "Id обязательно")]
        public int Id { get; set; }

        [Required(ErrorMessage = "Название обязательно")]
        [StringLength(100, ErrorMessage = "Название должно быть не длиннее 100 символов")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Цена обязательна")]
        [Range(0.01, 10000.00, ErrorMessage = "Цена должна быть в диапазоне от 0.01 до 10000.00")]
        public decimal Price { get; set; }

        [Required(ErrorMessage = "Укажите количество")]
        [Range(0, int.MaxValue, ErrorMessage = "Количество должно быть неотрицательным")]
        public int Quantity { get; set; }

        [StringLength(500, ErrorMessage = "Описание должно быть не длиннее 500 символов")]
        public string Description { get; set; }

        public string ImageUrl { get; set; } = string.Empty;
    }
}
