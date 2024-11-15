namespace EDC.Shared.Config;

public class ApplicationSettings
{
    public ApplicationConfiguration Config { get; set; }
    public AuthSettings Auth { get; set; }
    public FileUploadSettings FileUpload { get; set; }

    public ApplicationSettings()
    {
        Config = new();
        Auth = new();
        FileUpload = new();
    }
}

public class ApplicationConfiguration
{
    public bool UseHttpsRedirection { get; set; }
    public bool UseSwagger { get; set; }
    public bool UseHsts { get; set; }
    public bool UseWasmDebug { get; set; }
    public bool UseErrorPage { get; set; }
}

public class AuthSettings
{
    public string Issuer { get; set; }
    public string Audience { get; set; }
    public string Key { get; set; }
    public int Expiry { get; set; }
}

public class FileUploadSettings
{
    public string[] FileTypes { get; set; }
    public decimal MaxSizeinMB { get; set; }
    public string Path { get; set; }
}