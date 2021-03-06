﻿// Copyright (c) Six Labors and contributors.
// Licensed under the Apache License, Version 2.0.

using SixLabors.ImageSharp.PixelFormats;

using BenchmarkDotNet.Attributes;

namespace SixLabors.ImageSharp.Benchmarks.Codecs.Jpeg
{
    using System.Drawing;
    using System.Drawing.Imaging;
    using System.IO;

    using CoreImage = SixLabors.ImageSharp.Image;

    public class EncodeJpeg : BenchmarkBase
    {
        // System.Drawing needs this.
        private Stream bmpStream;
        private Image bmpDrawing;
        private Image<Rgba32> bmpCore;

        [GlobalSetup]
        public void ReadImages()
        {
            if (this.bmpStream == null)
            {
                this.bmpStream = File.OpenRead("../ImageSharp.Tests/TestImages/Formats/Bmp/Car.bmp");
                this.bmpCore = CoreImage.Load<Rgba32>(this.bmpStream);
                this.bmpStream.Position = 0;
                this.bmpDrawing = Image.FromStream(this.bmpStream);
            }
        }

        [GlobalCleanup]
        public void Cleanup()
        {
            this.bmpStream.Dispose();
            this.bmpCore.Dispose();
            this.bmpDrawing.Dispose();
        }

        [Benchmark(Baseline = true, Description = "System.Drawing Jpeg")]
        public void JpegSystemDrawing()
        {
            using (var stream = new MemoryStream())
            {
                this.bmpDrawing.Save(stream, ImageFormat.Jpeg);
            }
        }

        [Benchmark(Description = "ImageSharp Jpeg")]
        public void JpegCore()
        {
            using (var stream = new MemoryStream())
            {
                this.bmpCore.SaveAsJpeg(stream);
            }
        }
    }
}