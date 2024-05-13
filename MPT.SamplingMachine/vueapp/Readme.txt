To run app in release mode

0. Install .NET SDK
1. Open vueapp in Terminal
2. Build VUE application 'npm run build'
2. Open build folder, i.e. D:\Repos\SamplingMachine\MPT.SamplingMachine\vueapp\dist
2.1. Copy aspnetcore-https.js to the destination folder
3. npm run serve 
4. Open https://localhost:5002/ in browser

If error 'npm install exit code -1' run on vueapp:
npm install --force @microsoft/signalr

If it doesn't start, do the following:

1) copy src folder, babel.config.js and vue.config.js
2) npm install --force @microsoft/signalr
3) npm install --force mitt



-- Renew ssl certificate

1) Open privileged PS or cmd and go to C:\Program Files\Git\usr\bin or download openssl.exe
2) run- 'openssl req -newkey rsa:2048 -nodes -keyout samplingmachine.key -x509 -days 365 -out samplingmachine.crt'
3) export the new certificate- 'openssl pkcs12 -inkey samplingmachine.pem -in samplingmachine.cert -export -out samplingmachine.pfx'
4) copy .key & .pem files from openssl folder to asp.NET folder (C:\Users\[Username]\AppData\Roaming\ASP.NET\https)
5) import samplingmachine.pfx to local machine and current user 'Trusted Root Certification' folder
6) restart chrome