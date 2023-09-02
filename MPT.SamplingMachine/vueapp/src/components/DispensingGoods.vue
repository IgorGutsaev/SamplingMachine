<template>
    <div class="post">
        <p class="h3">Take your products</p>
        <img src="../assets/arrow-down.gif" width="250" />
    </div>
</template>

<script lang="js">
    import { defineComponent } from 'vue';
    import $ from 'jquery'
    import KioskSettings from '/src/modules/settings.module.js'
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
        extract() {
            if (KioskSettings.isEmulation) {
                let promise = new Promise((resolve, reject) => {
                    setTimeout(() => Sampling.toExit(), 5000)
                });
            }
        }
    });
</script>