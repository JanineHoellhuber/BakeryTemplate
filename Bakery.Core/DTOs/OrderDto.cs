using Bakery.Core.Entities;
using System;
using System.ComponentModel.DataAnnotations;

namespace Bakery.Core.DTOs
{
  public class OrderDto
  {
    public int OrderId { get; set; }
     [Required(ErrorMessage = "Bestellnummer darf nicht leer sein!")]
     public string OrderNr { get; set; }
    [Display(Name = "Datum")]
    [DisplayFormat(DataFormatString = "{0:dd.MM.yyyy}",
        ApplyFormatInEditMode = true)]
    public DateTime Date { get; set; }

    public string CustomerName { get; set; }


    public OrderDto(Order o)
    {
      if (o != null)
      {
        OrderId = o.Id;
        OrderNr = o.OrderNr;
        Date = o.Date;
      //  CustomerName = o.Customer.FullName;
      }
    }
  }
}
