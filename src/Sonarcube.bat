dotnet sonarscanner begin /k:"OMeta" /d:sonar.host.url="http://120.92.169.203:9192" /d:sonar.login="cd5769da76122b01818758cd50f6c8947f16f31a"
dotnet build OMeta.sln
dotnet sonarscanner end  /d:sonar.login="cd5769da76122b01818758cd50f6c8947f16f31a"