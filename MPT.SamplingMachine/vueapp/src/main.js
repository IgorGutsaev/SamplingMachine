import { createApp } from 'vue'
import 'bootstrap'
import { library } from '@fortawesome/fontawesome-svg-core'
import { faPhone, faHouse, faSpinner, faPlus, faRemove } from '@fortawesome/free-solid-svg-icons'
import { FontAwesomeIcon } from '@fortawesome/vue-fontawesome'
import i18n from './i18n'
library.add(faPhone, faHouse, faSpinner, faPlus, faRemove)
import App from './App.vue'

import mitt from 'mitt';
const emitter = mitt();

const app = createApp(App);
app.config.globalProperties.emitter = emitter;
app.component('font-awesome-icon', FontAwesomeIcon).use(i18n).mount('#app');