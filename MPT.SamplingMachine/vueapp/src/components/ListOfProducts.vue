<template>

        <div v-if="loading" class="loading">
            Please wait...
        </div>
        
        <div v-if="products" class="content">
            <div v-for="product in products" :key="product.sku" class="card" style="width: 18rem;">
                <img class="card-img-top" v-bind:src="'data:image/*;base64,' + product.picture">
                <div class="card-body">
                    <h5 class="card-title">{{product.names[currentLang]}}</h5>
                    <a href="#" class="btn btn-primary">Select</a>
                </div>
            </div>

            <div class="btn btn-alt btn-lg btn-primary btn-filled mt-3 d-block w-50 mx-auto" v-on:click="" role="button">Receive</div> 
        </div>

</template>

<script lang="js">
    import { defineComponent } from 'vue';
    import $ from 'jquery'
    import CatalogModule from '/src/modules/catalog.module.js'
    import KioskSettings from '/src/modules/settings.module.js'

    export default defineComponent({
        data() {
            return {
                loading: false,
                products: null
            };
        },
        props: {
            currentLang: {
                type: String,
                required: true
            },
        },
        components: {
            CatalogModule,
            KioskSettings
        },
        created() {
            this.fetchData();
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
                        //$.map(this.products, function (x) { return { name: x.names.find(x => x.lang == KioskSettings.currentLanguage.code).value, sku: x.sku } });
                        console.info("Total products available: " + CatalogModule.products.length);
                        this.loading = false;
                        return;
                    });
            }
        }
    });
</script>