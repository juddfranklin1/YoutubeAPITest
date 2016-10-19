using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

using System.IO;
using System.Reflection;
using System.Threading;
using Microsoft.AspNetCore.Hosting;
using System.Net;
using Microsoft.AspNetCore.Http;
using Microsoft.Net.Http.Headers;

namespace TestBootstrapWebApp.Controllers
{
    class NameBehavior
    {
        internal string DoSomeNameStuff(string[] args)
        {
            int count = 0;
            string Output = "";
            foreach (string Name in args)
            {
                count += 1;
                if(count == args.Length){
                    Output += Name + "!";
                } else {
                    Output += Name + ", ";
                }
            }
            return Output;
        }
    }
    public class HomeController : Controller
    {
        NameBehavior NameStuff = new NameBehavior();
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult About()
        {

            string[] Names = {"Judd","Melanie","Emmanuel","Andrew"};
            string NameString = NameStuff.DoSomeNameStuff(Names);

            ViewData["StupidInteger"] = @NameString;
            ViewData["Message"] = "Your application description page.";
            ViewData["SecondaryMessage"] = "Some more information";
            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Error()
        {
            return View();
        }

        public IActionResult UploadFiles()
        {
            return View();
        }
 
        private IHostingEnvironment hostingEnv;

        public HomeController(IHostingEnvironment env)
        {
            this.hostingEnv = env;
        }

        [HttpPost]
        public IActionResult UploadFiles(IList<IFormFile> files)
        {
            long size = 0;
            foreach(var file in files)
            {
                var filename = ContentDispositionHeaderValue
                                .Parse(file.ContentDisposition)
                                .FileName
                                .Trim('"');
                filename = hostingEnv.WebRootPath + $@"\{filename}";
                size += file.Length;
                using (FileStream fs = System.IO.File.Create(filename))
                {
                file.CopyTo(fs);
                fs.Flush();
                }
            }
            ViewBag.Message = $"{files.Count} file(s) / {size} bytes uploaded successfully!";
            return View();
        }

        [HttpPost]
        public IActionResult UploadFilesAjax()
        {
            long size = 0;
            var files = Request.Form.Files;
            foreach (var file in files)
            {
                var filename = ContentDispositionHeaderValue
                                .Parse(file.ContentDisposition)
                                .FileName
                                .Trim('"');
                filename = hostingEnv.WebRootPath + $@"/uploads/{filename}";
                size += file.Length;
                using (FileStream fs = System.IO.File.Create(filename))
                {
                file.CopyTo(fs);
                fs.Flush();
                }
            }
            string message = $"{files.Count} file(s) / {size} bytes uploaded successfully!";
            return Json(message);
        }
    }
}
