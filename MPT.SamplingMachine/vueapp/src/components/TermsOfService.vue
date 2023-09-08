<template>
    <div class="post">
        <p class="h3">{{ $t('titles.terms') }}</p>
        <p class="h4">Service provider</p>
        <p><small>Ogmento services are provided by, and you're contracting with:</small></p>
        <p><small>Ogmento LLC</small></p>
        <p><small>organized under the laws of the State of Delaware, USA, and operating under the laws of the USA</small></p>
        <p><small>1600 Amphitheatre Parkway</small></p>
        <p><small>Mountain View, California 94043</small></p>
        <p><small>USA</small></p>
        <p class="h4">Age requirements</p>
        <p><small>If you're under the age required to manage your own Google Account, you must have your parent or legal guardian's permission to use a Google Account. Please have your parent or legal guardian read these terms with you.</small></p>

        <div class="form-check">
            <input class="form-check-input" type="checkbox" value="" id="flexCheckDefault" v-model="accepted">
            <label class="form-check-label" for="flexCheckDefault">
                {{ $t('titles.termsConfirmation') }}
            </label>
        </div>

        <div class="btn btn-alt btn-lg btn-primary btn-filled mt-3 d-block w-50 mx-auto" :class="{ 'disabled': !this.accepted }" v-on:click="toIdentification" role="button">{{$t('buttons.next')}}</div>
    </div>
</template>

<script lang="js">
    import { defineComponent } from 'vue';
    import $ from 'jquery'
    import Sampling from './SamplingPage.vue'
//    import { RouterLink } from 'vue-router'

    export default defineComponent({
        data() {
            return {
                // loading: false
                accepted: false
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
            },
            toIdentification() {
                Sampling.toIdentification();
            }
        }
    });
</script>