using HtmlToPdf.ConversionService.Business.Configuration;
using HtmlToPdf.ConversionService.Business.Services.Interfaces;
using PuppeteerSharp;

namespace HtmlToPdf.ConversionService.Business.Services;

public class PuppeteerConversionService : IFileConversionService
{
    private readonly PuppeteerConfiguration _puppeteerConfiguration;
    
    public PuppeteerConversionService(PuppeteerConfiguration puppeteerConfiguration)
    {
        _puppeteerConfiguration = puppeteerConfiguration;
    }

    public async Task<string> ConvertToPdf(string filePath)
    {
        await using var browser = await Puppeteer.LaunchAsync(new LaunchOptions
        {
            Headless = true,
            ExecutablePath = _puppeteerConfiguration.ChromeExecutablePath
        });
        
        await using var page = await browser.NewPageAsync();
        
        var originalFile = await File.ReadAllTextAsync(filePath);
        
        await page.SetContentAsync(originalFile);
        
        var pdfStream = await page.PdfStreamAsync();

        var destinationFilePath = Path.ChangeExtension(filePath, ".pdf");
        
        await using var destinationFile = File.Create(destinationFilePath);
        
        await pdfStream.CopyToAsync(destinationFile);

        return destinationFilePath;
    }
}