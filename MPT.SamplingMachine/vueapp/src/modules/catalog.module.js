export default {
    props: {
        products: [],
    },
    bindProducts(kiosk) {
        this.products = kiosk.links.map((l) => {
            return {
                maxQty: l.maxQty,
                remains: l.remains,
                credit: l.credit,
                sku: l.product.sku,
                names: l.product.names,
                picture: l.product.picture ?? this.products.find(x => x.sku == l.product.sku).picture,
                disabled: l.disabled ?? false
            };
        });
    }
}