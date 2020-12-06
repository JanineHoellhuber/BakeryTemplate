using Bakery.Core.Contracts;
using Bakery.Core.DTOs;
using Bakery.Core.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace Bakery.Web.Pages
{
  public class EditModel : PageModel
  {
    private readonly IUnitOfWork _uow;

    public EditModel(IUnitOfWork uow)
    {
      _uow = uow;
    }
        public OrderWithItemsDto OrderWithItems { get; set; }
        public List<SelectListItem> Products { get; set; }
        [BindProperty]
        public int SelectedProductId { get; set; }
        [BindProperty]
        public int Amount { get; set; }

        public async Task<IActionResult> OnGet(int id)
    {
      return Page();
    }
  }
}
