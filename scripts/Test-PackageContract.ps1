param([string]$PackagePath = (Join-Path (Split-Path $PSScriptRoot -Parent) 'packages/UsrCustomProcessElement'))
$ErrorActionPreference = 'Stop'

# Check exported metadata independently of generated C# and a running Creatio instance.
$contracts = @{
    UsrFormatTextUserTask = @{ Inputs = @('Text', 'Prefix'); Result = 'FormattedText' }
    UsrAddNumbersUserTask = @{ Inputs = @('FirstAddend', 'SecondAddend'); Result = 'Result' }
    UsrSubtractNumbersUserTask = @{ Inputs = @('Minuend', 'Subtrahend'); Result = 'Result' }
    UsrMultiplyNumbersUserTask = @{ Inputs = @('Multiplicand', 'Multiplier'); Result = 'Result' }
    UsrDivideNumbersUserTask = @{ Inputs = @('Dividend', 'Divisor'); Result = 'Result' }
}
foreach ($name in $contracts.Keys) {
    $schema = (Get-Content (Join-Path $PackagePath "Schemas/$name/metadata.json") -Raw | ConvertFrom-Json).MetaData.Schema
    $contract = $contracts[$name]
    $parameters = @($schema.FJ1)
    if ($parameters.Count -ne 5) { throw "$name must expose exactly two inputs and three outputs." }
    foreach ($parameterName in @($contract.Inputs) + @($contract.Result, 'IsError', 'ErrorMessage')) {
        $matches = @($parameters | Where-Object A2 -eq $parameterName)
        if ($matches.Count -ne 1) { throw "$name requires exactly one $parameterName parameter." }
        $parameter = $matches[0]
        $direction = if ($parameterName -in $contract.Inputs) { 0 } else { 1 }
        if ($null -eq $parameter.PSObject.Properties['L12'] -or $parameter.L12 -ne $direction) {
            throw "$name.$parameterName must explicitly declare L12=$direction; omitted metadata means Variable."
        }
        if ($parameterName -eq 'ErrorMessage' -and $parameter.L1 -ne 'c0f04627-4620-4bc0-84e5-9419dc8516b1') {
            throw "$name.ErrorMessage must use native unlimited text."
        }
        if ($parameterName -eq 'IsError' -and $parameter.L1 -ne '90b65bf8-0ffc-4141-8779-2420877af907') {
            throw "$name.IsError must use native Boolean."
        }
    }
}
Write-Output 'Package contract passed: five tasks, explicit In/Out directions, Boolean error flag and unlimited error text.'
