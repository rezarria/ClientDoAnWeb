using Microsoft.AspNetCore.Mvc;

namespace Client.Areas.Admin.Components.Navbar;

public class ContentHeader : ViewComponent
{
    public IViewComponentResult Invoke(dynamic data)
    {
        return View();
    }
}