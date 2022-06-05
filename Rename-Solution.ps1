Param(
    $NewName
)

$ExcludedPaths = "bin*|obj*|Rename-Solution*"
$CurrentName = "TheHossGame"

function Get-DirectoriesStartingWith($Start) {
    Get-ChildItem -Recurse -Filter ($Start + '**')
    | Where-Object {
        $_.FullName -notmatch $ExcludedPaths
    } 
}

function Get-AllChildItems() {
    return Get-ChildItem -Recurse -File
}

function Rename-Items {
    param(
        [Parameter(ValueFromPipeline = $true)]
        $File
    )
    Begin {}
    Process {
        Rename-Item $File $File.Name.Replace($CurrentName, $NewName)
    }
    End {}
}

function Update-FilesContent {
    param (
        [Parameter(ValueFromPipeline = $true)]
        $File
    )
    Begin {}
    Process {
        $Content = Get-Content $_
        $NewContent = $Content.Replace($CurrentName, $NewName)
        Set-Content -Path $_ $NewContent
    }
    End {}
}

Get-DirectoriesStartingWith -Start $CurrentName | Rename-Items
Get-AllChildItems | Update-FilesContent

