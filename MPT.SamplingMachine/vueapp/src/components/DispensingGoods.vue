<template>
    <div class="post">
        <p class="h3">{{$t('titles.takeYourProducts')}}</p>
        <img id="arrow" src="../assets/arrow-down.gif" />
    </div>
</template>

<script lang="js">
    import { defineComponent } from 'vue';
    import $ from 'jquery'
    import KioskSettings from '/src/modules/settings.module.js'
    import { commitSessionAsync } from '/src/modules/sync.module.js';
    import Session from '/src/modules/session.module.js'
    import Sampling from './SamplingPage.vue'

    export default defineComponent({
        data() {
            return {
                loading: false
            };
        },
        components: {
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
                this.loading = false;
            }
        },
        async extract(products) {
            if (KioskSettings.isEmulation) {
                let promise = new Promise((resolve, reject) => {
                    setTimeout(() => Sampling.toExit(), 5000)
                });
            }

           await commitSessionAsync(Session.info.phone, products);
        }
    });
</script>