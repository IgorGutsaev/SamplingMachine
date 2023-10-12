<template>
    <div v-if="!activeMp4 && !activeGif" class="loading" id="mainSpinner">
        <font-awesome-icon :icon="['fas', 'spinner']" size="3x" spin />
    </div>
    <video v-show="activeMp4 || activeGif" v-on:click="toLanguages" style="object-fit: fill" muted autoplay loop>
        <source type="video/mp4" src="../assets/promo/videos/demo1.mp4" />
    </video>
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
                activeGif: null
            };
        },
        components: {
            KioskSettings
        },
        created() {
            this.fetchData();
        },
        watch: {
            // call again the method if the route changes
            '$route': 'fetchData'
        },
        methods: {
            resolveActiveMedia() {
                KioskSettings.media.forEach(x => {
                    let next = KioskSettings.media.find(m => m.startTime > x.startTime);
                    if (!next) {
                        next = new Date();
                        next.setHours(23);
                        next.setMinutes(59);
                        next.setSeconds(59);
                    }

                    let now = new Date();

                    if (now >= x.startTime && now < next) {
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
            },
            fetchData() {
                let self = this;

                let waitForKioskLoadTimer = setTimeout(function tick() {
                    waitForKioskLoadTimer = setTimeout(tick, 1000);
                    if (KioskSettings.media) {
                        KioskSettings.media.forEach((x) => {
                            let time = new Date();
                            time.setHours(x.start.split(":")[0]);
                            time.setMinutes(x.start.split(":")[1]);
                            time.setSeconds(x.start.split(":")[2]);
                            x.startTime = time;
                        });

                        self.resolveActiveMedia();

                        if (!self.activeMp4 && !self.activeGif) // if there's no video, then go to the next (language selector) screen
                            Sampling.toLanguages();

                        clearInterval(waitForKioskLoadTimer);
                        return;
                    }
                }, 1000);
            },
            toLanguages() {
                Sampling.toLanguages();
            }
        }
    });
</script>