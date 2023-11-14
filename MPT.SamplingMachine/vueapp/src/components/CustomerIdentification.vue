<template>
    <form class="was-validated">

        <div class="container w-50">
            <div class="row">
                <div class="col" v-if="!smsRequested">
                    <p class="h3">{{$t('titles.phoneNumber')}}</p>
                    <div v-if="showPhoneWarning" class="alert alert-warning" role="alert">
                        {{$t('titles.checkPhoneWarning')}}
                    </div>
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
                    <label v-if="showPinWarning" class="alert alert-warning" role="alert" v-html="$t('titles.checkPinWarning')" />
                    <div>
                        <div class="input-group-lg">
                            <input id="pinInput" class="form-control is-invalid" pattern="^\d{4}$" :maxlength="4" :value="pin" @input="onPinInputChange" :placeholder="$t('titles.formatCode')" required />

                            <SimpleKeyboard @onChange="onPinChange" :input="pin" :maxLength="4" />
                            <div class="valid-feedback">
                                <div class="btn btn-alt btn-lg btn-primary btn-filled mt-3 d-block mx-auto" v-on:click="login" role="button">{{$t('buttons.confirm')}}</div>
                            </div>
                        </div>
                    </div>
                    <br />
                    <h5 v-if="countdown < 60 && countdown > 0">00:{{('0' + countdown).slice(-2)}}</h5>
                    <button v-if="countdown <= 0" class="btn btn-outline-primary btn-lg" v-on:click="() => { smsRequested = false; pin = ''; showPhoneWarning = true; }">{{$t('buttons.sendPinCode')}}</button>
                </div>
            </div>
        </div>
    </form>
</template>

<script lang="js">
    import { defineComponent } from 'vue';
    import { loginAsync } from '/src/modules/sync.module.js';
    import Transaction from '/src/modules/transaction.module.js'
    import Sampling from './SamplingPage.vue'
    import SimpleKeyboard from './SimpleKeyboard';

    export default defineComponent({
        data() {
            return {
                // loading: false
                smsRequested: false,
                phoneNumber: '1234567890',
                pin: '1234',
                showPhoneWarning: false,
                showPinWarning: false,
                countdown: 60
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
                this.countdown = 60;

                var self = this;
                let countdownTimer = setTimeout(function tick() {
                    countdownTimer = setTimeout(tick, 1000);
                    self.countdown--;
                    if (self.countdown <= 0) {
                        clearInterval(countdownTimer);
                        return;
                    }
                }, 1000);
            },
            async login() {
                let loginResult = await loginAsync(this.phoneNumber, this.pin);
                if (loginResult) {
                    Sampling.toCatalog();
                    Transaction.info = { phone: this.phoneNumber };
                }
                else {
                    this.countdown = 0;
                    this.pin = '';
                    this.showPinWarning = true;
                }
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