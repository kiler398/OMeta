dotnet sonarscanner begin /k:"OMeta" /d:sonar.host.url="http://120.92.169.203:9192" /d:sonar.login="24a6b060dae5d3b5082a3183e3d54c02751fe568"
dotnet build OMeta.sln
dotnet sonarscanner end  /d:sonar.login="24a6b060dae5d3b5082a3183e3d54c02751fe568"