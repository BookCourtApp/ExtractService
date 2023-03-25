using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace BookParser;

public class Book
{
    public string Name { get; set; }

    public string Author { get; set; }

    public string Description { get; set; }

    public int Price { get; set; }

    public int Remainder { get; set; }

    public string SourceName { get; set; }
    
    public string Image { get; set; }

    public string NumberOfPages { get; set; }

    public string Genre { get; set; }

    public int NumberOfImages { get; set; }

    public string CoverType { get; set; }

    public string Language { get; set; }

    public string ISBN { get; set; }

    public string Publisher { get; set; }

    public string PublisherYear { get; set; }

    public string Series { get; set; }

    public string AgeRestrictions { get; set; }

    public string Format { get; set; }

    public string Weight { get; set; }

    public int Sales { get; set; }

    public string Reviews { get; set; }

    public string Rating { get; set; }

    public string VendorCode { get; set; }

    public DateTime ParsingDate { get; set; }

}