var log = console.log;
var debug = console.debug;
var error = console.error;

async function logData(args, level, handler) {
    try {
        if (args != null && args[0] != null && typeof args[0] == "string") {
            args[0] = args[0].replace(/\\r\\n/g, '\r\n');
        }

        if (handler) {
            handler.apply(this, Array.prototype.slice.call(args));
        }

        if (args != null) {

            var toLog = Object.values(args).join();
            if (toLog.substr) {
                toLog = toLog.substr(0, 32000);
            }
            var logLevel = 0;
            switch (level) {
                case "DEBUG":
                    logLevel = 1;
                    break;
                case "ERROR":
                    logLevel = -1;
                    break;
            }
            //$.ajax({
            //    type: "POST",
            //    url: `/log/${logLevel}`,
            //    data: toLog,
            //    async: true,
            //    contentType: 'text/plain',
            //    timeout: 3000
            //});


            await fetch(`kiosk/log/${logLevel}`, {
                method: 'POST',
                headers: {
                    'Content-Type': 'text/plain'
                },
                body: toLog
            })

        }
    } catch (e) {
        error(`http-logger: ${e}`);
    }
}

console.log = async function () {
    logData(arguments, "INFO", log);
};

console.debug = async function () {
    logData(arguments, "DEBUG", debug);
};

console.error = async function () {
    logData(arguments, "ERROR", error);
};

window.onerror = function (message, file, line, col, error) {
    if (error && error.message) {
        message = error.message;
    }
    var str = "Error occurred: " + message;
    for (var i in error) {
        if (i == "message") {
            continue;
        }

        str += "\r\n" + i + " : " + error[i];
    }
    str += "\r\nin file " + file + " at line: " + line + " col: " + col;
    console.error(str);
    return false;
};

window.addEventListener("unhandledrejection", function (promiseRejectionEvent) {
    var msg = "Some unhandled promise rejection happened without any error message";
    if (promiseRejectionEvent && promiseRejectionEvent.reason) {
        msg += `\r\n ${JSON.stringify(promiseRejectionEvent)} `;

        if (typeof (promiseRejectionEvent.reason) == 'string')
            msg += `\r\n Reason=${promiseRejectionEvent.reason} `;
        else
            msg += `\r\n Reason=${JSON.stringify(promiseRejectionEvent.reason)} `;

        if (promiseRejectionEvent.reason.stack)
            msg += `\r\n Stack=${promiseRejectionEvent.reason.stack}`;

        if (promiseRejectionEvent.reason.status) {
            msg += `\r\n Status=${promiseRejectionEvent.reason.status}`;
        }
    }

    window.onerror.apply(this, [msg]);
});

window.onpossiblyunhandledexception = function () {
    window.onerror.apply(this, arguments);
}