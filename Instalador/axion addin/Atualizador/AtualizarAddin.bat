@echo off
chcp 65001 > nul
setlocal enabledelayedexpansion

xcopy "C:\repositorio_corbie\00_CLIENTES\addin-artama - 4.0\AddinArtama\AddinArtama\bin\Debug\*.*" "C:\Program Files\SOLIDWORKS Corp\SOLIDWORKS" /E /I /Y

echo pause