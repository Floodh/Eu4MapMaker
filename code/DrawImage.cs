using System;
using System.Drawing;
using System.Drawing.Imaging;
using nQuant;

class DrawImage{

    //  template / base image
    private static Bitmap eu4_outputmap;
    private static bool setup_completed = false;
    
    public static void Setup(){

        //  template / base image
        eu4_outputmap = new Bitmap(Paths.colorMap);
        setup_completed = true;

    }

    public static void Reset(){

        eu4_outputmap = new Bitmap(Paths.colorMap);

    }

    public static void DrawCountry(Country country){

        if (setup_completed == false){throw new Exception("Tried to call function that required setup (Class: DrawMap)");}

        foreach(Province province in country.provinces){

            foreach (Point pixel in province.pixels){

                eu4_outputmap.SetPixel(pixel.X, pixel.Y, country.color);
            }
        }

    }

    //  save the result as a .bmp file
    public static void SaveAsBMP(){

        eu4_outputmap.Save(Paths.colorMap);

    }

    public static void SaveAsPng(){

        eu4_outputmap.Save(Paths.outputPathPng, ImageFormat.Png);

    }

    public static void SaveAsCompressedPng(){

        SaveAsPng();        //  temporary solution

        //Encoder ColorDepth = ;
        //Encoder Compression = Encoder.Compression;
        var quantizer = new WuQuantizer();
        using(var bitmap = Convert(new Bitmap(Paths.outputPathPng)))
        {
            using(var quantized = quantizer.QuantizeImage(bitmap))
            {
                quantized.Save(Paths.outputPathPngCompressed, ImageFormat.Png);
            }
        }       
        //eu4_outputmap.Save(Settings.path_outputfile_png, ImageFormat.Png, );

    }

    //  i don't remeber why i did this
    private static Bitmap Convert(Bitmap img){
        
        var bmp = new Bitmap(img.Width, img.Height, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
        using (var gr = Graphics.FromImage(bmp))
            gr.DrawImage(img, new Rectangle(0, 0, img.Width, img.Height));

        return bmp;
    }

}