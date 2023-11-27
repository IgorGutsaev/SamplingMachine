export async function getLanguagesAsync(currentLangs, newLangCodes) {
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

export async function loginAsync(phone, pin) {
    return await fetch('kiosk/login', {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json;charset=utf-8'
        },
        body: JSON.stringify({ phone, pin })
    }).then(r => r.json()
        .then((res) => {
            if (res.statusCode === 200) {
                return true;
            }
            if (res.statusCode === 401) {
                return false;
            }
        })
    );
}

export async function loginServiceAsync(pin) {
    return await fetch('kiosk/loginService', {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json;charset=utf-8'
        },
        body: JSON.stringify({ pin: pin })
    }).then(r => r.json()).then(r => {
        if (r.replenishmentUrl)
            return { url: r.replenishmentUrl };
        else return false;
    });
}

export async function commitTransactionAsync(phone, products) {
    return await fetch('kiosk/transaction', {
        method: 'PUT',
        headers: {
            'Content-Type': 'application/json;charset=utf-8'
        },
        body: JSON.stringify({
            phone,
            date: new Date(),
            items: products.map(p => {
                return {
                    product: { sku: p.sku },
                    count: p.count,
                    unitCredit: p.unitCredit
                }
            })
        })
    });
}

export async function dispenseAsync(products) {
    return await fetch('kiosk/dispense', {
        method: 'PUT',
        headers: {
            'Content-Type': 'application/json;charset=utf-8'
        },
        body: JSON.stringify(
            products.map(p => {
                return {
                    product: { sku: p.sku },
                    count: p.count,
                    unitCredit: p.unitCredit
                }
            })
        )
    });
}

export async function clearCache() {
    return await fetch('kiosk/cache/clear', {
        method: 'GET'
    });
}