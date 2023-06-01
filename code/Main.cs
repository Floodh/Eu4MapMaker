//  dotnet add package System.Drawing.Common --version 7.0.0
//  dotnet add package nQuant

using System;

static class MainClass
{

    const int debugLevel = 1;

    static int Main()
    {

        WriteLine("Start", 1);


        WriteLine("Loading provinces", 1);
        Province[] provinces = Province.LoadAll();

        foreach (Province province in provinces)
        {
            if (province.id < 1000)
                WriteLine(province.ToString(), 2);
        }

        WriteLine("Loading Countries", 1);
        Country[] countries = Country.LoadFromFile(Paths.inputTags);
        foreach (Country country in countries)
        {
            country.CollectOwnedProvinces(provinces);
        }

        foreach (Country country in countries)
        {
            WriteLine(country.ToString(), 2);
        }


        WriteLine("Drawing image", 1);
        DrawImage.Setup();
        foreach (Country country in countries)
        {
            DrawImage.DrawCountry(country);
        }        
        DrawImage.SaveAsCompressedPng();

        WriteLine("End", 1);

        return 0;
    }


    static void WriteLine(string line, int level)
    {
        if (debugLevel >= level)
            Console.WriteLine(line);
    }
}