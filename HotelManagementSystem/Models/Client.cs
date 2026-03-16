using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace HotelManagementSystem.Models;

public partial class Client
{
    public int ClId { get; set; }

    [Required(ErrorMessage = "Ім'я клієнта обов'язкове")]
    [StringLength(100, MinimumLength = 2, ErrorMessage = "Ім'я має бути від 2 до 100 символів")]
    [Display(Name = "ПІБ Клієнта")]
    public string ClName { get; set; } = null!;

    [Display(Name = "Номер телефону")]
    [Required(ErrorMessage = "Телефон обов'язковий")]
    [Phone(ErrorMessage = "Введіть коректний номер телефону")]
    public string? ClPhone { get; set; }

    public virtual ICollection<Booking> Bookings { get; set; } = new List<Booking>();
}