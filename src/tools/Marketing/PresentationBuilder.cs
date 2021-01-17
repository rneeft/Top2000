using Syncfusion.Presentation;
using System.Drawing;
using System.IO;
using System.Linq;

namespace Chroomsoft.Top2000.Tools.Marketing
{
    public class PresentationBuilder
    {
        private static readonly IPresentation presentation = Presentation.Open("template.pptx");
        private static readonly ISlide mainLeftSlide = presentation.Slides[0];
        private static readonly ISlide mainRightSlide = presentation.Slides[1];
        private static readonly ISlide defaultSlide = presentation.Slides[2];

        public PresentationBuilder AddMainSlide(byte[] pictureBytes, string text)
        {
            var leftSlide = mainLeftSlide.Clone();
            ((IPicture)leftSlide.Shapes[0]).ImageData = pictureBytes;
            presentation.Slides.Add(leftSlide);

            var rightSlide = mainRightSlide.Clone();
            ((IPicture)rightSlide.Shapes[0]).ImageData = pictureBytes;
            ChangeTextParagraphs((IShape)rightSlide.Shapes[2], text);

            presentation.Slides.Add(rightSlide);

            return this;
        }

        public PresentationBuilder AddInfoSlide(byte[] pictureBytes, string text)
        {
            var infoSlide = defaultSlide.Clone();
            ((IPicture)infoSlide.Shapes[1]).ImageData = pictureBytes;
            ChangeTextParagraphs((IShape)infoSlide.Shapes[0], text);

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
            foreach (var slide in presentation.Slides)
            {
                using var image = Image.FromStream(slide.ConvertToImage(Syncfusion.Drawing.ImageFormat.Png));
                image.Save(@$"S:\screenshots\{slide.SlideNumber}.png");
            }
        }

        private void ChangeTextParagraphs(IShape shape, string text)
        {
            var lines = text.Split('+').ToArray();

            var paragraphs = shape.TextBody.Paragraphs;

            for (int i = 0; i < paragraphs.Count(); i++)
            {
                paragraphs[i].Text = GetIndexOfEmpty(lines, i);
            }
        }

        private string GetIndexOfEmpty(string[] array, int index)
        {
            return (index < array.Length)
                ? array[index]
                : string.Empty;
        }
    }
}
