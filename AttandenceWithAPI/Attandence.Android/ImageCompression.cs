using Android.App;
using Android.Content;
using Android.Graphics;
using Android.Media;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Attandence.Droid;
using Attandence.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Xamarin.Forms;
using Orientation = Android.Media.Orientation;

[assembly: Dependency(typeof(ImageCompression))]
namespace Attandence.Droid
{
    public class ImageCompression : IImageCompressionService
    {
        public ImageCompression() { }

        public byte[] CmpressImage(byte[] imageData, int compressionPercentage, int ImageWidth, int ImaageHeight)
        {
            using (Bitmap image = BitmapFactory.DecodeByteArray(imageData, 0, imageData.Length))
            {
                using (Bitmap resizedImage = Bitmap.CreateScaledBitmap(image, ImageWidth, ImaageHeight, false))
                {
                    using (MemoryStream ms = new MemoryStream())
                    {
                        resizedImage.Compress(Bitmap.CompressFormat.Jpeg, compressionPercentage, ms);
                        resizedImage.Recycle();
                        resizedImage.Dispose();
                        return ms.ToArray();
                    }
                }
            }
        }

		public byte[] ResizeAndRotate(string filePath, int maxWidth, int maxHeight, int jpegQuality,int RotateAngle)
		{
			// loads just the dimensions of the file instead of the entire image
			var options = new BitmapFactory.Options { InJustDecodeBounds = true };
			BitmapFactory.DecodeFile(filePath, options);

			int outHeight = options.OutHeight;
			int outWidth = options.OutWidth;
			int inSampleSize = 1;

			if (outHeight > maxHeight || outWidth > maxWidth)
			{
				inSampleSize = (outWidth > outHeight) ? outWidth / maxWidth : outHeight / maxHeight;
			}

			options.InSampleSize = inSampleSize;
			options.InJustDecodeBounds = false;

			// decodes image file
			var newBitmap = BitmapFactory.DecodeFile(filePath, options);
			var matrix = new Matrix();

			// rotates the image if needed.
			// Known Issue: HTC phones will report all pictures as landscape orientation.
			ExifInterface exif = new ExifInterface(filePath);
			var orientation = (Orientation)exif.GetAttributeInt(ExifInterface.TagOrientation, (int)Orientation.Undefined);
			matrix.PreRotate(RotateAngle);
			//switch (orientation)
			//{
			//	case Orientation.Normal:
			//		// landscape. Don't do anything
			//		break;
			//	case Orientation.Rotate180:
			//		matrix.PreRotate(180);
			//		break;
			//	case Orientation.Rotate270:
			//		matrix.PreRotate(270);
			//		break;
			//	default:
			//		// handles portrait and other orientations that we may not need to handle.
			//		matrix.PreRotate(270);
			//		break;
			//}

			newBitmap = Bitmap.CreateBitmap(newBitmap, 0, 0, newBitmap.Width, newBitmap.Height, matrix, false);
			matrix.Dispose();
			matrix = null;

			using (var stream = new System.IO.MemoryStream())
			{
				newBitmap.Compress(Bitmap.CompressFormat.Jpeg, (100-jpegQuality), stream);
				newBitmap.Recycle();
				newBitmap.Dispose();
				newBitmap = null;
				return stream.ToArray();
			}
		}
	}
}