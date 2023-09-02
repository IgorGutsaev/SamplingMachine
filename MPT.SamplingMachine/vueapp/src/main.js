import { createApp } from 'vue'
import App from './App.vue'
import 'bootstrap'
import { library } from '@fortawesome/fontawesome-svg-core'
import { faPhone, faHouse, faSpinner } from '@fortawesome/free-solid-svg-icons'
import { FontAwesomeIcon } from '@fortawesome/vue-fontawesome'

library.add(faPhone, faHouse, faSpinner)

createApp(App).component('font-awesome-icon', FontAwesomeIcon).mount('#app')