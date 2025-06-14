
using System.Drawing;
using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;
using System;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using System.Runtime.InteropServices;

namespace Shared.Services
{
    public class ImageProcessingService
    {
        private ILogger<ImageProcessingService> _logger;
        private IConfiguration _configuration;
        private readonly IWebHostEnvironment _env;

        public ImageProcessingService(ILogger<ImageProcessingService> logger, IConfiguration configuration, IWebHostEnvironment env)
        {
            _logger = logger;
            _configuration = configuration;
            _env = env;
        }



        public bool IsValidImage(byte[] file)
        {
            //string imagePath = "C:\\Users\\Omar\\Downloads\\WhatsApp Image 2025-02-22 at 2.02.06 PM (1).jpeg"; ;
            //Mat image = CvInvoke.Imread(imagePath, ImreadModes.Color);

            Mat image = new();
            CvInvoke.Imdecode(file, ImreadModes.Color, image);
            CascadeClassifier faceDetector = null;
            if (_env.IsProduction())
            {
                faceDetector = new CascadeClassifier(_configuration["EMGU:CascadeClassifier"]);

            }else{
                if(RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
                {
                    faceDetector = new CascadeClassifier(_configuration["EMGU:MacOS:CascadeClassifier"]);
                }
                else
                {
                    faceDetector = new CascadeClassifier(_configuration["EMGU:Windows:CascadeClassifier"]);

                }
            }
            // Load face detector (Haar Cascade)

            // Convert to grayscale
            Mat grayImage = new Mat();
            CvInvoke.CvtColor(image, grayImage, ColorConversion.Bgr2Gray);

            // Detect faces
            Rectangle[] faces = faceDetector.DetectMultiScale(grayImage, 1.1, 4);
            bool hasFace = faces.Length > 0;

            // Check background color
            bool isWhiteBackground = CheckWhiteBackground(image, faces);

            if (hasFace && isWhiteBackground)
            {
                _logger.LogInformation("The image contains a human with a white background.");
                return true;
            }
            else
            {
                _logger.LogInformation("The image does not meet the criteria.");
                return false;
            }


        }



        private bool CheckWhiteBackground(Mat image, Rectangle[] faceRegions)
        {
            int whiteThreshold = 200; // Adjust threshold for near-white
            int whitePixelCount = 0;
            int totalPixelCount = 0;

            for (int y = 0; y < image.Rows; y++)
            {
                for (int x = 0; x < image.Cols; x++)
                {
                    bool isFaceRegion = false;
                    foreach (var face in faceRegions)
                    {
                        if (face.Contains(x, y))
                        {
                            isFaceRegion = true;
                            break;
                        }
                    }
                    if (isFaceRegion) continue;

                    var pixel = image.GetRawData(y, x);
                    byte blue = pixel[0], green = pixel[1], red = pixel[2];

                    if (red > whiteThreshold && green > whiteThreshold && blue > whiteThreshold)
                    {
                        whitePixelCount++;
                    }
                    totalPixelCount++;
                }
            }

            double whiteRatio = (double)whitePixelCount / totalPixelCount;
            return whiteRatio > 0.3;
        }



    }
}
