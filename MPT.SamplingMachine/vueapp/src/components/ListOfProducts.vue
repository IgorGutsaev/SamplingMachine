<template>
        <div v-if="loading" class="loading">
            Please wait...
        </div>

        <div v-if="products" class="content">
            <span v-if="this.credit > 1 && this.credit > this.creditUsed && this.products.length > 1" class="badge text-bg-warning credit">{{$t('titles.credit')}}: {{this.credit - this.creditUsed}}</span>
            <span v-if="this.credit > 1 && this.credit === this.creditUsed && this.products.length > 1" class="badge text-bg-danger credit">{{$t('titles.credit')}}: {{this.credit - this.creditUsed}}</span>

            <div id="productCarousel" class="carousel" data-ride="carousel">
                <div class="carousel-inner">
                    <div v-for="(page, index) in productChunks" v-bind:key="index" class="carousel-item">

                            <div class="container-fluid">
                                <div class="row mt-4" v-for="chunk in pageChunks(page)">
                                    <div v-for="product in chunk" :key="product.sku" :class="calcChunkSize(page) == 1 ? 'col-12' : (calcChunkSize(page) == 2 ? 'col-6' : 'col-4')">
                                        <div class="card">
                                            <img class="card-img-top rounded-top pic" :id="product.sku" v-bind:src="'data:image/*;base64,' + product.picture">
                                            <div class="card-body">
                                                <p class="product-title">{{product.names[currentLang]}}</p>
                                                <!-- hide +/- buttons, out of stock and lack of credit label if there's a single product -->
                                                <button type="button" class="btn btn-primary btn-sm position-relative lmButton" v-if="product.count > 0 && products.length > 1" v-on:click="removeFromCart(product)">
                                                    {{$t('buttons.remove')}}
                                                    <span class="position-absolute top-0 start-100 translate-middle badge rounded-pill bg-info">
                                                        {{product.count}}
                                                        <span class="visually-hidden">{{$t('titles.itemsCount')}}</span>
                                                    </span>
                                                </button>

                                                <a href="#" class="btn btn-primary btn-sm lmButton" v-if="productAvailable(product) && products.length > 1" v-on:click="addToCart(product)">{{(product.count > 0 ? $t('buttons.more'): $t('buttons.add'))}}</a>

                                                <p class="text-secondary" v-if="product.totalCount == 0 && products.length > 1">{{$t('titles.outOfStock')}}</p>
                                                <div class="text-danger" v-if="product.count == 0 && product.credit > (this.credit - this.creditUsed) && products.length > 1">{{$t('titles.lackOfCredit')}}</div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                    </div>
                </div>
                <button v-if="products.length > 9" class="carousel-control-prev prod-carousel-button" type="button" data-bs-target="#productCarousel" data-bs-slide="prev">
                    <span class="carousel-control-prev-icon" aria-hidden="true"></span>
                    <span class="visually-hidden">Previous</span>
                </button>

                <div v-if="this.creditUsed > 0" class="btn btn-alt btn-lg btn-primary btn-filled mt-3 d-block w-50 mx-auto dispense-button" v-on:click="issueProducts" role="button">{{$t('buttons.dispense')}}</div>
                <button v-if="this.creditUsed == 0" type="button" class="btn btn-alt btn-lg btn-secondary btn-filled mt-3 d-block w-50 mx-auto dispense-button" disabled>{{$t('buttons.dispense')}}</button>

                <button v-if="products.length > 9" class="carousel-control-next prod-carousel-button" type="button" data-bs-target="#productCarousel" data-bs-slide="next">
                    <span class="carousel-control-next-icon" aria-hidden="true"></span>
                    <span class="visually-hidden">Next</span>
                </button>
            </div>
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
                credit: null,
                productChunks: []
            };
        },
        emits: ["homeButtonEnabled"],
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
                        this.products = CatalogModule.products.filter(x => x.credit <= KioskSettings.credit);
                        this.products.forEach(function (x) {
                            x.count = 0;
                        });

                        this.credit = KioskSettings.credit;
                        
                        let chunkIndex = 0;
                        for (let i = 0; i < this.products.length; i += 9) { // max 9 products per page
                            this.productChunks[chunkIndex] = this.products.slice(i, i + 9);
                            chunkIndex++;
                        }

                        //$.map(this.products, function (x) { return { name: x.names.find(x => x.lang == KioskSettings.currentLanguage.code).value, sku: x.sku } });
                        console.info("Total products available: " + CatalogModule.products.length);

                        if (this.products.length == 1) // add product automatically if it's the only one in the list
                            this.addToCart(this.products[0]);
                        this.loading = false;
                        return;
                    });
            },
            removeFromCart(product) {
                if (product.count <= 0)
                    return;

                while (product.count > 0) {
                    product.count--;
                    
                    // remove from shopping cart
                    if (ShoppingCart.items.filter(x => x.sku == product.sku).length > 0) {
                        let cartItem = ShoppingCart.items.filter(x => x.sku == product.sku)[0];
                        cartItem.count--;

                        if (cartItem.count == 0)
                            ShoppingCart.items = ShoppingCart.items.filter(item => item.sku !== cartItem.sku);
                    }

                    // release credit
                    this.creditUsed -= product.credit;
                }

                console.info(ShoppingCart.items.length == 0 ? "Cart is empty" : "Cart changed: " + JSON.stringify(ShoppingCart.items));
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
            },
            calcChunkSize(chunk) {
                return this.products.length > 9 ? 3 : (chunk.length == 2 || chunk.length == 4 ? 2 : (chunk.length == 1 ? 1 : 3));
            },
            pageChunks(page) {
                let result = [];
                let index = 0;

                let chunkSize = this.calcChunkSize(page);
                
                for (let i = 0; i < page.length; i += chunkSize) {
                    result[index] = page.slice(i, i + chunkSize);
                    index++;
                }

                return result;
            }
        }
    });
</script>