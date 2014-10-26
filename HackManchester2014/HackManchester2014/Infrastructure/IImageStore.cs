using System;
using System.IO;

namespace HackManchester2014.Infrastructure
{
    public interface IImageStore
    {
        Guid SaveImage(Stream stream);
        Stream GetImage(Guid Id);
    }
}