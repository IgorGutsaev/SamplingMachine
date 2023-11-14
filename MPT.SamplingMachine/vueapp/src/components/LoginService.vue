<template>
    <div class="btn btn-alt btn-lg btn-dark btn-filled mt-4 mb-4 col-6" v-on:click="goToUI" role="button">{{$t('buttons.goBackToUI')}}</div>
    <form class="was-validated">
        <div class="col">
            <p class="h1">{{$t('titles.serviceLogin')}}</p>
            <label v-if="showPinWarning" class="alert alert-warning" role="alert" v-html="$t('titles.checkServicePinWarning')" />
            <div>
                <div class="input-group-lg">
                    <input id="pinInput" class="form-control is-invalid" pattern="^\d{4}$" :maxlength="4" :value="pin" @input="onPinInputChange" :placeholder="$t('titles.formatCode')" required />

                    <SimpleKeyboard @onChange="onPinChange" :input="pin" :maxLength="4" />
                    <div class="valid-feedback">
                        <div class="btn btn-alt btn-lg btn-primary btn-filled mt-4 mb-4 col-6" v-on:click="loginAsync" role="button">{{$t('buttons.confirm')}}</div>
                    </div>
                </div>
            </div>
            <br />
            <h5 v-if="countdown < 60 && countdown > 0">00:{{('0' + countdown).slice(-2)}}</h5>
        </div>
    </form>
</template>

<script lang="js">
    import { defineComponent } from 'vue';
    import { loginServiceAsync } from '/src/modules/sync.module.js';
    import SimpleKeyboard from './SimpleKeyboard';

    export default defineComponent({
        name: 'LoginService',
        data() {
            return {
                pin: null,
                countdown: 60,
                showPinWarning: false
            };
        },
        components:{
            SimpleKeyboard
        },
        created() {
            this.init();
        },
        methods: {
            init() {
                 this.countdown = 60;

                var self = this;
                let countdownTimer = setTimeout(function tick() {
                    countdownTimer = setTimeout(tick, 1000);
                    self.countdown--;
                    if (self.countdown <= 0) {
                        self.goToUI();
                        clearInterval(countdownTimer);
                        return;
                    }
                }, 1000);
            },
            async loginAsync() {
                let loginResult = await loginServiceAsync(this.pin);
                if (loginResult.url) {
                    window.location.href = loginResult.url + '/true';
                }
                else {
                    this.countdown = 60;
                    this.pin = '';
                    this.showPinWarning = true;
                }
            },
            onPinChange(input) {
                this.pin = input;
            },
            onPinInputChange(input) {
                this.pin = input.target.value;
            },
            goToUI() {
                window.location.href = location.origin;
            }
        }
    });
</script>