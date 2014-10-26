using System;
using System.IO;

namespace HackManchester2014.Infrastructure
{
    public interface IImageStore
    {
        Guid SaveImage(Stream stream);
        void GetImage(Guid Id, Stream stream);
    }
}