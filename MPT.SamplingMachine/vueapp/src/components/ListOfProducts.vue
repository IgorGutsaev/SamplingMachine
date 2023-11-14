<template>
    <div v-if="loading" class="loading">
        Please wait...
    </div>

    <div v-if="!loading" class="content">
        <span v-if="this.credit > 1 && this.credit > this.creditUsed" class="badge text-bg-warning credit">{{$t('titles.credit')}}: {{this.credit - this.creditUsed}}</span>
        <span v-if="this.credit > 1 && this.credit === this.creditUsed" class="badge text-bg-danger credit">{{$t('titles.credit')}}: {{this.credit - this.creditUsed}}</span>

        <div id="productCarousel" class="carousel" data-ride="carousel">
            <div class="carousel-inner">
                <div v-for="(page, index) in productChunks" v-bind:key="index" class="carousel-item">

                    <div class="container-fluid" v-bind:style="{'margin-top': marginTop}">
                        <!-- container -->
                        <div class="row justify-content-md-center mt-4" v-bind:key="chunk" v-for="chunk in pageChunks(page)">
                            <div class="col col-lg-2" v-if="calcChunkSize(page) == 1" />
                            <div v-for="product in chunk" :key="product.sku" :class="calcChunkSize(page) == 1 ? 'col-8' : (calcChunkSize(page) == 2 ? 'col-6' : 'col-4')">
                                <div class="card" id="card">
                                    <img class="card-img-top rounded-top pic" :id="product.sku" v-bind:src="'data:image/*;base64,' + product.picture">
                                    <div v-if="product.count && product.maxQty <= product.count" class="outOfStock display-6">
                                        <label v-html="$t('titles.outOfStock')" />
                                    </div>
                                    <div v-if="products.length > 1 && product.maxQty > product.count && (product.credit > (this.credit - this.creditUsed))" class="lackOfCredit display-6">
                                        <label v-html="$t('titles.lackOfCredit')" />
                                    </div>

                                    <div class="card-body">
                                        <p class="product-title">{{ product.names.find(x => x.lang == currentLang)?.value ?? product.names[0]?.value }}</p>
                                        <!-- hide +/- buttons, out of stock and lack of credit label if there's a single product -->
                                        <button type="button" class="btn btn-dark btn-sm position-relative lmButton" v-if="product.count > 0" v-on:click="removeFromCart(product)">
                                            {{$t('buttons.remove')}}
                                            <span class="position-absolute top-0 start-100 translate-middle badge rounded-pill bg-info">
                                                {{product.count * product.credit}}
                                                <span class="visually-hidden">{{$t('titles.itemsCount')}}</span>
                                            </span>
                                        </button>

                                        <!-- <a href="#" class="btn btn-primary btn-sm lmButton" v-if="productAvailable(product)" v-on:click="addToCart(product)">{{(product.count > 0 ? $t('buttons.more'): $t('buttons.add'))}}</a> -->
                                        <button class="btn btn-primary btn-sm lmButton" v-if="productAvailable(product)" v-on:click="addToCart(product)">{{(product.count > 0 ? $t('buttons.more'): $t('buttons.add'))}}<span v-if="this.credit > 1" class="badge bg-warning" style="margin-left: 0.25em; color: black"> {{product.credit}}</span></button>
                                        <button disabled class="btn btn-secondary btn-sm lmButton" v-if="!productAvailable(product)">{{(product.count > 0 ? $t('buttons.more'): $t('buttons.add'))}}</button>
                                    </div>
                                </div>
                            </div>
                            <div class="col col-lg-2" v-if="calcChunkSize(page) == 1" />
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
                productChunks: [],
                marginTop: '0%'
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
        },
        created() {
            this.fetchData();
            ShoppingCart.items = [];
        },
        watch: {
            // call again the method if the route changes
            '$route': 'fetchData'
        },
        mounted() { 
            this.emitter.on('syncKiosk', async kiosk => {
                this.credit = kiosk.credit;
                this.products = CatalogModule.products.filter(x => x.credit <= this.credit && !x.disabled);

                let cart = this.products.filter(x => x.count > 0); // some products credit may have changed. We have to recalculate used credit
                let recalCreditUsed = 0;
                cart.forEach(x => recalCreditUsed += x.count * x.credit);
                this.creditUsed = recalCreditUsed;

                this.buildChunks();
            });
            this.emitter.on('syncProduct', async (product) => {
                let p = CatalogModule.products.find(x => x.sku == product.sku);
                p.credit = product.credit;
                this.products = CatalogModule.products.filter(x => x.credit <= this.credit);
                this.buildChunks();
            });
        },
        methods: {
            fetchData() {
                this.loading = true;

                let self = this;
                let check = function () {
                    setTimeout(function () {
                        if (CatalogModule.products === null)
                            check();
                        else {
                            self.products = CatalogModule.products.filter(x => x.credit <= KioskSettings.credit);
                            self.products.forEach(function (x) {
                                x.count = 0;
                            });

                            self.credit = KioskSettings.credit;
                            self.buildChunks();

                            self.loading = false;
                        }
                    }, 500);
                };

                check();
            },
            buildChunks() {
                let self = this;

                let chunkIndex = 0;
                self.productChunks = [];

                for (let i = 0; i < self.products.length; i += 9) { // max 9 products per page
                    self.productChunks[chunkIndex] = self.products.slice(i, i + 9);
                    chunkIndex++;
                }

                if (self.products.length == 1) // add product automatically if it's the only one in the list
                {
                    self.addToCart(self.products[0]);
                    self.marginTop = '30%';
                }
                else if (self.products.length == 2)
                    self.marginTop = '40%';
                else if (self.products.length == 3)
                    self.marginTop = '50%';
                else if (self.products.length == 4)
                    self.marginTop = '15%';
                else if (self.products.length == 5 || self.products.length == 6)
                    self.marginTop = '30%';
                else self.marginTop = '0';
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
                    ShoppingCart.items.push({ sku: product.sku, count: 0, unitCredit: product.credit });

                ShoppingCart.items.filter(x => x.sku == product.sku)[0].count++;
                this.creditUsed += product.credit;

                console.info("Cart changed: " + JSON.stringify(ShoppingCart.items));
            },
            productAvailable(product) {
                // show Add button if 1) stock is not empty 2) no limitation violation 3) credit is sufficient
                let isAvailable = product.remains > 0 && (product.maxQty > product.count || !product.count) && ((this.credit - this.creditUsed) >= product.credit);

                let pic = $(".pic#" + product.sku);
                if (isAvailable)
                    pic.removeClass("grayscale");
                else
                    pic.addClass("grayscale");

                return isAvailable;
            },
            async issueProducts() {
                this.$emit('homeButtonEnabled', false);
                Sampling.toDispensing();

                await Dispensing.extract(ShoppingCart.items);
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