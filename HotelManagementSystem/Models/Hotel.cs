using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace HotelManagementSystem.Models;

public partial class Hotel
{
    public int HtId { get; set; }

    [Required(ErrorMessage = "Поле 'Назва готелю' не повинно бути порожнім")]
    [Display(Name = "Назва готелю")]
    public string HtName { get; set; } = null!;

    [Required(ErrorMessage = "Вкажіть адресу готелю")]
    [Display(Name = "Адреса")]
    public string HtAddress { get; set; } = null!;

    [Display(Name = "Кількість зірок")]
    [Range(1, 5, ErrorMessage = "Кількість зірок має бути від 1 до 5")]
    public int? HtStars { get; set; }
    
    public virtual ICollection<Room> Rooms { get; set; } = new List<Room>();
}
