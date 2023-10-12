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
};

export async function loginAsync(phone, pin) {
    return await fetch('kiosks/login', {
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
};

export async function commitSessionAsync(phone, products) {
    return await fetch('kiosks/session', {
        method: 'PUT',
        headers: {
            'Content-Type': 'application/json;charset=utf-8'
        },
        body: JSON.stringify({
            phone,
            items: products.map(p => {
                return {
                    product: { sku: p.sku },
                    count: p.count,
                    unitCredit: p.unitCredit
                }
            })
        })
    });
};

export async function clearCache() {
    return await fetch('kiosks/cache/clear', {
        method: 'GET'
    });
};