using Microsoft.AspNet.Identity;
using System.Web;
using System.Web.Mvc;

namespace SquareDanceASP.Controllers
{
    public class VideoController : Controller
    {
        // GET: Video
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Upload()
        {
            var userId = User.Identity.GetUserId();
            return View();
        }

        [HttpPost]
        public void VideoUpload(HttpPostedFileBase fileupload)
        {
            
        }
    }
}