# ============================================
# Open Projects in Visual Studio 2026
# ============================================
# This script helps you open projects easily

Write-Host @"

╔═══════════════════════════════════════════════════════════╗
║                                                           ║
║   Visual Programming Practice - .NET 10 Projects          ║
║   Upgraded for Visual Studio 2026                         ║
║                                                           ║
╚═══════════════════════════════════════════════════════════╝

"@ -ForegroundColor Cyan

$solutionPath = "e:\Visual Programing Practice\Taska\Taska.sln"

Write-Host "`nAvailable Options:" -ForegroundColor Yellow
Write-Host "==================`n" -ForegroundColor Yellow

Write-Host "1. Open Entire Solution (All 11 Projects)" -ForegroundColor Green
Write-Host "2. ADO_WinForms - Windows Forms Desktop" -ForegroundColor White
Write-Host "3. ADO_CRUD - Web Forms with ADO.NET" -ForegroundColor White
Write-Host "4. DbCon_CRUD - Web Forms with DbCon Utility" -ForegroundColor White
Write-Host "5. ThreeTier_CRUD - 3-Tier Web Forms" -ForegroundColor White
Write-Host "6. StoredProcedure_CRUD - Web Forms with SPs" -ForegroundColor White
Write-Host "7. MasterPage_Demo - Master Pages" -ForegroundColor White
Write-Host "8. Auth_WebForms - Authentication Demo" -ForegroundColor White
Write-Host "9. Auth_WebForms_Connected - Connected Auth" -ForegroundColor White
Write-Host "10. REST_API - RESTful API with EF Core" -ForegroundColor White
Write-Host "11. API_Consumer - API Client" -ForegroundColor White
Write-Host "12. Core_MVC_EF - ASP.NET Core MVC" -ForegroundColor White
Write-Host "`n0. Exit`n" -ForegroundColor Red

$choice = Read-Host "Enter your choice (0-12)"

switch ($choice) {
    "1" {
        Write-Host "`nOpening entire solution in Visual Studio 2026..." -ForegroundColor Cyan
        Start-Process $solutionPath
    }
    "2" {
        Write-Host "`nOpening ADO_WinForms project..." -ForegroundColor Cyan
        Start-Process "e:\Visual Programing Practice\Taska\ADO_WinForms\ADO_WinForms.csproj"
    }
    "3" {
        Write-Host "`nOpening ADO_CRUD project..." -ForegroundColor Cyan
        Start-Process "e:\Visual Programing Practice\Taska\ADO_CRUD\ADO_CRUD.csproj"
    }
    "4" {
        Write-Host "`nOpening DbCon_CRUD project..." -ForegroundColor Cyan
        Start-Process "e:\Visual Programing Practice\Taska\DbCon_CRUD\DbCon_CRUD.csproj"
    }
    "5" {
        Write-Host "`nOpening ThreeTier_CRUD project..." -ForegroundColor Cyan
        Start-Process "e:\Visual Programing Practice\Taska\ThreeTier_CRUD\ThreeTier_CRUD.csproj"
    }
    "6" {
        Write-Host "`nOpening StoredProcedure_CRUD project..." -ForegroundColor Cyan
        Start-Process "e:\Visual Programing Practice\Taska\StoredProcedure_CRUD\StoredProcedure_CRUD.csproj"
    }
    "7" {
        Write-Host "`nOpening MasterPage_Demo project..." -ForegroundColor Cyan
        Start-Process "e:\Visual Programing Practice\Taska\MasterPage_Demo\MasterPage_Demo.csproj"
    }
    "8" {
        Write-Host "`nOpening Auth_WebForms project..." -ForegroundColor Cyan
        Start-Process "e:\Visual Programing Practice\Taska\Auth_WebForms\Auth_WebForms.csproj"
    }
    "9" {
        Write-Host "`nOpening Auth_WebForms_Connected project..." -ForegroundColor Cyan
        Start-Process "e:\Visual Programing Practice\Taska\Auth_WebForms_Connected\Auth_WebForms_Connected.csproj"
    }
    "10" {
        Write-Host "`nOpening REST_API project..." -ForegroundColor Cyan
        Start-Process "e:\Visual Programing Practice\Taska\REST_API\REST_API.csproj"
    }
    "11" {
        Write-Host "`nOpening API_Consumer project..." -ForegroundColor Cyan
        Start-Process "e:\Visual Programing Practice\Taska\API_Consumer\API_Consumer.csproj"
    }
    "12" {
        Write-Host "`nOpening Core_MVC_EF project..." -ForegroundColor Cyan
        Start-Process "e:\Visual Programing Practice\Taska\Core_MVC_EF\Core_MVC_EF.csproj"
    }
    "0" {
        Write-Host "`nExiting... Goodbye!`n" -ForegroundColor Yellow
        exit
    }
    default {
        Write-Host "`nInvalid choice. Please run the script again.`n" -ForegroundColor Red
    }
}

Write-Host "`nDone! Visual Studio 2026 should open shortly.`n" -ForegroundColor Green
Write-Host "First time opening? Visual Studio will:" -ForegroundColor Yellow
Write-Host "  1. Restore NuGet packages" -ForegroundColor White
Write-Host "  2. Download .NET 10 SDK (if not installed)" -ForegroundColor White
Write-Host "  3. Build the project`n" -ForegroundColor White

Write-Host "Press any key to exit..." -ForegroundColor Gray
$null = $Host.UI.RawUI.ReadKey("NoEcho,IncludeKeyDown")
