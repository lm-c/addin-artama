@echo off
chcp 65001 > nul
setlocal enabledelayedexpansion

xcopy "C:\repositorio_corbie\00_CLIENTES\addin-artama\AddinArtama\AddinArtama\bin\Debug\*.*" "C:\Program Files\SOLIDWORKS Corp\SOLIDWORKS" /E /I /Y

echo pause