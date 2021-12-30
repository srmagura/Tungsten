# In PowerShell, run: Import-Module SetAliases.ps1

Set-Alias -Name tflatc -Value "$PSScriptRoot/../Compiler/TFlat.Compiler/bin/Debug/net6.0/TFlat.Compiler.exe"
Set-Alias -Name tflatdasm -Value "$PSScriptRoot/../Compiler/TFlat.Disassembler/bin/Debug/net6.0/TFlat.Disassembler.exe"
Set-Alias -Name tflat -Value "$PSScriptRoot/../Runtime/TFlat.Runtime/bin/Debug/net6.0/TFlat.Runtime.exe"
