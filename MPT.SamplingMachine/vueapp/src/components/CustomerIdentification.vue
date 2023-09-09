<template>
    <form class="was-validated">

        <div class="container w-50">
            <div class="row">
                <div class="col" v-if="!smsRequested">
                    <p class="h3">{{$t('titles.phoneNumber')}}</p>
                    <div>
                        <div class="input-group-lg">
                            <input id="mobileInput" class="input form-control is-invalid" pattern="^\d{10}$" :maxlength="10" :value="phoneNumber" @input="onPhoneInputChange" :placeholder="$t('titles.formatNumber')" required />

                            <SimpleKeyboard @onChange="onPhoneChange" :input="phoneNumber" :maxLength="10" />
                            <div class="valid-feedback">
                                <div class="btn btn-alt btn-lg btn-primary btn-filled mt-3 d-block mx-auto" v-on:click="sendSMS" role="button">{{$t('buttons.sendSMS')}}</div>
                            </div>
                        </div>
                    </div>
                </div>

                <div class="col" v-if="smsRequested">
                    <p class="h3">{{$t('titles.pinCode')}}</p>
                    <div>
                        <div class="input-group-lg">
                            <input id="pinInput" class="form-control is-invalid" pattern="^\d{4}$" :maxlength="4" :value="pin" @input="onPinInputChange" :placeholder="$t('titles.formatCode')" required />

                            <SimpleKeyboard @onChange="onPinChange" :input="pin" :maxLength="4" />
                            <div class="valid-feedback">
                                <div class="btn btn-alt btn-lg btn-primary btn-filled mt-3 d-block mx-auto" v-on:click="toCatalog" role="button">{{$t('buttons.confirm')}}</div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </form>
</template>

<script lang="js">
    import { defineComponent } from 'vue';
    import $ from 'jquery'
    import Sampling from './SamplingPage.vue'
    import SimpleKeyboard from './SimpleKeyboard';

    export default defineComponent({
        data() {
            return {
                // loading: false
                smsRequested: false,
                phoneNumber: '',
                pin: ''
            };
        },
        components:{
            SimpleKeyboard
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
            },
            sendSMS() {
                // save number
                this.smsRequested = true;
            },
            toCatalog() {
                // check
                Sampling.toCatalog();
            },
            onPhoneChange(input) {
              this.phoneNumber = input;
            },
            onPhoneInputChange(input) {
              this.phoneNumber = input.target.value;
            },
            onPinChange(input) {
                this.pin = input;
            },
            onPinInputChange(input) {
              this.pin = input.target.value;
            }
        }
    });
</script>