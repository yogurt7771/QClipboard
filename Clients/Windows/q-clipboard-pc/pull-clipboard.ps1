Add-Type -AssemblyName System.Windows.Forms

function PullClipboard()
{
    $config = Get-Content -Path "config.json" | ConvertFrom-Json
    $response = Invoke-WebRequest -Uri "$($config.url)/$($config.user)" -Headers @{Token=$config.token}
    if ($response.StatusCode -Eq 200)
    {
        $data = ConvertFrom-Json $response.Content
        $type = $data.type
        if ($type -Eq "text") {
            $content = $data.content
            if (![System.String]::IsNullOrEmpty($content))
            {
                [System.Windows.Forms.Clipboard]::SetText($content)
            }
            Write-Output "Copied: $content"
        }
        elseif ($type -Eq "image")
        {
            $content = $data.content
            $image_data = [System.Convert]::FromBase64String($content)
            $image_stream = [System.IO.MemoryStream]::new($image_data)
            $image = [System.Drawing.Image]::FromStream($image_stream, $true, $true)
            [System.Windows.Forms.Clipboard]::SetImage($image)
            Write-Output "Copied: <image>"
        }
        elseif ($type -Eq "file")
        {
            Write-Output "File name: $($data.content)"
            Start-Process -FilePath "$($config.url)$($data.url)"
        }
        else
        {
            Write-Output "Unsupported type: $type"
            Pause
        }
    }
    else
    {
        Write-Output "Access failed: $($response.StatusCode), $($response.StatusMessage)"
        Pause
    }
}

try
{
    PullClipboard
}
catch
{
    Write-Output "$($_.Exception.Message)"
    Pause
}
