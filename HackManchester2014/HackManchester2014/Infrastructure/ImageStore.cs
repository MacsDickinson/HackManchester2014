using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace HackManchester2014.Infrastructure
{
    public class ImageStore : IImageStore
    {
        public string DataDir { get; private set; }

        public ImageStore(string dataDir)
        {
            DataDir = dataDir;
        }
        public Guid SaveImage(Stream stream)
        {
            var Id = Guid.NewGuid();
            var uploadDirectory = Path.Combine(DataDir, "ImageUploads");

            if (!Directory.Exists(uploadDirectory))
            {
                Directory.CreateDirectory(uploadDirectory);
            }

            using (var fileStream = File.Create(string.Format("{0}/{1}",uploadDirectory, Id)))
            {
                stream.CopyTo(fileStream);
            }
            return Id;
        }

        public Stream GetImage(Guid Id)
        {
            var uploadDirectory = Path.Combine(DataDir, "ImageUploads");
            return File.OpenRead(string.Format("{0}/{1}", uploadDirectory, Id));
        }
    }
}