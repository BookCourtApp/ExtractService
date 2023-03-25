// See https://aka.ms/new-console-template for more information

using LabirintExtractor;

ExtractorBooks parser = new ExtractorBooks();
parser.Parse("https://www.labirint.ru/books/", 848248, 848800);
