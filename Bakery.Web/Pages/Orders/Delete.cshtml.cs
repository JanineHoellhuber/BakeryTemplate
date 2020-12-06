using Bakery.Core.Contracts;
using Bakery.Core.DTOs;
using Bakery.Core.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace Bakery.Web.Pages
{
  public class DeleteModel : PageModel
  {
    private readonly IUnitOfWork _uow;

    public OrderDto Order { get; set; }

    public DeleteModel(IUnitOfWork uow)
    {
      _uow = uow;
    }

    public async Task<IActionResult> OnGet(int id)
    {
            Order = await _uow.Orders.GetByIdAsync(id);

            return Page();
     }

        public async Task<IActionResult> OnPostDelete(int id)
        {

            if (id == null)
            {
                return NotFound();
            }

            Order order = await _uow.Orders.GetAllByIdAsync(id);

            _uow.Orders.Remove(order);

            try
            {
                await _uow.SaveChangesAsync();
            }
            catch (ValidationException ex)
            {
                ModelState.AddModelError("", ex.Message);
            }

            return RedirectToPage("../Index", id);
        }
    }
}
