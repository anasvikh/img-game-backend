using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace WebApplication1.Controllers
{
    public class ValuesController : ApiController
    {
        // GET api/values
        public IEnumerable<string> Get()
        {

            //RotateImages();

            //ParseImages4();

            RenameFiles();

            return new string[] {"value1", "value2"};
        }

        // GET api/values/5
        public string Get(int id)
        {
            return "value";
        }

        // POST api/values
        public void Post([FromBody] string value)
        {
        }

        // PUT api/values/5
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/values/5
        public void Delete(int id)
        {
        }

        public void RotateImages()
        {
            var images = Directory.GetFiles(@"D:\4\Перевертыши", "*.*", SearchOption.AllDirectories)
                .ToList();

            for (int i = 0; i < images.Count; i++)
            {
                Bitmap source = new Bitmap(images[i]);
                source.RotateFlip(RotateFlipType.Rotate180FlipNone);
                source.Save(images[i], System.Drawing.Imaging.ImageFormat.Jpeg);
            }

            images = Directory.GetFiles(@"D:\5\Перевертыши", "*.*", SearchOption.AllDirectories)
                .ToList();

            for (int i = 0; i < images.Count; i++)
            {
                Bitmap source = new Bitmap(images[i]);
                source.RotateFlip(RotateFlipType.Rotate180FlipNone);
                source.Save(images[i], System.Drawing.Imaging.ImageFormat.Jpeg);
            }

            images = Directory.GetFiles(@"D:\8\Перевертыши", "*.*", SearchOption.AllDirectories)
                .ToList();

            for (int i = 0; i < images.Count; i++)
            {
                Bitmap source = new Bitmap(images[i]);
                source.RotateFlip(RotateFlipType.Rotate180FlipNone);
                source.Save(images[i], System.Drawing.Imaging.ImageFormat.Jpeg);
            }
        }

        public void ParseImages()
        {
            var images = Directory.GetFiles(@"D:\classic", "*.*", SearchOption.AllDirectories)
                .ToList();

            var number = 1;
            for (int i = 0; i < images.Count; i++)
            {
                Bitmap source = new Bitmap(images[i]);

                int x = 1016;
                int y = 314;
                int width = 1415;
                int height = 945;
                Bitmap croppedImage3 =
                    source.Clone(new System.Drawing.Rectangle(x, y, width, height), source.PixelFormat);
                croppedImage3.RotateFlip(RotateFlipType.Rotate270FlipNone);
                croppedImage3.Save($@"D:\classic\classic\{number}.jpg", System.Drawing.Imaging.ImageFormat.Jpeg);

                number++;

                x = 1016;
                y = 1282;
                width = 1415;
                height = 945;
                Bitmap croppedImage4 =
                    source.Clone(new System.Drawing.Rectangle(x, y, width, height), source.PixelFormat);
                croppedImage4.RotateFlip(RotateFlipType.Rotate270FlipNone);
                croppedImage4.Save($@"D:\classic\classic\{number}.jpg", System.Drawing.Imaging.ImageFormat.Jpeg);

                number++;

                x = 1016;
                y = 2247;
                width = 1415;
                height = 945;
                Bitmap croppedImage5 =
                    source.Clone(new System.Drawing.Rectangle(x, y, width, height), source.PixelFormat);
                croppedImage5.RotateFlip(RotateFlipType.Rotate270FlipNone);
                croppedImage5.Save($@"D:\classic\classic\{number}.jpg", System.Drawing.Imaging.ImageFormat.Jpeg);

                number++;

                x = 50;
                y = 310;
                width = 945;
                height = 1415;
                Bitmap croppedImage1 =
                    source.Clone(new System.Drawing.Rectangle(x, y, width, height), source.PixelFormat);
                croppedImage1.Save($@"D:\classic\classic\{number}.jpg", System.Drawing.Imaging.ImageFormat.Jpeg);

                number++;

                x = 46;
                y = 1775;
                width = 945;
                height = 1415;
                Bitmap croppedImage2 =
                    source.Clone(new System.Drawing.Rectangle(x, y, width, height), source.PixelFormat);
                croppedImage2.Save($@"D:\classic\classic\{number}.jpg", System.Drawing.Imaging.ImageFormat.Jpeg);

                number++;
            }
        }

        public void ParseImages4()
        {
            var images = Directory.GetFiles(@"D:\prime\32", "*.*", SearchOption.AllDirectories)
                .ToList();

            var number = 1;
            for (int i = 0; i < images.Count; i++)
            {
                Bitmap source = new Bitmap(images[i]);

                int x = 234;
                int y = 194;
                int width = 508;
                int height = 746;
                Bitmap croppedImage3 =
                    source.Clone(new System.Drawing.Rectangle(x, y, width, height), source.PixelFormat);
                //croppedImage3.Save($@"D:\prime\{number}.jpg", System.Drawing.Imaging.ImageFormat.Jpeg);

                number++;

                x = 1002;
                y = 194;
                Bitmap croppedImage4 =
                    source.Clone(new System.Drawing.Rectangle(x, y, width, height), source.PixelFormat);
                croppedImage4.Save($@"D:\prime\{number}.jpg", System.Drawing.Imaging.ImageFormat.Jpeg);

                number++;

                x = 204;
                y = 1240;
                Bitmap croppedImage5 =
                    source.Clone(new System.Drawing.Rectangle(x, y, width, height), source.PixelFormat);
                croppedImage5.Save($@"D:\prime\{number}.jpg", System.Drawing.Imaging.ImageFormat.Jpeg);

                number++;

                x = 1004;
                y = 1240;
                Bitmap croppedImage1 =
                    source.Clone(new System.Drawing.Rectangle(x, y, width, height), source.PixelFormat);
                croppedImage1.Save($@"D:\prime\{number}.jpg", System.Drawing.Imaging.ImageFormat.Jpeg);

                number++;
            }
        }

        public void RenameFiles()
        {
            var images = Directory.GetFiles(@"D:\Classic-2", "*.*", SearchOption.AllDirectories)
                .ToList();
            int number = 0;
            for (int i = 0; i < images.Count; i++)
            {
                Bitmap source = new Bitmap(images[i]);
                number = i + 1;
                source.Save($@"D:\Classic\{number}.jpg", System.Drawing.Imaging.ImageFormat.Jpeg);
            }
        }
    }
}
