using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace HotelManagementSystem.Models;

public partial class Room
{
    public int RmId { get; set; }

    [Required(ErrorMessage = "Поле не повинно бути порожнім")]
    [Range(1, 10000, ErrorMessage = "Номер кімнати має бути додатним числом (від 1 до 10000)")]
    [Display(Name = "Номер кімнати")]
    public int? RmNumber { get; set; }

    [Required(ErrorMessage = "Оберіть готель")]
    [Display(Name = "Готель")]
    public int? RmHtId { get; set; }

    [Required(ErrorMessage = "Оберіть категорію")]
    [Display(Name = "Категорія")]
    public int? RmCatId { get; set; }

    public virtual ICollection<Booking> Bookings { get; set; } = new List<Booking>();

    [Display(Name = "Категорія")]
    public virtual Category? RmCat { get; set; }

    [Display(Name = "Готель")]
    public virtual Hotel? RmHt { get; set; }
}