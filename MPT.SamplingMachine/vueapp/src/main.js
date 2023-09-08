import { createApp } from 'vue'
import App from './App.vue'
import 'bootstrap'
import { library } from '@fortawesome/fontawesome-svg-core'
import { faPhone, faHouse, faSpinner, faPlus, faRemove } from '@fortawesome/free-solid-svg-icons'
import { FontAwesomeIcon } from '@fortawesome/vue-fontawesome'
import i18n from './i18n'

library.add(faPhone, faHouse, faSpinner, faPlus, faRemove)

createApp(App).component('font-awesome-icon', FontAwesomeIcon).use(i18n).mount('#app')