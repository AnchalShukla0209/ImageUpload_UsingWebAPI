using ImageUploads.Models;
using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;

namespace ImageUploads.Controllers
{
    public class ImageFileController : ApiController
    {
        DB_imgEntities db = new DB_imgEntities();
        [HttpGet]
        [Route("api/ImageFile/GetAllImage")]

        public HttpResponseMessage GetAllImage()
        {
            DB_imgEntities db = new DB_imgEntities();
            var files = db.tblProducts;
            return Request.CreateResponse(HttpStatusCode.OK, files);
        }
        [HttpPost]
        [Route("api/ImageFile/InsertImage")]
        public HttpResponseMessage InsertImage()
        {
            //Create HTTP Response.
            HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK);

            //Check if Request contains File.
            if (HttpContext.Current.Request.Files.Count == 0)
            {
                throw new HttpResponseException(HttpStatusCode.UnsupportedMediaType);
            }

            //Read the File data from Request.Form collection.
            HttpPostedFile postedFile = HttpContext.Current.Request.Files[0];

            //Fetch the File Name.
            string fileName = Path.GetFileName(postedFile.FileName);

            //Set the File Path.
            string filePath = @"F:\Image\" + fileName;

            //Save the File in Folder.
            postedFile.SaveAs(filePath);

            //Insert the File to Database Table.
            DB_imgEntities db = new DB_imgEntities();
            tblProduct file = new tblProduct
            {
                ImageName = fileName,
                ImagePath = filePath

            };
            db.tblProducts.Add(file);
            db.SaveChanges();

            return Request.CreateResponse(HttpStatusCode.OK, "Data insert successfully");

        }

        //update image------------

        //        [HttpPut]
        //        public HttpResponseMessage updateImage(int Id)
        //        {
        //            try
        //            {
        //                HttpPostedFile postedFile = HttpContext.Current.Request.Files[0];

        //                tblProduct putid = db.tblProducts.Where(x => x.Id == Id).FirstOrDefault();

        //                if (putid != null)
        //                {
        //                    tblProduct tblupl = db.tblProducts.Find(Id)
        //;
        //                    tblupl.Id = Id;
        //                    tblupl.ImageName = Path.GetFileName(postedFile.FileName);
        //                    tblupl.ImagePath = postedFile.ContentType;
        //                    string filePath = @"F:\Image\" + tblupl.ImageName;
        //                    postedFile.SaveAs(filePath);
        //                    db.SaveChanges();
        //                    //return Request.CreateResponse(HttpStatusCode.OK, "updated sucessfully");
        //                }
        //                return Request.CreateResponse(HttpStatusCode.OK, "updated sucessfully");
        //            }

        //            catch (Exception)
        //            {
        //                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Data not updated");
        //            }

        //        }

        [HttpPut]
        public HttpResponseMessage UpdateImage(int Id)
        {
            try
            {
                HttpPostedFile postedFile = HttpContext.Current.Request.Files[0];
                tblProduct putid = db.tblProducts.Where(x => x.Id == Id).FirstOrDefault();
                string file = putid.ImageName;
                string path = @"F:\Image\" + file;
                FileInfo AA = new FileInfo(path);
                if (AA.Exists)
                {
                    AA.Delete();
                    tblProduct tblupl = db.tblProducts.Find(Id)
;
                    tblupl.Id = Id;
                    tblupl.ImageName = Path.GetFileName(postedFile.FileName);
                    tblupl.ImagePath = postedFile.ContentType;
                    string filePath = @"F:\Image\" + tblupl.ImageName;
                    postedFile.SaveAs(filePath);
                    db.SaveChanges();
                }
                return Request.CreateResponse(HttpStatusCode.OK, "updated sucessfully");
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Data not updated");
            }
        }

        [HttpDelete]
        //[Route("api/ImageFile")]
        public HttpResponseMessage Delete(int id)
        {
            try
            {
                
                DB_imgEntities entities = new DB_imgEntities();
                tblProduct file = entities.tblProducts.ToList().Find(p => p.Id == id);
                string files = file.ImageName;
                string path = @"F:\Image\" + files;
                FileInfo AA = new FileInfo(path);
                if (file != null)
                {
                    AA.Delete();
                    string filePath = @"F:\Image\" + files;
                    File.Delete(filePath);
                    //db.tblProducts.Remove(file);
                    db.SaveChanges();
                    entities.tblProducts.Remove(file);
                    entities.SaveChanges();

                    db.SaveChanges();
                    //return Request.CreateResponse(HttpStatusCode.OK, "updated sucessfully");
                }
                return Request.CreateResponse(HttpStatusCode.OK, "data delete sucessfully");
            }

            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "ID not Found");
            }
        } 
    }
}


   