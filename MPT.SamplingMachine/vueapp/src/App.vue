<template>
    <div id="idleModal" class="modal" tabindex="-1">
        <div class="modal-mask">
            <div class="modal-dialog modal-dialog-centered" role="document">
                <div class="modal-content">
                    <div class="modal-body">
                        <p>{{$t('titles.idleModalTitle')}}</p>
                    </div>
                    <div class="modal-footer container">
                        <div class="row" style="width: 100%">
                            <div class="col-6">
                                <button type="button" class="btn btn-danger" v-on:click="exit">{{$t('titles.exit')}} {{this.qrCountdown}}</button>
                            </div>
                            <div class="col-6">
                                <button type="button" class="btn btn-success" v-on:click="proceed">{{$t('titles.yes')}}</button>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>

    <Sampling id="app" msg="OgmentO sampling machine" />
</template>

<script>
    import $ from 'jquery'
    import Sampling from './components/SamplingPage.vue'
    import KioskSettings from '/src/modules/settings.module.js'
    import { HubConnectionBuilder, LogLevel, HttpTransportType } from '@microsoft/signalr';

    export default {
        name: 'App',
        components: {
            Sampling,
            KioskSettings
        },
        data() {
            idleTimer: null;
            exitPopupOpened: false;
        },
        created() {
             const connection = new HubConnectionBuilder()
                .withUrl('https://localhost:7244/notificationhub', {
                    skipNegotiation: true,
                    transport: HttpTransportType.WebSockets
                })
                .configureLogging(LogLevel.Information)
                .build();

                async function start() {
                    try {
                        await connection.start();
                        console.log("SignalR Connected.");
                    } catch (err) {
                        console.log(err);
                        setTimeout(start, 5000);
                    }
                };

                connection.onclose(async () => {
                    await start();
                });

                // Start the connection.
                start();

                connection.on("syncKiosk", message => Sampling.syncKiosk(message));
            let startIdleTimer = () => {
                if (KioskSettings.canLogOff && !this.exitPopupOpened)
                {
                    $('#idleModal').show();
                    let self = this;
                    self.exitPopupOpened = true;
                    let exitCountdown = 15;
                    console.log(exitCountdown + ' sec to exit');

                    let countdown = setTimeout(function tick() {
                        countdown = setTimeout(tick, 1000);
                        exitCountdown--;

                        if (exitCountdown % 5 == 0)
                            console.log(exitCountdown + ' sec to exit');

                        if (!self.exitPopupOpened) {
                            clearInterval(countdown);
                            console.log('Auto exit canceled by the customer');
                        }

                        if (exitCountdown <= 0) {
                            clearInterval(countdown);
                            exitCountdown = 0;
                            location.reload();
                            console.log('Timeout exit');
                        }
                    }, 1000);
                }
            };

            let resetTimer = () => {
                clearInterval(this.idleTimer);
                this.idleTimer = setInterval(startIdleTimer, KioskSettings.idleTimeoutSec * 1000);
            };

            window.onload = resetTimer;
            window.onmousemove = resetTimer;
            window.onmousedown = resetTimer;
            window.ontouchstart = resetTimer;
            window.onclick = resetTimer;
            window.onkeypress = resetTimer;
        },
        methods: {
            exit() {
                location.reload();
                console.log('The customer pressed exit');
            },
            proceed() {
                $('#idleModal').hide();
                this.exitPopupOpened = false;
            }
        }
    }
</script>

<style>
    @import url('css/style.css');

    #app {
        font-family: Avenir, Helvetica, Arial, sans-serif;
        -webkit-font-smoothing: antialiased;
        -moz-osx-font-smoothing: grayscale;
        text-align: center;
        color: #2c3e50;
        margin-top: 5px;
    }

    @import'~bootstrap/dist/css/bootstrap.css';
</style>
