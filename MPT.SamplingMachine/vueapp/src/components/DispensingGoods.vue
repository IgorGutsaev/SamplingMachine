<template>
    <div class="post">
        <p class="h3">{{$t('titles.takeYourProducts')}}</p>
        <img id="arrow" src="../assets/arrow-down.gif" />
    </div>
</template>

<script lang="js">
    import { defineComponent } from 'vue';
    import KioskSettings from '/src/modules/settings.module.js'
    import { commitTransactionAsync, dispenseAsync } from '/src/modules/sync.module.js';
    import Transaction from '/src/modules/transaction.module.js'
    import Sampling from './SamplingPage.vue'

    export default defineComponent({
        data() {
            return {
                loading: false
            };
        },
        components: {
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
                this.loading = false;
            }
        },
        async extract(products) {
            console.log("Emulation mode: " + KioskSettings.isEmulation);
            if (KioskSettings.isEmulation) {
                new Promise(() => {
                    setTimeout(() => Sampling.toExit(), 5000)
                });
            }
            else {
                await commitTransactionAsync(Transaction.info.phone, products);
                await dispenseAsync(products);
                Sampling.toExit();
            }
        }
    });
</script>