using System.Drawing;
using System.Collections.Generic;
using System.IO;

class Province
{

    public int id;
    public string name;
    public string ownerTag = "NONE";
    public Point[] pixels = new Point[0];

    private Point firstPixel = new Point(0,0);  //  is not initlized in the constructor due to time complexity reasons


    //  private because it does not read the files to figure outs its own pixels
    //  a public constructor should do this
    private Province(string filePath)
    {

        string filename = Path.GetFileName(filePath).Replace(' ', '-');
        string idText = filename.Substring(0, filename.IndexOf('-'));
        
        this.id = int.Parse(idText);
        this.name = filename.Substring(filename.LastIndexOf('-') + 1).Replace(".txt", "");

        string[] text = File.ReadAllLines(filePath);

        int lineNumber = 0;
        foreach (string line in text)
        {

            if (line.StartsWith("owner = "))    //  this solution does not work for every province
            {
                this.ownerTag = line.Substring("owner = ".Length, 3);
            }
            //  deal with the annoying history feuture that paradox did not
            //  even utilize
            else if (line.StartsWith("14"))
            {
                if (int.Parse(line.Substring(0,4)) < 1445)
                {

                    int localLineNumber = lineNumber;
                    while (true)
                    {
                        string searchLine = text[localLineNumber++];

                        int ownerInfoIndex = searchLine.IndexOf("owner = ");
                        if (ownerInfoIndex > 0)
                        {
                            this.ownerTag = searchLine.Substring(ownerInfoIndex + "owner = ".Length, 3);
                            break;
                        }
                        if (searchLine.IndexOf('}') > 0)
                        {
                            break;
                        }
                    }

                }

            }

            lineNumber++;

        }



    }

    public void LoadPixels(Bitmap map)
    {
        this.pixels = BitSpread(this.firstPixel, map);
    }


    public override string ToString()
    {
        return $"{id} {ownerTag} {name} pixels = {pixels.Length}";
    }

    public static Province[] LoadAll()
    {

        List<Province> provinces = new List<Province>();

        string[] filePaths = Directory.GetFiles(Paths.provincesFolder);

        foreach (string filePath in filePaths)
        {
            provinces.Add(new Province(filePath));
        }

        string[] positionText = File.ReadAllLines(Paths.positions);

        foreach (Province province in provinces)
        {
            int line = province.id * 12 - 9;
            Point[] potentialFirstPixels = TextToPoints(positionText[line]);
            province.firstPixel = potentialFirstPixels[1];
        }

        return provinces.ToArray();

    }


    private static Point[] TextToPoints(string text)
    {
        Point[] points = new Point[7];

        text = text.Replace("		", "");
        text = text.Replace("-", "");
        text = text.Trim();

        string[] floatTexts = text.Split(' ');

        for (int i = 0; i < floatTexts.Length; i+=2)
        {
            string[] textX = floatTexts[i].Split('.');
            string[] textY = floatTexts[i + 1].Split('.');

            int valueX = int.Parse(textX[0]);
            int valueY = int.Parse(textY[0]);
            points[i / 2] = new Point(valueX, valueY);
        }
        
        return points;

    }

    //  realative index values for bordering indexes for an index
    private static int[] array_bordered_1d = new int[2] {-1,1};
    private static int[,] array_bordered_2d = new int[8,2] {
        {-1,1}  , {0,1}  , {1,1} ,
        {-1,0}  ,          {1,0} ,
        {-1,-1} , {0,-1} , {1,-1},
    };

    //  this function is very old
    //  i brought it from an older project
    public static Point[] BitSpread(Point cord, Bitmap input_bitmap){

        cord.Y = input_bitmap.Height - cord.Y;
        int width = input_bitmap.Width;
        int height = input_bitmap.Height;

        Color color = input_bitmap.GetPixel(cord.X, cord.Y); 

        //  maps
        Point[] points = new Point[ width * height ];       
        bool[,] worked = new bool[ width , height ];

        //  start points
        points[0] = cord;                         
        worked[cord.X, cord.Y] = true;          

        //  work total will be increasing, i will effectivly be equal to the work done and the index for the work to do
        int work_total = 1;
        for (int i = 0; i < work_total; i++){

            for (int j = 0; j < array_bordered_2d.GetLength(0); j++){

                int x = points[i].X + array_bordered_2d[j,0];
                int y = points[i].Y + array_bordered_2d[j,1];

                if (  (0 <= x && x < width)  &&  (0 <= y && y < height)  ){     //  will only check pixel if it's inside the bmp area
                if (input_bitmap.GetPixel(x, y).ToArgb() == color.ToArgb()){    //  if the pixel is of the right color
                if (worked[x,y] == false){                                      //  if pixel is not a duplicate

                    points[work_total].X = x;
                    points[work_total].Y = y;
                    worked[x,y] = true;                 //  mark the pixel as being registerd/noticed
                    work_total++;                       //  increase the pixels that will need to be worked(total)

                }
                }
                }

            }

        } 

        // at this point we got a number of point in cord_array that equal to the value in work_total
        Point[] output_points = new Point[work_total];
        for (int i = 0; i < work_total; i++){
            output_points[i] = points[i];
        }
        return output_points;

    }

}