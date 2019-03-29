using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;

namespace DemoApiRest.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BlobController : ControllerBase
    {
        private readonly IConfiguration configuration;
        public BlobController(IConfiguration config) => (configuration) = (config);

        // GET blob/values
        [HttpGet]
        public ActionResult<string> Get()
        {
            var response = DeleteFileAsync().GetAwaiter().GetResult();
            return response;
        }

        private async Task<string> CreateContainerAsync()
        {
            CloudStorageAccount storgaAccount = CloudStorageAccount.Parse(configuration.GetConnectionString("conexionAzure"));
            CloudBlobClient clienteblob = storgaAccount.CreateCloudBlobClient();
            CloudBlobContainer blobContainer = clienteblob.GetContainerReference("alumnos");
            await blobContainer.CreateIfNotExistsAsync();
            await blobContainer.SetPermissionsAsync(new BlobContainerPermissions { PublicAccess = BlobContainerPublicAccessType.Blob });

            return "Creado Correctamente";
        }

        private async Task<string> UploadFilesAsync()
        {
            string filePath = @"C:\users.csv";
            CloudStorageAccount storgeAccount = CloudStorageAccount.Parse(configuration.GetConnectionString("conexionAzure"));
            CloudBlobClient blobClient = storgeAccount.CreateCloudBlobClient();
            CloudBlobContainer blobContainer = blobClient.GetContainerReference("alumnos");
            CloudBlockBlob cloudBlockBlob = blobContainer.GetBlockBlobReference(Path.GetFileName(filePath));
            await cloudBlockBlob.UploadFromFileAsync(filePath);

            return "Archivo cargado Correctamente";
        }

        private string GetNameFile()
        {
            CloudStorageAccount storgeAccount = CloudStorageAccount.Parse(configuration.GetConnectionString("conexionAzure"));
            CloudBlobClient blobClient = storgeAccount.CreateCloudBlobClient();
            CloudBlobContainer blobContainer = blobClient.GetContainerReference("alumnos");
            CloudBlockBlob blockBlob = blobContainer.GetBlockBlobReference("users.cs");   

            return blockBlob.Name.ToString();
        }    

        private async Task<string> DeleteFileAsync()
        {
            CloudStorageAccount storgeAccount = CloudStorageAccount.Parse(configuration.GetConnectionString("conexionAzure"));
            CloudBlobClient blobClient = storgeAccount.CreateCloudBlobClient();
            CloudBlobContainer blobContainer = blobClient.GetContainerReference("alumnos");
            CloudBlockBlob blockBlob = blobContainer.GetBlockBlobReference("users.csv");   
            await blockBlob.DeleteAsync();

            return "Archivo Eliminado Correctamente";

        }              

    }
}