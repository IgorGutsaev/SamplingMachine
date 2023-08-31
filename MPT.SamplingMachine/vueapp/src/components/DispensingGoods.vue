<template>
    <div class="post">
    </div>
</template>

<script lang="js">
    import { defineComponent } from 'vue';
    import $ from 'jquery'
    import KioskSettings from '/src/modules/settings.module.js'

    export default defineComponent({
        data() {
            return {
                loading: false
            };
        },
        components: {
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
            }
        }       
    });
</script>