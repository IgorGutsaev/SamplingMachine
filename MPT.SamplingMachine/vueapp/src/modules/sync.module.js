export async function getLanguages(currentLangs, newLangCodes) {
    let forceUpdate = false;

    if (currentLangs && currentLangs.length == newLangCodes.length) {
        for (let i = 0; i < currentLangs.length; i++) {
            if (currentLangs[i].code !== newLangCodes[i])
                forceUpdate = true;
            break;
        }
    }
    else forceUpdate = true;

    if (forceUpdate) {
        return await fetch('settings/languages', {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json;charset=utf-8'
            },
            body: JSON.stringify(newLangCodes)
        })
        .then(r => r.json());
    }
    else return currentLangs;
}

export function getSecTimeoutFromTimespan(timespan) {
    let a = timespan.split(':'); // split it at the colons
    // minutes are worth 60 seconds. Hours are worth 60 minutes.
    return (+a[0]) * 60 * 60 + (+a[1]) * 60 + (+a[2]);
}