using System;
using System.Collections.Generic;
using System.Text;

namespace Attandence.Services
{
    public interface IImageCompressionService
    {
        byte[] CmpressImage(byte[] imageData, int compressionPercentage, int ImageWidth, int ImaageHeight);
        byte[] ResizeAndRotate(string filePath, int maxWidth, int maxHeight, int jpegQuality,int RotateAngle);
    }
}
