using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace HotelManagementSystem.Models;

public partial class BookingDetail
{
    public int BdId { get; set; }

    [Required(ErrorMessage = "Оберіть бронювання")]
    [Display(Name = "Бронювання")]
    public int? BdBkId { get; set; }

    [Required(ErrorMessage = "Оберіть послугу")]
    [Display(Name = "Послуга")]
    public int? BdSrvId { get; set; }

    [Required(ErrorMessage = "Вкажіть кількість")]
    [Display(Name = "Кількість")]
    [Range(1, 100, ErrorMessage = "Кількість повинна бути від 1 до 100")] 
    public int? BdQuantity { get; set; }

    [Display(Name = "Бронювання")]
    public virtual Booking? BdBk { get; set; }

    [Display(Name = "Послуга")]
    public virtual Service? BdSrv { get; set; }
}