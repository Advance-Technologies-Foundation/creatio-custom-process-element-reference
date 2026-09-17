param([Parameter(Mandatory)][string]$EnvironmentName)
$ErrorActionPreference = 'Stop'
Push-Location (Split-Path $PSScriptRoot -Parent)
try {
    & (Join-Path $PSScriptRoot 'Test-PackageContract.ps1')
    # Reuse registered credentials without writing them into the repository or command line.
    $configuration = Get-Content (Join-Path $env:LOCALAPPDATA 'creatio/clio/appsettings.json') -Raw | ConvertFrom-Json
    $target = $configuration.Environments.$EnvironmentName
    if (!$target) { throw "Clio environment '$EnvironmentName' is not registered." }
    $env:CREATIO_URL = $target.Uri
    $env:CREATIO_IS_NETCORE = 'true'
    $env:CREATIO_USERNAME = $target.Login
    $env:CREATIO_PASSWORD = $target.Password
    if ($env:CREATIO_ACCESS_TOKEN) { throw 'Clear CREATIO_ACCESS_TOKEN before using this registered-credentials recipe.' }
    dotnet test tests/UsrCustomProcessElement/UsrCustomProcessElement.Tests.csproj -c dev-n8 --filter 'FullyQualifiedName~FormatText|FullyQualifiedName~Arithmetic'
    if ($LASTEXITCODE) { throw 'Unit tests failed.' }
    dotnet test tests/UsrCustomProcessElement.IntegrationTests/UsrCustomProcessElement.IntegrationTests.csproj --filter 'FullyQualifiedName~FormatTextProcessTests|FullyQualifiedName~ArithmeticProcessTests'
    if ($LASTEXITCODE) { throw 'Live process tests failed.' }
} finally {
    Remove-Item Env:CREATIO_PASSWORD -ErrorAction SilentlyContinue
    Pop-Location
}
