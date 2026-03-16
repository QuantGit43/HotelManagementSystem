using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace HotelManagementSystem.Models;

public partial class Service
{
    public int SrvId { get; set; }

    [Required(ErrorMessage = "Назва послуги обов'язкова")]
    [Display(Name = "Назва послуги")]
    public string SrvName { get; set; } = null!;

    [Required(ErrorMessage = "Вкажіть вартість послуги")]
    [Display(Name = "Вартість послуги (грн)")]
    [Range(0, 100000, ErrorMessage = "Вартість має бути додатною")]
    public decimal? SrvPrice { get; set; }

    public virtual ICollection<BookingDetail> BookingDetails { get; set; } = new List<BookingDetail>();
}