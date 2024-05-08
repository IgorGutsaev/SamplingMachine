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