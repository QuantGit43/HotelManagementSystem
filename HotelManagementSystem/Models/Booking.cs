using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace HotelManagementSystem.Models;

public partial class Booking
{
    public int BkId { get; set; }

    [Required(ErrorMessage = "Вкажіть дату заїзду")]
    [Display(Name = "Дата заїзду")]
    [DataType(DataType.Date)]
    public DateOnly? BkDateIn { get; set; }

    [Required(ErrorMessage = "Вкажіть дату виїзду")]
    [Display(Name = "Дата виїзду")]
    [DataType(DataType.Date)]
    public DateOnly? BkDateOut { get; set; }

    [Required(ErrorMessage = "Оберіть клієнта")]
    [Display(Name = "Клієнт")]
    public int? BkClId { get; set; }

    [Required(ErrorMessage = "Оберіть кімнату")]
    [Display(Name = "Кімната")]
    public int? BkRmId { get; set; }

    [Display(Name = "Клієнт")]
    public virtual Client? BkCl { get; set; }

    [Display(Name = "Кімната")]
    public virtual Room? BkRm { get; set; }

    public virtual ICollection<BookingDetail> BookingDetails { get; set; } = new List<BookingDetail>();
}