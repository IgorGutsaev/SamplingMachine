<template>
    <div class="col">
        <p class="h3">{{$t('titles.pinCode')}}</p>
        <div>
            <div class="input-group-lg">
                <input id="pinInput" class="form-control is-invalid" pattern="^\d{4}$" :maxlength="4" :value="pin" @input="onPinInputChange" :placeholder="$t('titles.formatCode')" required />

                <SimpleKeyboard @onChange="onPinChange" :input="pin" :maxLength="4" />
                <div class="valid-feedback">
                    <div class="btn btn-alt btn-lg btn-primary btn-filled mt-3 d-block mx-auto" v-on:click="loginAsync" role="button">{{$t('buttons.confirm')}}</div>
                </div>
            </div>
        </div>
        <br />
        <h5 v-if="countdown < 60 && countdown > 0">00:{{('0' + countdown).slice(-2)}}</h5>
    </div>
</template>

<script lang="js">
    import { defineComponent } from 'vue';
    import $ from 'jquery'
    import { loginServiceAsync } from '/src/modules/sync.module.js';
    import SimpleKeyboard from './SimpleKeyboard';

    export default defineComponent({
        name: 'LoginService',
        data() {
            return {
                pin: null,
                countdown: 60
            };
        },
        components:{
            SimpleKeyboard
        },
        created() {
            alert('!');
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
                        goToUI();
                        clearInterval(countdownTimer);
                        return;
                    }
                }, 1000);
            },
            async loginAsync() {
                let loginResult = await loginServiceAsync(this.pin);
                if (loginResult) {
                    //Sampling.toCatalog();
                }
                else {
                    this.countdown = 60;
                   // this.pin = '';
                //    this.showPinWarning = true;
                }
            },
            goToUI() {
            }
        }
    });
</script>