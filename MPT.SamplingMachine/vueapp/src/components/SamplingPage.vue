<template>
    <div class="post">
        <div v-if="loading" class="loading">
            Please wait...
        </div>

        <div v-if="languages" class="content">
            <i class="fa-regular fa-phone">gfdhgfh</i>
            <div id="globalCarousel" class="carousel slide carousel-fade">
                <div class="carousel-inner">
                    <div class="carousel-item active" id="home-screen">
                        <img src="https://www.petful.com/wp-content/uploads/2013/12/Abyssinian-1-750x398.jpg" class="d-block w-100">
                        <div class="btn btn-alt btn-lg btn-primary btn-filled mt-3 d-block w-50 mx-auto" v-on:click="setLanguage(lang)" v-for="lang in languages" :key="lang.code" role="button">{{lang.value}}</div>
                    </div>
                    <div class="carousel-item" id="agreement-screen">
                        <img src="https://icatcare.org/app/uploads/2018/07/Thinking-of-getting-a-cat.png" class="d-block w-100">
                        <Terms />
                    </div>
                    <div class="carousel-item" id="identification-screen">
                        <img src="https://images.hindustantimes.com/img/2022/08/07/550x309/cat_1659882617172_1659882628989_1659882628989.jpg" class="d-block w-100">
                        <Identification />
                    </div>
                    <div class="carousel-item" id="catalog-screen">
                        <ListOfProducts v-bind:currentLang="currentLang" />
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
    import Terms from './TermsOfService.vue'
    import ListOfProducts from './ListOfProducts.vue'
    import Identification from './CustomerIdentification.vue'
    import Dispensing from './DispensingGoods.vue'
    import EndSession from './EndSession.vue'

    export default defineComponent({
        data() {
            return {
                loading: false,
                languages: null,
                currentLang: false
            };
        },
        components: {
            Terms,
            Identification,
            ListOfProducts,
            Dispensing,
            EndSession
        },
        props: {
        },
        created() {
            // fetch the data when the view is created and the data is
            // already being observed
            this.fetchData();
            KioskSettings.credit = 3;
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
        },
        toDispensing() {
            $("#globalCarousel .carousel-item.active").removeClass("active");
            $("#dispensing-screen").addClass("active");
            console.info("Current screen is " + $("#globalCarousel .carousel-item.active").attr('id'));
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
        methods: {
            fetchData() {
                this.loading = true;

                fetch('settings/languages')
                    .then(r => r.json())
                    .then(json => {
                        this.languages = json;
                        this.loading = false;
                        return;
                    });
            },
            setLanguage(lang) {
                this.currentLang = lang.code;
                KioskSettings.currentLanguage = lang;
                console.info("Selected language: " + KioskSettings.currentLanguage.code);

                $("#globalCarousel .carousel-item.active").removeClass("active");
                $("#agreement-screen").addClass("active");
                console.info("Current screen is " + $("#globalCarousel .carousel-item.active").attr('id'));
            }
        }
    });
</script>