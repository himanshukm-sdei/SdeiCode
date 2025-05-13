using AutoMapper;
using NaCore.Domain.Clipping;
using NaServices.Clipping;
using NaWebsite.Common;
using NaWebsite.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace NaWebsite.Controllers
{
    public class ClippingsController : Controller
    {
        private readonly IClipping _clippingService;

        public ClippingsController(IClipping clippingService)
        {
            _clippingService = clippingService;
        }
        // GET: Clippings
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public JsonResult GetClipDetailsByName(string clipName)
        {
            var clipDetails = _clippingService.GetClipDetailsFromClipName(clipName);
            //8527323998
            return Json(clipDetails, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetAllCategory()
        {
            var tags = _clippingService.GetAllCategory();
            return Json(tags, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public string SaveClipsDetails(SaveClipModel clippedDetail)
        {
            string returnVal = string.Empty;
            var mapper = GenericAutoMapperConfigure.InitializeAutoMapper<SaveClipModel, CabinetSaveClip>();
            var clipInfo = mapper.Map<CabinetSaveClip>(clippedDetail);
            int resultVal = _clippingService.SaveClipDetails(clipInfo);

            if (resultVal > 0)
                returnVal = "Saved";
            else
                returnVal = "Failed";

            return returnVal;
        }
    }
}