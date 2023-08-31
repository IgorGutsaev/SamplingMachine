<template>
    <form class="was-validated">
        <div class="post" v-if="!smsRequested">
            <p class="h3">Your phone number:</p>
            <div class="mb-3">
                <input class="form-control is-invalid" id="validationTextarea" pattern="^\d{10}$" value="1234567890" placeholder="format: xxxxxxxxxx" required/>
                <div class="valid-feedback"><div class="btn btn-alt btn-lg btn-primary btn-filled mt-3 d-block w-50 mx-auto" v-on:click="sendSMS" role="button">Send SMS</div></div>
            </div>
        </div>

        <div class="post" v-if="smsRequested">
            <p class="h3">PIN code:</p>
            <div class="mb-3">
                <input class="form-control is-invalid" id="validationTextarea" pattern="^\d{4}$" value="1234" placeholder="format: xxxx" required/>
                <div class="valid-feedback"><div class="btn btn-alt btn-lg btn-primary btn-filled mt-3 d-block w-50 mx-auto" v-on:click="toCatalog" role="button">Confirm</div></div>
            </div>
        </div>
    </form>
</template>

<script lang="js">
    import { defineComponent } from 'vue';
    import $ from 'jquery'
    import Sampling from './SamplingPage.vue'

    export default defineComponent({
        data() {
            return {
                // loading: false
                smsRequested: false
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
            sendSMS() {
                // save number
                this.smsRequested = true;
            },
            toCatalog() {
                // check
                Sampling.toCatalog();
            }
        }
    });
</script>