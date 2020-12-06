using Bakery.Core.Contracts;
using Bakery.Core.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Bakery.Web.Pages
{
  public class CreateModel : PageModel
  {
    private readonly IUnitOfWork _uow;
        [BindProperty]
        public Order Order { get; set; }
        [BindProperty]
        public int CustomerId { get; set; }


        public List<SelectListItem> Customers { get; set; }
      
        public CreateModel(IUnitOfWork uow)
    {
      _uow = uow;
    }

    public async Task<IActionResult> OnGet()
    {

            Customers = (await _uow.Customers.GetAllAsync()).Select(c => new SelectListItem(
                         c.FullName, c.Id.ToString()))
                .ToList();
            return Page();
    }

        public async Task<IActionResult> OnPost()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }
            Customer customer = await _uow.Customers.GetByIdAsync(CustomerId);

            var order = new Order();
            order.OrderNr = Order.OrderNr;
            order.Date = Order.Date;
            order.Customer = customer;


            _uow.Orders.Add(order);

            try
            {
                await _uow.SaveChangesAsync();
            }
            catch (ValidationException ve)
            {
                ModelState.AddModelError("", ve.Message);
                return Page();
            }
            return RedirectToPage("../Index");
        }
    }
}
