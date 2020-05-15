dotnet sonarscanner  begin /k:"OMeta" /d:sonar.host.url="http://120.92.159.240:9192" /d:sonar.login="9d1667e3de914398031dec481197d15a2304ec4d"
dotnet build OMeta.sln
dotnet sonarscanner end  /d:sonar.login="9d1667e3de914398031dec481197d15a2304ec4d" 