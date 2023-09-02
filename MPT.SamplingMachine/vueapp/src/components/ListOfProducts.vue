<template>
    <div v-if="loading" class="loading">
        Please wait...
    </div>

    <div v-if="products" class="content">
        <span v-if="this.credit > 1 && this.credit > this.creditUsed" class="badge text-bg-warning">Credit: {{this.credit - this.creditUsed}}</span>
        <span v-if="this.credit > 1 && this.credit === this.creditUsed" class="badge text-bg-danger">Credit: {{this.credit - this.creditUsed}}</span>
        <!-- <div v-for="product in products.filter(x => x.totalCount > 0)"... -->
        <div v-for="product in products" :key="product.sku" class="card" style="width: 18rem;">
            <img class="card-img-top rounded-top pic" :id="product.sku" v-bind:src="'data:image/*;base64,' + product.picture">
            <div class="card-body">
                <h5 class="card-title">{{product.names[currentLang]}}</h5>
                <a href="#" class="btn btn-primary" v-if="product.count > 0" v-on:click="removeFromCart(product)">Remove</a>
                <a href="#" class="btn btn-primary" v-if="productAvailable(product)" v-on:click="addToCart(product)">Add</a>
                <p class="text-secondary" v-if="product.totalCount == 0">Out of stock</p>
            </div>
        </div>

        <div class="btn btn-alt btn-lg btn-primary btn-filled mt-3 d-block w-50 mx-auto" v-if="this.creditUsed > 0" v-on:click="issueProducts" role="button">Issue products</div>
    </div>
</template>

<script lang="js">
    import { defineComponent } from 'vue';
    import $ from 'jquery'
    import CatalogModule from '/src/modules/catalog.module.js'
    import KioskSettings from '/src/modules/settings.module.js'
    import ShoppingCart from '/src/modules/cart.module.js'
    import Sampling from './SamplingPage.vue'
    import Dispensing from './DispensingGoods.vue'

    export default defineComponent({
        data() {
            return {
                loading: false,
                products: null,
                creditUsed: 0,
                credit: null
            };
        },
        props: {
            currentLang: {
                type: String,
                required: true
            }
        },
        components: {
            CatalogModule,
            KioskSettings
        },
        created() {
            this.fetchData();
            ShoppingCart.items = [];
        },
        watch: {
            // call again the method if the route changes
            '$route': 'fetchData'
        },
        methods: {
            fetchData() {
                this.loading = true;
                fetch('catalog')
                    .then(r => r.json())
                    .then(json => {
                        CatalogModule.products = json;
                        this.products = CatalogModule.products;
                        this.products.forEach(function (x) {
                            x.count = 0;
                        });
                        this.credit = KioskSettings.credit;
                        //$.map(this.products, function (x) { return { name: x.names.find(x => x.lang == KioskSettings.currentLanguage.code).value, sku: x.sku } });
                        console.info("Total products available: " + CatalogModule.products.length);
                        this.loading = false;
                        return;
                    });
            },
            removeFromCart(product) {
                if (product.count <= 0)
                    return;

                product.count--;
                this.creditUsed -= product.credit;

                console.info("Cart changed: " + JSON.stringify(ShoppingCart.items));
            },
            addToCart(product) {
                product.count++;

                if (ShoppingCart.items.filter(x => x.sku == product.sku).length == 0)
                    ShoppingCart.items.push({ sku: product.sku, count: 0 });

                ShoppingCart.items.filter(x => x.sku == product.sku)[0].count++;
                this.creditUsed += product.credit;

                console.info("Cart changed: " + JSON.stringify(ShoppingCart.items));
            },
            productAvailable(product) {
                // show Add button if 1) stock is not empty 2) no limitation violation 3) credit is sufficient
                let isAvailable = product.totalCount > 0 && (product.maxCountPerSession > product.count || !product.count) && ((this.credit - this.creditUsed) >= product.credit);

                let pic = $(".pic#" + product.sku);
                if (isAvailable)
                    pic.removeClass("grayscale");
                else pic.addClass("grayscale");

                return isAvailable;
            },
            issueProducts() {
                this.$emit('homeButtonEnabled', false)
                Sampling.toDispensing();
                Dispensing.extract();
            }
        }
    });
</script>