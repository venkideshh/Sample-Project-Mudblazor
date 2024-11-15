namespace EDC.Client;

public class AppTheme : MudTheme
{
    public AppTheme() : base()
    {
        var font = new string[] { "Noto Sans", "sans-serif" };

        Typography.Body1.FontFamily = font;
        Typography.Body1.FontWeight = 400;
        Typography.Body1.FontSize = "1rem";
        Typography.Body2.FontFamily = font;
        Typography.Body2.FontWeight = 400;
        Typography.Body2.FontSize = ".75rem";
        Typography.Button.FontFamily = font;
        Typography.Caption.FontFamily = font;
        Typography.Default.FontFamily = font;
        Typography.H1.FontFamily = font;
        Typography.H1.FontWeight = 400;
        Typography.H1.FontSize = "2.50rem";
        Typography.H2.FontFamily = font;
        Typography.H2.FontWeight = 400;
        Typography.H2.FontSize = "2.25rem";
        Typography.H3.FontFamily = font;
        Typography.H3.FontWeight = 400;
        Typography.H3.FontSize = "2.00rem";
        Typography.H4.FontFamily = font;
        Typography.H4.FontWeight = 400;
        Typography.H4.FontSize = "1.75rem";
        Typography.H5.FontFamily = font;
        Typography.H5.FontWeight = 400;
        Typography.H5.FontSize = "1.50rem";
        Typography.H6.FontFamily = font;
        Typography.H6.FontWeight = 400;
        Typography.H6.FontSize = "1.25rem";
        Typography.Overline.FontFamily = font;
        Typography.Subtitle1.FontFamily = font;
        Typography.Subtitle2.FontFamily = font;

        PaletteLight.AppbarBackground = "#4B49AC";
        PaletteLight.AppbarText = "#e1e1e1";
        PaletteLight.Primary = "#4B49AC";
        PaletteLight.PrimaryLighten = "#98BDFF";
        PaletteLight.PrimaryDarken = "#4B49AC";
        PaletteLight.TextPrimary = "#101010";
        PaletteLight.Secondary = "#5B8EF7"; 
        PaletteLight.SecondaryLighten = "#4A76CF";
        PaletteLight.SecondaryDarken = "#3B5B99";
        PaletteLight.Info = "#2A91FF"; 
        PaletteLight.InfoLighten = "#5DB1FF";
        PaletteLight.InfoDarken = "#236FCC";
        PaletteLight.Success = "#4CAF50";
        PaletteLight.Error = "#F44336";

        PaletteDark.AppbarBackground = "#4B49AC";
        PaletteDark.AppbarText = "#e1e1e1";
        PaletteDark.Primary = "#4B49AC";
        PaletteDark.PrimaryLighten = "#98BDFF";
        PaletteDark.PrimaryDarken = "#4B49AC";
        PaletteDark.TextPrimary = "#e1e1e1";
        PaletteDark.Secondary = "#5B8EF7"; 
        PaletteDark.SecondaryLighten = "#4A76CF";
        PaletteDark.SecondaryDarken = "#3B5B99";    
        PaletteDark.Info = "#2A91FF";  
        PaletteDark.InfoLighten = "#5DB1FF"; 
        PaletteDark.InfoDarken = "#236FCC";  
        PaletteDark.Success = "#4CAF50";   
        PaletteDark.Error = "#F44336";  
    }
}
