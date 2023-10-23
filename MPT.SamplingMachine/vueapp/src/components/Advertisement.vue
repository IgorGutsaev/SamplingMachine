<template>
    <div v-if="!activeMp4 && !activeGif" class="loading" id="mainSpinner">
        <font-awesome-icon :icon="['fas', 'spinner']" size="3x" spin />
    </div>
    <video v-show="activeMp4" v-on:click="toLanguages" style="object-fit: fill" muted autoplay loop>
        <source type="video/mp4" src="../assets/promo/videos/demo1.mp4" />
    </video>
    <img v-show="activeGif" v-on:click="toLanguages" src='../assets/promo/gifs/cat-abduction.gif' class="overlay" />
</template>

<script lang="js">
    import { defineComponent } from 'vue';
    import $ from 'jquery'
    import KioskSettings from '/src/modules/settings.module.js'
    import Sampling from './SamplingPage.vue'

    export default defineComponent({
        data() {
            return {
                activeMp4: null,
                activeGif: null,
                media: null
            };
        },
        components: {
            KioskSettings
        },
        emits: ["homeButtonEnabled"],
        mounted() {
            this.emitter.on('syncKiosk', async kiosk => {
                this.resolveActiveMedia(kiosk.media);
            });
        },
        created() {
            this.fetchData();
        },
        watch: {
            // call again the method if the route changes
            '$route': 'fetchData'
        },
        methods: {
            resolveActiveMedia(media) {
                this.activeMp4 = null;
                this.activeGif = null;

                if (media.length == 0) {
                    this.$emit('homeButtonEnabled', false);
                    Sampling.toLanguages();
                }

                media.forEach(x => {
                    let time = new Date();
                    time.setHours(x.start.split(":")[0]);
                    time.setMinutes(x.start.split(":")[1]);
                    time.setSeconds(x.start.split(":")[2]);
                    x.startTime = time;
                });

                media.forEach(x => {
                    let next = media.find(m => m.startTime > x.startTime);
                    if (!next) {
                        next = {};
                        next.startTime = new Date();
                        next.startTime.setHours(23);
                        next.startTime.setMinutes(59);
                        next.startTime.setSeconds(59);
                    }

                    let now = new Date();

                    if (now >= x.startTime && now < next.startTime) {
                        if (x.media.type == 'mp4') {
                            this.activeMp4 = x.media;
                            this.activeGif = null;
                        }
                        else if (x.media.type == 'gif') {
                            this.activeMp4 = null;
                            this.activeGif = x.media;
                        }
                    }
                });

                // No active media
                if (this.activeMp4 == null && this.activeGif == null) {
                    this.$emit('homeButtonEnabled', false);
                    Sampling.toLanguages();
                }
            },
            fetchData() {
                let self = this;

                let waitForKioskLoadTimer = setTimeout(function tick() {
                    waitForKioskLoadTimer = setTimeout(tick, 1000);

                    if (KioskSettings.media) {
                        self.resolveActiveMedia(KioskSettings.media);

                        if (!self.activeMp4 && !self.activeGif) // if there's no video, then go to the next (language selector) screen
                            Sampling.toLanguages();

                        clearInterval(waitForKioskLoadTimer);
                        return;
                    }
                }, 1000);


                let startChangeMediaTimer = setTimeout(function tick() {
                    startChangeMediaTimer = setTimeout(tick, 10000);
                    self.resolveActiveMedia(KioskSettings.media);
                }, 10000);
            },
            toLanguages() {
                Sampling.toLanguages();
            }
        }
    });
</script>