using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Reflection;
using iTextSharp.text;
using iTextSharp.text.pdf;
using Image = System.Drawing.Image;

namespace PicturesToPdf
{
  internal class Program

  {
    private const int ResizeRatio = 4;


    private const string Folder = "passport";

    private static void Main(string[] args)

    {
      var location = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

      var doc = new Document(PageSize.A4, 0, 0, 0, 0);

      var pdfFileStream = new FileStream(Path.Combine(location, $"{Folder}.pdf"), FileMode.Create);

      PdfWriter.GetInstance(doc, pdfFileStream);

      doc.Open();

      foreach (var file in Directory.GetFiles(Path.Combine(location, Folder)).OrderBy(x => x))

      {
        var originalImage = Image.FromFile(file);

        var resizedImage = (Image) new Bitmap(originalImage,
          new Size(originalImage.Width / ResizeRatio, originalImage.Height / ResizeRatio));

        var im = iTextSharp.text.Image.GetInstance(resizedImage, ImageFormat.Jpeg);
        im.ScaleToFit(doc.PageSize.Width, doc.PageSize.Height);

        doc.Add(im);

        doc.NewPage();
      }
      doc.Close();
    }
  }
}