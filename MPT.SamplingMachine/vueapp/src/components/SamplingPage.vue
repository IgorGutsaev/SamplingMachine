<template>
    <div class="post">
        <div v-if="loading" class="loading" id="mainSpinner">
            <font-awesome-icon :icon="['fas', 'spinner']" size="3x" spin />
        </div>

        <div v-if="languages" class="content">
            <div class="container-fluid">
                <div class="row mb-1">
                    <div class="col-lg-12 d-flex justify-content-between">
                        <img alt="OgmentO" src="../assets/logo.png" width="24" />
                        <div v-if="homeButtonEnabled"><font-awesome-icon icon="house" style="color: darkgrey" size="xl" v-on:click="goHome" /></div>
                    </div>
                </div>
            </div>

            <div id="globalCarousel" class="carousel slide carousel-fade">
                <div class="carousel-inner">
                    <div class="carousel-item active" id="home-screen">
                        <img src="https://www.petful.com/wp-content/uploads/2013/12/Abyssinian-1-750x398.jpg" class="d-block w-100">
                        <div class="btn btn-alt btn-lg btn-primary btn-filled mt-3 d-block w-50 mx-auto" v-on:click="setLanguage(lang)" v-for="lang in languages" :key="lang.code" role="button">{{lang.value}}</div>
                    </div>
                    <div class="carousel-item" id="agreement-screen">
                        <img src="https://icatcare.org/app/uploads/2018/07/Thinking-of-getting-a-cat.png" class="d-block w-100">
                        <br>
                        <Terms />
                    </div>
                    <div class="carousel-item" id="identification-screen">
                        <img src="https://images.hindustantimes.com/img/2022/08/07/550x309/cat_1659882617172_1659882628989_1659882628989.jpg" class="d-block w-100">
                        <br>
                        <Identification />
                    </div>
                    <div class="carousel-item" id="catalog-screen">
                        <ListOfProducts v-bind:currentLang="currentLang" @homeButtonEnabled="changeHomeButton" />
                    </div>
                    <div class="carousel-item" id="dispensing-screen">
                        <Dispensing />
                    </div>
                    <div class="carousel-item" id="exit-screen">
                        <EndSession />
                    </div>
                </div>
            </div>
        </div>
    </div>
</template>

<script lang="js">
    import { defineComponent } from 'vue';
    import $ from 'jquery'
    import KioskSettings from '/src/modules/settings.module.js'
    import CatalogModule from '/src/modules/catalog.module.js'
    import { getLanguages, getSecTimeoutFromTimespan } from '/src/modules/sync.module.js';
    import Terms from './TermsOfService.vue'
    import ListOfProducts from './ListOfProducts.vue'
    import Identification from './CustomerIdentification.vue'
    import Dispensing from './DispensingGoods.vue'
    import EndSession from './EndSession.vue'
    import i18n from '../i18n'
    
    export default defineComponent({
        data() {
            return {
                loading: false,
                languages: null,
                currentLang: false,
                homeButtonEnabled: false
            };
        },
        components: {
            CatalogModule,
            Terms,
            Identification,
            ListOfProducts,
            Dispensing,
            EndSession,
            KioskSettings
        },
        props: {
        },
        created() {
            // fetch the data when the view is created and the data is
            // already being observed
            this.fetchData();
            
            KioskSettings.canLogOff = false;
            KioskSettings.isEmulation = true;
        },
        watch: {
            // call again the method if the route changes
            '$route': 'fetchData'
        },
        toIdentification() {
            $("#globalCarousel .carousel-item.active").removeClass("active");
            $("#identification-screen").addClass("active");
            console.info("Current screen is " + $("#globalCarousel .carousel-item.active").attr('id'));
        },
        toCatalog() {
            $("#globalCarousel .carousel-item.active").removeClass("active");
            $("#catalog-screen").addClass("active");
            console.info("Current screen is " + $("#globalCarousel .carousel-item.active").attr('id'));
        $($("#productCarousel .carousel-item")[0]).addClass("active");
        },
        toExit() {
            $("#globalCarousel .carousel-item.active").removeClass("active");
            $("#exit-screen").addClass("active");
            console.info("Current screen is " + $("#globalCarousel .carousel-item.active").attr('id'));

            let promise = new Promise((resolve, reject) => {
                setTimeout(() => {
                    location.reload();
                }, 2000)
            });
        },
        toDispensing() {
            $("#globalCarousel .carousel-item.active").removeClass("active");
            $("#dispensing-screen").addClass("active");
            console.info("Current screen is " + $("#globalCarousel .carousel-item.active").attr('id'));
        },
        mounted() { 
            this.emitter.on('sync', async kiosk => {
                this.languages = await getLanguages(this.languages, kiosk.languages);
                KioskSettings.idleTimeoutSec = getSecTimeoutFromTimespan(kiosk.idleTimeout) / 10;
                KioskSettings.credit = kiosk.credit;
                ListOfProducts.credit = KioskSettings.credit;
            });
        },
        methods: {
            async fetchData() {
                this.loading = true;

                let kiosk;
                await fetch('kiosk')
                    .then(r => r.json())
                    .then(x => {
                        kiosk = x;
                        // set products
                        CatalogModule.products = kiosk.links.map((l) => {
                            return {
                                maxQty: l.maxQty,
                                remains: l.remains,
                                credit: l.credit,
                                sku: l.product.sku,
                                names: l.product.names,
                                picture: l.product.picture };
                        });

                        return;
                    });

                // get localized list of languages
                this.languages = await getLanguages(null, kiosk.languages);
                KioskSettings.idleTimeoutSec = getSecTimeoutFromTimespan(kiosk.idleTimeout);
                KioskSettings.credit = kiosk.credit;

                this.loading = false;
            },
            setLanguage(lang) {
                this.currentLang = lang.code;
                KioskSettings.currentLanguage = lang;
                console.info("Selected language: " + KioskSettings.currentLanguage.code);
                i18n.global.locale = KioskSettings.currentLanguage.code;

                $("#globalCarousel .carousel-item.active").removeClass("active");
                $("#agreement-screen").addClass("active");
                console.info("Current screen is " + $("#globalCarousel .carousel-item.active").attr('id'));
                this.changeHomeButton(true);
            },
            goHome() {
                location.reload();
            },
            changeHomeButton(enabled) {
                this.homeButtonEnabled = enabled;
                KioskSettings.canLogOff = enabled;
            },
            syncKiosk(revision)
            {
                KioskSettings.credit = revision.credit;
            }
        }
    });
</script>