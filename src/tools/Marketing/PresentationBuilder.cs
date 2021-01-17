using Syncfusion.OfficeChartToImageConverter;
using Syncfusion.Presentation;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chroomsoft.Top2000.Tools.Marketing
{
    public class PresentationBuilder
    {
        private static IPresentation presentation = Presentation.Open("template.pptx");
        private static ISlide mainLeftSlide = presentation.Slides[0];
        private static ISlide mainRightSlide = presentation.Slides[1];
        private static ISlide defaultSlide = presentation.Slides[2];

        public PresentationBuilder AddMainSlide(byte[] pictureBytes, string text)
        {
            var leftSlide = mainLeftSlide.Clone();
            ((IPicture)leftSlide.Shapes[4]).ImageData = pictureBytes;
            presentation.Slides.Add(leftSlide);

            var rightSlide = mainRightSlide.Clone();
            ((IPicture)rightSlide.Shapes[5]).ImageData = pictureBytes;
            
            var textShape = (IShape)rightSlide.Shapes[4];
            
            var lines = text.Split('+').ToArray();

            var paragraphs = textShape.TextBody.Paragraphs;

            paragraphs[0].Text = lines[0];
            paragraphs[1].Text = lines[1];
            paragraphs[2].Text = lines[2];
            paragraphs[3].Text = lines[3];
            paragraphs[4].Text = GetIndexOfEmpty(lines, 4);
            paragraphs[5].Text = GetIndexOfEmpty(lines, 5);

            //foreach (var pargraph in paragraphs)
            //{


            //    _ = textShape.TextBody.AddParagraph(line);

            //}
            //rightSlide.Shapes.Add(textShape);

            presentation.Slides.Add(rightSlide);

            return this;
        }

        private string GetIndexOfEmpty(string[] array, int index)
        {
            return (index < array.Length)
                ? array[index]
                : string.Empty;
        }

        public PresentationBuilder AddInfoSlide(byte[] pictureBytes, string text)
        {
            var infoSlide = defaultSlide.Clone();
            //infoSlide.Background.Fill.SolidFill.Color = ColorObject.FromArgb()
            ((IShape)infoSlide.Shapes[1]).TextBody.Text = text.Replace("+", Environment.NewLine);
            ((IPicture)infoSlide.Shapes[1]).ImageData = pictureBytes;

            presentation.Slides.Add(infoSlide);

            return this;
        }

        public void Save(string location)
        {
            if (File.Exists(location))
                File.Delete(location);

            // we need to remove the template slides (first three)
            presentation.Slides.RemoveAt(0);
            presentation.Slides.RemoveAt(0);
            presentation.Slides.RemoveAt(0);

            presentation.Save(location);
        }

        public void SaveAsPictures()
        {
            // presentation.PresentationRenderer = new Syncfusion.PresentationRenderer.PresentationRenderer();
            // presentation.ChartToImageConverter = new ChartToImageConverter();
            foreach (var slide in presentation.Slides)
            {
                using (var image = Image.FromStream(slide.ConvertToImage(Syncfusion.Drawing.ImageFormat.Png)))
                {
                    image.Save(@$"S:\screenshots\{slide.SlideNumber}.png");
                }
            }
            //ISlide slide = presentation.Slides[0];
            //using (Image image = Image.FromStream(slide.ConvertToImage(ExportImageFormat.Png)))
            //{
            //    image.Save(@"C:\Users\RickNeeft\Desktop\nl_1.png");
            //}
        }
    }
}
