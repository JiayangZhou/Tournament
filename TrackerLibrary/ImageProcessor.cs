using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using System.IO;
using System.Threading.Tasks;
using ImageProcessor;
using ImageProcessor.Processors;
using ImageProcessor.Imaging.Formats;
using TrackerLibrary;
using System.Drawing;
using AForge.Imaging;
using AForge.Imaging.Filters;
using AForge;

namespace TrackerLibrary
{
    public class ImageProcessor
    {
        private const string ImageFile = "me.png";
        //private const string ImageFileLocation = "C:/Users/dengd/source/repos/Tournament/me.png";


        //public string RotateImage(string picLocaion)
        //{
        //    byte[] photoBytes = File.ReadAllBytes(picLocaion);
        //    var format = new JpegFormat();
        //    var size = new Size(150, 0);
        //    using (MemoryStream inStream = new MemoryStream(photoBytes))
        //    {
        //        using (MemoryStream outStream = new MemoryStream())
        //        {
        //            using (ImageFactory imageFactory = new ImageFactory(preserveExifData: true))
        //            {
        //                imageFactory.Load()
        //                imageFactory.Load(inStream)
        //                            .Resize(size)
        //                            .Format(format)
        //                            .Save(outStream);

        //            }
        //        }
        //    }

        //    return picLocaion;
        //}

        void CompareTwoImages()
        {
            
        }

    }
}
