using System.Drawing;
using System.Collections.Generic;
using System.IO;

class Country
{  
    public string tag;
    public string name;
    public Color color;

    public List<Province> provinces = new List<Province>();
    public Country(string tag, string name, string filePath)
    {
        this.tag = tag;
        this.name = name;

        string[] text = File.ReadAllLines(filePath);
        foreach (string line in text)
        {
            if (line.StartsWith("color ="))
            {
                string colorText = line.Substring("color =".Length);
                this.color = StringToColor(colorText);
            }
        }

    }

    public void CollectOwnedProvinces(Province[] provinces)
    {
        Bitmap map = new Bitmap(Paths.provinces);
        foreach (Province province in provinces)
        {
            if (this.tag == province.ownerTag)
            {
                province.LoadPixels(map);   //  only when we know the province will be drawn do we need to do this.
                this.provinces.Add(province);
            }
        }

    }



    public override string ToString()
    {
        return $"{tag} {name} {color.ToString()} provinces = {provinces.Count}";
    }


    public static Country[] LoadAll()
    {
        List<Country> countries = new List<Country>();



        string[] text = File.ReadAllLines(Paths.countryTags);
        foreach (string line in text)
        {
            if (!line.StartsWith('#') && line.Length > 10)
            {
                string tag = line.Substring(0, 3);

                int startIndex = line.IndexOf('\"') + 1;
                int endIndex = line.LastIndexOf('\"') - line.IndexOf('\"') - 1;
                string relPath = line.Substring(startIndex, endIndex);
                string name = relPath.Substring("countries/".Length);
                name = name.Substring(0, name.Length - ".txt".Length);

                countries.Add(new Country(tag, name, Paths.commonFolder + "/" + relPath));
            }
        }

        return countries.ToArray();        
    }

    public static Country[] LoadFromFile(string path)
    {
        List<Country> result = new List<Country>();
        Country[] countries = LoadAll();
        string[] fileContent = File.ReadAllLines(path);

        foreach (string line in fileContent)
            foreach (Country country in countries)
                if (country.name == line || country.tag == line)
                    result.Add(country);

        return result.ToArray();
    }

    private static Color StringToColor(string text)
    {
        text = text.Replace('{', ' ');
        text = text.Replace('}', ' ');
        text = text.Replace('=', ' ');
        text = text.Replace('\n', ' ');

        int digit = 0;
        byte[] values = new byte[3];
        int index = 0;
        foreach (char c in text)
        {
            if (c != ' ')
            {
                byte value = (byte)(c - 48);
                values[index] *= 10;
                values[index] += value;
                digit++;
            }
            else if (digit > 0)
            {
                index++;
                digit = 0;
                if (index > 2)
                    break;
            }
        }

        return Color.FromArgb(values[0], values[1], values[2]);
    }

}