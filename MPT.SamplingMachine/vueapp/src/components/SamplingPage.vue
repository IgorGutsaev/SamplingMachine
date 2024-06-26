﻿<template>
    <div class="post">
        <div v-if="loading" class="loading" id="mainSpinner">
            <font-awesome-icon :icon="['fas', 'spinner']" size="3x" spin />
        </div>

        <h1 id="oosBanner" v-if="!loading && !isOn">
            {{$t('titles.oos')}}
        </h1>

        <div v-if="!loading && isOn" class="content">
            <div class="container-fluid">
                <div class="row mb-1">
                    <div class="col-lg-12 d-flex justify-content-between">
                        <img alt="OgmentO" src="../assets/logo.png" width="24" v-on:dblclick="goToDashboard" />
                        <div v-if="homeButtonEnabled"><font-awesome-icon icon="house" style="color: darkgrey" size="xl" v-on:click="goHome" /></div>
                    </div>
                </div>
            </div>

            <div id="globalCarousel" class="carousel slide carousel-fade">
                <div class="carousel-inner">
                    <div class="carousel-item active" id="home-screen">
                        <AdvertisementManager @homeButtonEnabled="changeHomeButton" />
                    </div>
                    <div class="carousel-item" id="languages-screen">
                        <img src="https://www.petful.com/wp-content/uploads/2013/12/Abyssinian-1-750x398.jpg" class="d-block w-100">
                        <div class="btn btn-alt btn-lg btn-primary btn-filled mt-3 d-block w-50 mx-auto" v-on:click="setLanguage(lang, true)" v-for="lang in languages" :key="lang.code" role="button">{{languages.length <= 1 ? $t('buttons.start') : lang.value}}</div>
                    </div>
                    <div class="carousel-item" id="agreement-screen">
                        <img src="https://icatcare.org/app/uploads/2018/07/Thinking-of-getting-a-cat.png" class="d-block w-100">
                        <br>
                        <Terms />
                    </div>
                    <div class="carousel-item" id="identification-screen">
                        <img src="https://www.m24.ru/b/d/nBkSUhL2hFYikMm-Lr6BrNOp2Z3z8Zj21iDEh_fH_nKUPXuaDyXTjHou4MVO6BCVoZKf9GqVe5Q_CPawk214LyWK9G1N5ho=mdhxAlb3poduu5yzsYPDtQ.jpg" class="d-block w-100">
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
                        <EndTransaction />
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
    import { getLanguagesAsync, clearCache } from '/src/modules/sync.module.js';
    import { getSecTimeoutFromTimespan } from '/src/modules/helpers.module.js';
    import AdvertisementManager from './AdvertisementManager.vue'
    import Terms from './TermsOfService.vue'
    import ListOfProducts from './ListOfProducts.vue'
    import Identification from './CustomerIdentification.vue'
    import Dispensing from './DispensingGoods.vue'
    import EndTransaction from './EndTransaction.vue'
    import i18n from '../i18n'
    import { } from '/src/http-logger.js'

    export default defineComponent({
        data() {
            return {
                loading: true,
                languages: null,
                currentLang: null,
                homeButtonEnabled: true,
                isOn: true
            };
        },
        components: {
            AdvertisementManager,
            Terms,
            Identification,
            ListOfProducts,
            Dispensing,
            EndTransaction,
        },
        props: {
        },
        created() {
            // fetch the data when the view is created and the data is
            // already being observed
            this.fetchData();

            KioskSettings.languages = [];
            KioskSettings.canLogOff = false;
            KioskSettings.isEmulation = false;
        },
        watch: {
            // call again the method if the route changes
            '$route': 'fetchData'
        },
        toAgreement() {
            $("#globalCarousel .carousel-item.active").removeClass("active");
            $("#agreement-screen").addClass("active");
            console.log("Current screen is " + $("#globalCarousel .carousel-item.active").attr('id'));
        },
        toIdentification() {
            $("#globalCarousel .carousel-item.active").removeClass("active");
            $("#identification-screen").addClass("active");
            console.log("Current screen is " + $("#globalCarousel .carousel-item.active").attr('id'));
        },
        toCatalog() {
            $("#globalCarousel .carousel-item.active").removeClass("active");
            $("#catalog-screen").addClass("active");
            console.log("Current screen is " + $("#globalCarousel .carousel-item.active").attr('id'));
            $($("#productCarousel .carousel-item")[0]).addClass("active");
        },
        toExit() {
            $("#globalCarousel .carousel-item.active").removeClass("active");
            $("#exit-screen").addClass("active");
            console.log("Current screen is " + $("#globalCarousel .carousel-item.active").attr('id'));

            new Promise(() => {
                setTimeout(() => {
                    location.reload();
                }, 2000)
            });
        },
        toDispensing() {
            $("#globalCarousel .carousel-item.active").removeClass("active");
            $("#dispensing-screen").addClass("active");
            console.log("Current screen is " + $("#globalCarousel .carousel-item.active").attr('id'));
        },
        toLanguages() {
            if (KioskSettings.languages.length <= 1) {
                this.toAgreement();
            }
            else {
                $("#globalCarousel .carousel-item.active").removeClass("active");
                $("#languages-screen").addClass("active");
                console.log("Current screen is " + $("#globalCarousel .carousel-item.active").attr('id'));
            }
        },
        mounted() {
            this.emitter.on('syncKiosk', async kiosk => {
                clearCache();
                KioskSettings.languages = await getLanguagesAsync(KioskSettings.languages, kiosk.languages);
                this.languages = KioskSettings.languages;
                KioskSettings.isOn = this.isOn = kiosk.isOn;
                KioskSettings.idleTimeoutSec = getSecTimeoutFromTimespan(kiosk.idleTimeout);
                KioskSettings.credit = kiosk.credit;
                KioskSettings.media = kiosk.media;
                ListOfProducts.credit = KioskSettings.credit;

                if (KioskSettings.languages.length == 1)
                    this.setLanguage(KioskSettings.languages[0], false);
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
                        CatalogModule.bindProducts(kiosk);

                        return;
                    });

                // get localized list of languages
                KioskSettings.languages = await getLanguagesAsync(null, kiosk.languages);
                this.languages = KioskSettings.languages;
                this.currentLang = KioskSettings.languages[0].code;
                KioskSettings.isOn = this.isOn = kiosk.isOn;
                KioskSettings.idleTimeoutSec = getSecTimeoutFromTimespan(kiosk.idleTimeout);
                KioskSettings.credit = kiosk.credit;
                KioskSettings.media = kiosk.media;

                this.loading = false;
            },
            setLanguage(lang, goNextScreen) {
                this.currentLang = lang.code;
                KioskSettings.currentLanguage = lang;
                console.log("Selected language: " + KioskSettings.currentLanguage.code);
                i18n.global.locale = KioskSettings.currentLanguage.code;

                if (goNextScreen)
                {
                    $("#globalCarousel .carousel-item.active").removeClass("active");
                    $("#agreement-screen").addClass("active");
                    console.log("Current screen is " + $("#globalCarousel .carousel-item.active").attr('id'));
                    this.changeHomeButton(true);
                }
            },
            goHome() {
                location.reload();
            },
            changeHomeButton(enabled) {
                this.homeButtonEnabled = enabled;
                KioskSettings.canLogOff = enabled;
            },
            syncKiosk(revision) {
                KioskSettings.credit = revision.credit;
            },
            goToDashboard() {
                this.$router.push('/login')
            }
        }
    });
</script>