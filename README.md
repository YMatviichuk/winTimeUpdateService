# winTimeUpdateService
This service updates Windows system time on startup / logon using NIST NTP.

How to install service
1. Open dev command prompt for VS2017
2. Go to directory with TimeUpdateService.exe
3. installutil TimeUpdateService.exe
4. Put your credentials to Windows (use .\<username> if you use local account)
5. Go to services administrative tool and turn on TimeUpdateService
