using Common;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;

namespace WebApi.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class TestUploadExcelController : ControllerBase
    {
        readonly IConfiguration _config;
        readonly IWebHostEnvironment _env;
        public TestUploadExcelController(IConfiguration config, IWebHostEnvironment env)
        {
            _config = config;
            _env = env;
        }

        ExcelHelper ex = new ExcelHelper();

        [HttpPost]
        public async Task<IActionResult> ReadFile(string path = "default")
        {
            var files = Request.Form.Files;

            if (files.Count == 0) 
                return Ok("");

            //var domain = _config["FilePath"];
            var dircstr = $"/Files/{path}/{DateTime.Now:yyyyMMdd}/";  //Files/default/20220409/
            var result = new List<string>();
            foreach (var file in files)
            {
                var filename = Path.GetFileName(file.FileName);  //123.xlsx
                if (string.IsNullOrEmpty(filename)) 
                    continue;

                var fileext = Path.GetExtension(filename).ToLower();  //.xlsx
                var folderpath = _env.ContentRootPath;  //D:\git\Report\WebApi\
                folderpath = folderpath.Replace("\\", "/").Remove(folderpath.Length - 1, 1);
                FileHelper.CreateDir(folderpath + dircstr);
                var pre = DateTime.Now.ToString("yyyyMMddHHmmssffff");  //重新命名文件
                var after = FileHelper.GetRandom(10000000, 99999999).ToString();
                var filefullname = folderpath + dircstr + pre + "_" + after + FileHelper.ProExt(fileext);  //Files/default/20220409/202204092241148749_6625.xlsx
                using (var stream = new FileStream(filefullname, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }
                DataTable dt = ex.ExcelToDatatable(filefullname);
                result.Add(filefullname);
            }
            return Ok(result);
}
    }
}
