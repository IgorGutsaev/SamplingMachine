<template>
    <div class="post">
        <p class="h3 centered">{{$t('titles.goodBuy')}}</p>
    </div>
</template>

<script lang="js">
    import { defineComponent } from 'vue';
    //import Sampling from './SamplingPage.vue'

    export default defineComponent({
        data() {
            return {
                // loading: false
            };
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
                //this.loading = true;

                // fetch('settings/languages')
                //     .then(r => r.json())
                //     .then(json => {
                //         this.languages = json;
                //         this.loading = false;
                //         return;
                //     });
            }
        }
    });
</script>