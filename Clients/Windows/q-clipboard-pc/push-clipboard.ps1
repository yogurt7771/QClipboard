Add-Type -AssemblyName System.Windows.Forms

function PushClipboardText()
{
    $config = Get-Content -Path "config.json" | ConvertFrom-Json
    $content = [System.Windows.Forms.Clipboard]::GetText()
    $data = @{type="text"; content=$content}
    $data_json = ConvertTo-Json -InputObject $data -Compress
    $response = Invoke-WebRequest -Uri "$($config.url)/$($config.user)" -Headers @{Token=$config.token} -Body $data_json -ContentType 'application/json' -Method POST
    if ($response.StatusCode -Eq 200) {
        Write-Output "Pushed: '$content'"
    } else {
        Write-Output "Push '$content' failed: $($response.StatusCode), $($response.StatusMessage)"
        Pause
    }
}

function PushClipboardImage()
{
    $config = Get-Content -Path "config.json" | ConvertFrom-Json
    $image = [System.Windows.Forms.Clipboard]::GetImage()
    $stream = [System.IO.MemoryStream]::new()
    $image.Save($stream, [System.Drawing.Imaging.ImageFormat]::Png)
    $stream.Position = 0
    $content = [System.Convert]::ToBase64String($stream.GetBuffer())
    $data = @{type="image"; content=$content}
    $data_json = ConvertTo-Json -InputObject $data -Compress
    $response = Invoke-WebRequest -Uri "$($config.url)/$($config.user)" -Headers @{Token=$config.token} -Body $data_json -ContentType 'application/json' -Method POST
    if ($response.StatusCode -Eq 200) {
        Write-Output "Pushed: <image>"
    } else {
        Write-Output "Push image failed: $($response.StatusCode), $($response.StatusMessage)"
        Pause
    }
}

function PushClipboardFile()
{
    $config = Get-Content -Path "config.json" | ConvertFrom-Json
    foreach ($file in [System.Windows.Forms.Clipboard]::GetFileDropList())
    {
        if (![System.IO.File]::Exists(($file)))
        {
            continue;
        }
        $form = @{
            type="file";
            file=Get-Item -Path $file;
        }
        $response = Invoke-WebRequest -Uri "$($config.url)/$($config.user)/upload" -Headers @{Token=$config.token} -Form $form -ContentType 'multipart/form-data; boundary=----53014704754052338' -Method POST
        if ($response.StatusCode -Eq 200) {
            Write-Output "Pushed: '$file'"
        } else {
            Write-Output "Push '$file' failed: $($response.StatusCode), $($response.StatusMessage)"
            Pause
        }
        return
    }
    Write-Output "No file found (Cannot upload folder)"
    Pause
}

function PushClipboard()
{
    if ([System.Windows.Forms.Clipboard]::ContainsText())
    {
        PushClipboardText
    }
    elseif ([System.Windows.Forms.Clipboard]::ContainsImage())
    {
        PushClipboardImage
    }
    elseif ([System.Windows.Forms.Clipboard]::ContainsFileDropList())
    {
        PushClipboardFile
    }
    else
    {
        Write-Output "Unsupported clipboard type"
        Pause
    }
}

try
{
    PushClipboard
}
catch
{
    Write-Output "$($_.Exception.Message)"
    Pause
}
