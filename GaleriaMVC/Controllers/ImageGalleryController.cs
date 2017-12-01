using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GaleriaMVC.Controllers
{
    public class ImageGalleryController : Controller
    {
        // GET: ImageGallery
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Gallery()
        {
            List<ImageGallery> Images = new List<ImageGallery>();
            using(GaleriaDBEntities db = new GaleriaDBEntities())
            {
                Images = db.ImageGallery.ToList();
            }
            return View(Images);

        }

        public ActionResult Upload()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Upload( ImageGallery IG)
        {
            // Tamaño de Archivo
            if (IG.File.ContentLength > (2*2024*2024))
            {
                ModelState.AddModelError("CustomError", "El tamaño del archivo debe ser menos de 2 MB.");
                return View();
            }

            if (!(IG.File.ContentType == "image/jpeg" || IG.File.ContentType == "image/gif" || IG.File.ContentType == "image/jpg"))
            {
                ModelState.AddModelError("CustomError", "El formato de imagen no es valido.");
                return View();
            }
            IG.FileName = IG.File.FileName;
            IG.ImageSize = IG.File.ContentLength;

            byte[] data = new byte[IG.File.ContentLength];
            IG.File.InputStream.Read(data, 0, IG.File.ContentLength);

            IG.ImageData = data;

            using (GaleriaDBEntities dc = new GaleriaDBEntities()) 
            {
                dc.ImageGallery.Add(IG);
                dc.SaveChanges();
            }
            return RedirectToAction("Gallery");

        }
    }
}