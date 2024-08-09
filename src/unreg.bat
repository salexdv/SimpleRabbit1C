@echo off
setlocal
%SystemRoot%\Microsoft.NET\Framework\v4.0.30319\RegAsm.exe  "bin\Release\SimpleRabbit1C.dll" /unregister /verbose

%SystemRoot%\Microsoft.NET\Framework64\v4.0.30319\RegAsm.exe  "bin\Release\SimpleRabbit1C.dll" /unregister /verbose