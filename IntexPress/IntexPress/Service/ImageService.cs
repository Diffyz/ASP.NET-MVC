using DBUserCodeFirst;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Drive.v3;
using Google.Apis.Services;
using Google.Apis.Util.Store;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Web;

namespace IntexPress.Service
{
    internal class ImageService 
    {
        private static string[] Scopes = { DriveService.Scope.Drive };
        internal static string image;

        //https://www.youtube.com/watch?v=xtqpWG5KDXY google drive lesson

        internal static void UploadFile( HttpPostedFileBase file)
        {
            DriveService service = GetService();
            string path = Path.Combine(System.Web.HttpContext.Current.Server.MapPath("~/GoogleDriveFiles"),
                 Path.GetFileName(file.FileName));
            file.SaveAs(path);
            var FileMetaData = new Google.Apis.Drive.v3.Data.File();
            FileMetaData.Name = Path.GetFileName(file.FileName);

            FileMetaData.MimeType = MimeMapping.GetMimeMapping(path);
            FilesResource.CreateMediaUpload request;
            using (var stream = new System.IO.FileStream(path, System.IO.FileMode.Open))
            {
                request = service.Files.Create(FileMetaData, stream, FileMetaData.MimeType);
                request.Fields = "id";
                request.Upload();
            }
            InsertNew(service, file);
        }

        private static DriveService GetService()
        {
            UserCredential credential;
            using (var stream = new FileStream(@"D:\client_secret.json", FileMode.Open, FileAccess.Read))
            {
                String FolderPath = @"D:\";
                String FilePath = Path.Combine(FolderPath, "DriveServiceCredentials.json");

                credential = GoogleWebAuthorizationBroker.AuthorizeAsync(
                GoogleClientSecrets.Load(stream).Secrets,
                Scopes,
                "user",
                CancellationToken.None,
                new FileDataStore(FilePath, true)).Result;
            }
            DriveService service = new DriveService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = credential,
                ApplicationName = "GoogleDriveRestAPI_v3",
            });
            return service;
        }

        internal static void InsertNew(DriveService service, HttpPostedFileBase file)
        {
            FilesResource.ListRequest FileListRequest = service.Files.List();
            FileListRequest.Fields = "nextPageToken, files(id, name)";
            IList<Google.Apis.Drive.v3.Data.File> files = FileListRequest.Execute().Files; // get all file at google disk

            foreach (var file_ in files)
            {
                if (file_.Name == file.FileName) // find image
                {
                    image = "https://lh3.google.com/u/0/d/" + file_.Id;
                    break;
                }
            }
        }

        internal void InsertNew()
        {
            throw new NotImplementedException();
        }

        internal static string GetPicture(int changeNewId)
        {
            using(SampleContext context = new SampleContext())
            {
                string image = context.news.SingleOrDefault(x => x.newsId == changeNewId).image;
                if (image != null)
                    return image;
            }
            return "";
        }
    }

}
