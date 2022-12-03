using Microsoft.AspNetCore.Mvc;

namespace Client.Areas.Admin.Components.Navbar;

public class Navbar : ViewComponent
{
    public IViewComponentResult Invoke(dynamic data)
    {
        return View();
    }
}