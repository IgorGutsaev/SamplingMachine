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
                                <button type="button" class="btn btn-danger" v-on:click="exit">{{$t('titles.exit')}}</button>
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

    export default {
        name: 'App',
        components: {
          Sampling,
          KioskSettings
        },
        methods: {
            exit() {
                location.reload();
            },
            proceed() {
                $('#idleModal').hide();
            }
        }
    }

    let idleTimer;
          
    function resetTimer() {
        clearInterval(idleTimer);
        idleTimer = setInterval(startIdleTimer, KioskSettings.idleTimeoutSec * 1000);
    }
          
    window.onload = resetTimer;
    window.onmousemove = resetTimer;
    window.onmousedown = resetTimer;
    window.ontouchstart = resetTimer;
    window.onclick = resetTimer;
    window.onkeypress = resetTimer;
          
    function startIdleTimer() {
        if (KioskSettings.canLogOff)
            $('#idleModal').show();
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
