using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace HotelManagementSystem.Models;

public partial class Category
{
    public int CatId { get; set; }

    [Required(ErrorMessage = "Назва категорії обов'язкова")]
    [Display(Name = "Категорія номеру")]
    public string CatName { get; set; } = null!;

    [Required(ErrorMessage = "Вкажіть ціну")]
    [Display(Name = "Ціна за добу (грн)")]
    public decimal CatPrice { get; set; }

    public virtual ICollection<Room> Rooms { get; set; } = new List<Room>();
}
